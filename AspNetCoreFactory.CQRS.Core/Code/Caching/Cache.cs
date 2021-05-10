using AspNetCoreFactory.Domain.Entities;// .CQRS.Core.Domain;
using AspNetCoreFactory.Domain.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreFactory.CQRS.Core
{
    #region Interface

    public interface ICache
    {
        void EagerLoad();

        Dictionary<int, Plane> Planes { get; }
        void ClearPlanes();

        Dictionary<int, Seat> Seats { get; }
        void AddSeat(Seat seat);
        void UpdateSeat(Seat seat);
        void DeleteSeat(Seat seat);
        void ClearSeats();

        Dictionary<int, Traveler> Travelers { get; }
        void AddTraveler(Traveler traveler);
        void UpdateTraveler(Traveler traveler);
        void DeleteTraveler(Traveler traveler);
        void ClearTravelers();

        Dictionary<int, Flight> Flights { get; }
        void AddFlight(Flight flight);
        void UpdateFlight(Flight flight);
        void DeleteFlight(Flight flight);
        void ClearFlights();
    }

    #endregion

    public class Cache : ICache
    {
        #region Dependency Injection
        private readonly IServiceManager _serviceManager;
        private readonly IMemoryCache _memoryCache;

        public Cache(IMemoryCache memoryCache, IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
            _memoryCache = memoryCache;

            EagerLoad();
        }

        #endregion

        #region Eager Load

        // ** Eager Load patterns

        // Static constructor would not work 
        // because injected services are not availble.

        private static bool _eagerLoaded = false;

        public void EagerLoad()
        {
            if (!_eagerLoaded)
            {
                _ = Planes;
                _ = Seats;
                _ = Travelers;
                _ = Flights;

                _eagerLoaded = true;
            }
        }

        #endregion

        #region Cache management

        private static readonly object locker = new object();

        private static readonly string PlanesKey = nameof(PlanesKey);
        private static readonly string SeatsKey = nameof(SeatsKey);
        private static readonly string TravelersKey = nameof(TravelersKey);
        private static readonly string FlightsKey = nameof(FlightsKey);

        private static readonly HashSet<string> UsedKeys = new HashSet<string>();

        #endregion

        #region Caches

        // ** Proxy pattern

        // ** Identity Map pattern

        public Dictionary<int, Plane> Planes
        {
            get
            {
                // ** Lazy Load pattern 

                if (!(_memoryCache.Get(PlanesKey) is Dictionary<int, Plane> dictionary))
                {
                    lock (locker)
                    {
                        dictionary = _serviceManager.Plane.GetPlanes(trackChanges: false).ToDictionary(p => p.Id);
                        Add(PlanesKey, dictionary, DateTime.Now.AddHours(24));
                    }
                }

                return dictionary;
            }
        }

        // Clears all planes

        public void ClearPlanes()
        {
            Clear(PlanesKey);
        }

        // ** Identity Map pattern

        public Dictionary<int, Seat> Seats
        {
            get
            {
                // ** Lazy Load pattern 

                if (!(_memoryCache.Get(SeatsKey) is Dictionary<int, Seat> dictionary))
                {
                    lock (locker)
                    {
                        dictionary = _serviceManager.Seat.GetSeats(trackChanges: false).ToDictionary(s => s.Id);
                        Add(SeatsKey, dictionary, DateTime.Now.AddHours(24));
                    }
                }

                return dictionary;
            }
        }

        public void AddSeat(Seat seat)
        {
            lock (locker)
            {
                if (!Seats.ContainsKey(seat.Id))
                    Seats.Add(seat.Id, seat);
            }
        }

        public void UpdateSeat(Seat seat)
        {
            lock (locker)
            {
                Seats[seat.Id] = seat;
            }
        }

        public void DeleteSeat(Seat seat)
        {
            lock (locker)
            {
                Seats.Remove(seat.Id);
            }
        }

        public void ClearSeats()
        {
            Clear(SeatsKey);
        }

        // ** Identity Map pattern

        public Dictionary<int, Traveler> Travelers
        {
            get
            {
                // ** Lazy Load pattern 

                if (!(_memoryCache.Get(TravelersKey) is Dictionary<int, Traveler> dictionary))
                {
                    lock (locker)
                    {
                        dictionary = _serviceManager.Traveler.GetTravelers(trackChanges: false).ToDictionary(t => t.Id);
                        Add(TravelersKey, dictionary, DateTime.Now.AddHours(4));
                    }
                }

                return dictionary;
            }
        }

        public void AddTraveler(Traveler traveler)
        {
            lock (locker)
            {
                if (!Travelers.ContainsKey(traveler.Id))
                    Travelers.Add(traveler.Id, traveler);
            }
        }

        public void UpdateTraveler(Traveler traveler)
        {
            lock (locker)
            {
                Travelers[traveler.Id] = traveler;
            }
        }

        public void DeleteTraveler(Traveler traveler)
        {
            lock (locker)
            {
                Travelers.Remove(traveler.Id);
            }
        }

        public void ClearTravelers()
        {
            Clear(TravelersKey);
        }

        // ** Identity Map pattern

        public Dictionary<int, Flight> Flights
        {
            get
            {
                // ** Lazy Load pattern 

                if (!(_memoryCache.Get(FlightsKey) is Dictionary<int, Flight> dictionary))
                {
                    lock (locker)
                    {
                        dictionary = _serviceManager.Flight.GetFlights(trackChanges: false).ToDictionary(f => f.Id);
                        Add(FlightsKey, dictionary, DateTime.Now.AddHours(4));
                    }
                }

                return dictionary;
            }
        }

        public void AddFlight(Flight flight)
        {
            lock (locker)
            {
                if (!Flights.ContainsKey(flight.Id))
                    Flights.Add(flight.Id, flight);
            }
        }

        public void UpdateFlight(Flight flight)
        {
            lock (locker)
            {
                Flights[flight.Id] = flight;
            }
        }

        public void DeleteFlight(Flight flight)
        {
            lock (locker)
            {
                Flights.Remove(flight.Id);
            }
        }

        public void ClearFlights()
        {
            Clear(FlightsKey);
        }

        #endregion

        #region Cache Helpers

        // Clears single cache entry

        private void Clear(string key)
        {
            lock (locker)
            {
                _memoryCache.Remove(key);

                UsedKeys.Remove(key);
            }
        }

        // Clears entire cache

        public void Clear()
        {
            lock (locker)
            {
                foreach (var usedKey in UsedKeys)
                    _memoryCache.Remove(usedKey);

                UsedKeys.Clear();
            }
        }

        // Add item to cache 

        private void Add(string key, object value, DateTimeOffset expiration)
        {
            _memoryCache.Set(key, value,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiration));

            UsedKeys.Add(key);
        }

        #endregion
    }
}
