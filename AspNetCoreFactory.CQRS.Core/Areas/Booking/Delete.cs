using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
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
            private readonly IServiceManager _serviceManager;
            private readonly ICache _cache;
            private readonly IRollup _rollup;
            private readonly IEvent _event;

            public CommandHandler(IServiceManager serviceManager, ICache cache, IRollup rollup, IEvent @event)
            {
                _serviceManager = serviceManager;
                _cache = cache;
                _rollup = rollup;
                _event = @event;
            }

            protected override void Handle(Command message)
            {
                var booking = _serviceManager.Booking.GetBooking(message.Id, trackChanges: false);

                // ** Event Sourcing pattern

                _event.DeleteBooking(booking);

                // Eventual consistency
                _serviceManager.Booking.DeleteBooking(booking);
                _serviceManager.Save();

                _rollup.TotalBookings(booking);
            }
        }
    }
}
