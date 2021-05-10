using System;
using System.Collections.Generic;

namespace AspNetCoreFactory.Domain.Entities
{
    public partial class Flight
    {
        public Flight()
        {
            Booking = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public int PlaneId { get; set; }
        public string FlightNumber { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public int TotalBookings { get; set; }

        public virtual Plane Plane { get; set; }
        public virtual ICollection<Booking> Booking { get; set; }
    }
}
