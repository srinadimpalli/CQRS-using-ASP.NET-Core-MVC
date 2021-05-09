using AspNetCoreFactory.CQRS.Core.Domain;
using MediatR;

namespace AspNetCoreFactory.CQRS.Core.Areas.Flight
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

            public CommandHandler(CQRSContext db, ICache cache)
            {
                _db = db;
                _cache = cache;
            }

            protected override void Handle(Command message)
            {
                var flight = _db.Flight.Find(message.Id);

                _db.Flight.Remove(flight);
                _db.SaveChanges();

                _cache.DeleteFlight(flight);
            }
        }
    }
}
