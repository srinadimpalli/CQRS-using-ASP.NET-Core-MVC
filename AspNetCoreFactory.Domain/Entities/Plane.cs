using System;
using System.Collections.Generic;

namespace AspNetCoreFactory.Domain.Entities
{
    public partial class Plane
    {
        public Plane()
        {
            Flight = new HashSet<Flight>();
            Seat = new HashSet<Seat>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public int TotalSeats { get; set; }

        public virtual ICollection<Flight> Flight { get; set; }
        public virtual ICollection<Seat> Seat { get; set; }
    }
}
