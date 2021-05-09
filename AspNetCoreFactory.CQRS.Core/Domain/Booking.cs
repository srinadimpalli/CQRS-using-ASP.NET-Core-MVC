using System;
using System.Collections.Generic;

namespace AspNetCoreFactory.CQRS.Core.Domain
{
    public partial class Booking
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int SeatId { get; set; }
        public int TravelerId { get; set; }
        public DateTime BookingDate { get; set; }
        public string BookingNumber { get; set; }

        public virtual Flight Flight { get; set; }
        public virtual Seat Seat { get; set; }
        public virtual Traveler Traveler { get; set; }
    }
}
