using AspNetCoreFactory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreFactory.Domain.Services
{
    public interface IServiceManager
    {
        ITravelerService Traveler { get; }
        IBookingService Booking { get; }
        ISeatService Seat { get; }
        IPlaneService Plane { get; }
        IFlightService Flight { get; }
        IEventService Event { get; }
        Task SaveAsync();
        void Save();
    }
    public class ServiceManager : IServiceManager
    {
        private readonly CQRSContext _cQRSContext;
        private ITravelerService _travelService;
        private IBookingService _bookingService;
        private ISeatService _seatService;
        private IPlaneService _planeService;
        private IFlightService _flightService;
        private IEventService _eventService;
        public ServiceManager(CQRSContext cQRSContext, ITravelerService travelerService,
                                                       IBookingService bookingService,
                                                       ISeatService seatService,
                                                       IPlaneService planeService,
                                                       IFlightService flightService,
                                                       IEventService eventService)
        {
            _cQRSContext = cQRSContext;
            _travelService = travelerService;
            _bookingService = bookingService;
            _seatService = seatService;
            _planeService = planeService;
            _flightService = flightService;
            _eventService = eventService;
        }
        public ITravelerService Traveler
        {
            get
            {
                return _travelService;
            }
        }
        public IBookingService Booking
        {
            get
            {
                return _bookingService;
            }
        }
        public ISeatService Seat
        {
            get
            {
                return _seatService;
            }
        }
        public IPlaneService Plane
        {
            get
            {
                return _planeService;
            }
        }
        public IFlightService Flight
        {
            get
            {
                return _flightService;
            }
        }

        public IEventService Event => _eventService;


        public Task SaveAsync() => _cQRSContext.SaveChangesAsync();
        public void Save() => _cQRSContext.SaveChanges();
    }
}
