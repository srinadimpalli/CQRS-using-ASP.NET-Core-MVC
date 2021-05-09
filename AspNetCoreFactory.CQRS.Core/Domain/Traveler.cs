using System;
using System.Collections.Generic;

namespace AspNetCoreFactory.CQRS.Core.Domain
{
    public partial class Traveler
    {
        public Traveler()
        {
            Booking = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int TotalBookings { get; set; }

        public virtual ICollection<Booking> Booking { get; set; }
    }
}
