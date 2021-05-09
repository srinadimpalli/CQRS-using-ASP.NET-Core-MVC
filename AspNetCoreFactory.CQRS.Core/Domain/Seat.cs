using System;
using System.Collections.Generic;

namespace AspNetCoreFactory.CQRS.Core.Domain
{
    public partial class Seat
    {
        public Seat()
        {
            Booking = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public int PlaneId { get; set; }
        public string Number { get; set; }
        public string Location { get; set; }
        public int TotalBookings { get; set; }

        public virtual Plane Plane { get; set; }
        public virtual ICollection<Booking> Booking { get; set; }
    }
}
