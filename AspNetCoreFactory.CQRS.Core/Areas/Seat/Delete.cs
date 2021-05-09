using AspNetCoreFactory.CQRS.Core.Domain;
using MediatR;

namespace AspNetCoreFactory.CQRS.Core.Areas.Seat
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

            public CommandHandler(CQRSContext db, ICache cache, IRollup rollup)
            {
                _db = db;
                _cache = cache;
                _rollup = rollup;
            }

            protected override void Handle(Command message)
            {
                var seat = _db.Seat.Find(message.Id);

                _db.Seat.Remove(seat);
                _db.SaveChanges();

                _cache.DeleteSeat(seat);
                _rollup.TotalSeatsByPlane(seat.PlaneId);
                
            }
        }
    }
}
