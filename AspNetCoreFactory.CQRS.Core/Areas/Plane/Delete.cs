using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using MediatR;

namespace AspNetCoreFactory.CQRS.Core.Areas.Plane
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

            public CommandHandler(IServiceManager serviceManager, ICache cache)
            {
                _serviceManager = serviceManager;
                _cache = cache;
            }

            protected override void Handle(Command message)
            {
                var plane = _serviceManager.Plane.GetPlane(message.Id, trackChanges: true);
                _serviceManager.Plane.DeletePlane(plane);
                _serviceManager.Save();

                _cache.ClearPlanes();

            }
        }
    }
}
