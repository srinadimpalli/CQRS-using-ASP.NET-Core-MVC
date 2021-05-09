using AspNetCoreFactory.CQRS.Core.Domain;
using MediatR;

namespace AspNetCoreFactory.CQRS.Core.Areas.Booking
{
    // ** Command Query pattern

    public class Delete
    {
        // Input

        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        // Process

        public class CommandHandler : RequestHandler<Command>
        {
            // ** DI Pattern

            private readonly CQRSContext _db;
            private readonly ICache _cache;
            private readonly IRollup _rollup;
            private readonly IEvent _event;

            public CommandHandler(CQRSContext db, ICache cache, IRollup rollup, IEvent @event)
            {
                _db = db;
                _cache = cache;
                _rollup = rollup;
                _event = @event;
            }

            protected override void Handle(Command message)
            {
                var booking = _db.Booking.Find(message.Id);

                // ** Event Sourcing pattern

                _event.DeleteBooking(booking);

                // Eventual consistency

                _db.Booking.Remove(booking);
                _db.SaveChanges();

                _rollup.TotalBookings(booking);
            }
        }
    }
}
