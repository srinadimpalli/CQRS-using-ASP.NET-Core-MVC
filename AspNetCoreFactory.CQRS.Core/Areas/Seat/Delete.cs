using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
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
            private readonly IServiceManager _serviceManager;
            private readonly ICache _cache;
            private readonly IRollup _rollup;

            public CommandHandler(IServiceManager serviceManager, ICache cache, IRollup rollup)
            {
                _serviceManager = serviceManager;
                _cache = cache;
                _rollup = rollup;
            }

            protected override void Handle(Command message)
            {
                var seat = _serviceManager.Seat.GetSeat(message.Id, trackChanges: true);
                _serviceManager.Seat.DeleteSeat(seat);
                _serviceManager.Save();

                _cache.DeleteSeat(seat);
                _rollup.TotalSeatsByPlane(seat.PlaneId);

            }
        }
    }
}
