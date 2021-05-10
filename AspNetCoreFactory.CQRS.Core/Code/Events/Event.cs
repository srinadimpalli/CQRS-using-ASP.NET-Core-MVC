using System;
using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using Newtonsoft.Json;

namespace AspNetCoreFactory.CQRS.Core
{
    #region Interface

    public interface IEvent
    {
        void InsertBooking(Booking booking);  // insert
        void DeleteBooking(Booking booking);  // delete
        void UpdateBooking(Booking oldBooking, Booking newBooking);  // delete
    }

    #endregion

    public class Event : IEvent
    {
        #region Dependency Injection 
        private readonly IServiceManager _serviceManager;

        public Event(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        #endregion

        public void InsertBooking(Booking booking)
        {
            var evt = new Domain.Entities.Event
            {
                Action = "Insert",
                Transaction = Guid.NewGuid().ToString(),
                Table = "Booking",
                TableId = booking.Id,
                Content = JsonConvert.SerializeObject(booking),
                EventDate = DateTime.Now
            };
            _serviceManager.Event.CreateEvent(evt);
            _serviceManager.Save();
        }

        public void UpdateBooking(Booking oldBooking, Booking newBooking)
        {
            bool flightChanged = oldBooking.FlightId != newBooking.FlightId;
            bool seatChanged = oldBooking.SeatId != newBooking.SeatId;
            if (!flightChanged && !seatChanged) return;

            var evt = new Domain.Entities.Event
            {
                Action = "Update",
                Transaction = Guid.NewGuid().ToString(),
                Table = "Booking",
                TableId = newBooking.Id,
                Content = JsonConvert.SerializeObject(newBooking),
                EventDate = DateTime.UtcNow
            };
            _serviceManager.Event.CreateEvent(evt);
            _serviceManager.Save();
        }

        public void DeleteBooking(Booking booking)
        {
            var evt = new Domain.Entities.Event
            {
                Action = "Delete",
                Transaction = Guid.NewGuid().ToString(),
                Table = "Booking",
                TableId = booking.Id,
                Content = $"{{Id : {booking.Id}}}",
                EventDate = DateTime.UtcNow
            };
            _serviceManager.Event.CreateEvent(evt);
            _serviceManager.Save();
        }
    }
}
