using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AspNetCoreFactory.CQRS.Core.Domain
{
    public partial class CQRSContext : DbContext
    {
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Flight> Flight { get; set; }
        public virtual DbSet<Plane> Plane { get; set; }
        public virtual DbSet<Seat> Seat { get; set; }
        public virtual DbSet<Traveler> Traveler { get; set; }

        public CQRSContext(){ }

        public CQRSContext(DbContextOptions<CQRSContext> options) : base(options) { }
    }
}
