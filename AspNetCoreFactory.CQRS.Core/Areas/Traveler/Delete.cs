using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreFactory.CQRS.Core.Areas.Traveler
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

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            // ** DI Pattern
            private readonly IServiceManager _serviceManager;
            private readonly ICache _cache;

            public CommandHandler(ICache cache, IServiceManager serviceManager)
            {
                _serviceManager = serviceManager;
                _cache = cache;
            }

            protected override async Task Handle(Command message, CancellationToken cancellationToken)
            {
                var traveler = await _serviceManager.Traveler.GetTravelerAsync(message.Id, trackChanges: false);
                _serviceManager.Traveler.DeleteTraveler(traveler);
                _serviceManager.Save();

                _cache.DeleteTraveler(traveler);
            }
        }
    }
}
