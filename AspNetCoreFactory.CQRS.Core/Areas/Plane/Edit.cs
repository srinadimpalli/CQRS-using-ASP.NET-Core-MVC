using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AspNetCoreFactory.CQRS.Core.Areas.Plane
{
    // ** Command Query pattern

    public class Edit
    {
        // Query Input

        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        // Query Process

        public class QueryHandler : RequestHandler<Query, Command>
        {
            // ** DI Pattern
            private readonly IServiceManager _serviceManager;

            public QueryHandler(IServiceManager serviceManager)
            {
                _serviceManager = serviceManager;
            }

            protected override Command Handle(Query message)
            {
                var command = new Command();

                var plane = _serviceManager.Plane.GetPlane(message.Id, trackChanges: false);
                if (plane != null)
                {
                    // ** Data Mapper pattern

                    command.Id = plane.Id;
                    command.Name = plane.Name;
                    command.Model = plane.Model;
                    command.SerialNumber = plane.SerialNumber;
                    command.TotalSeats = plane.TotalSeats;
                }

                return command;
            }
        }

        // Request Output. Command Input (** DTO Pattern)

        public class Command : IRequest
        {
            public int Id { get; set; }
            [Required(ErrorMessage = "Name is required")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Model is required")]
            public string Model { get; set; }
            [Required(ErrorMessage = "Serial Number is required")]
            public string SerialNumber { get; set; }
            public int TotalSeats { get; set; }
        }

        // Command Process

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
                if (message.Id == 0) // new plane
                {
                    // ** Data Mapper Pattern

                    var plane = new Domain.Entities.Plane();
                    plane.Name = message.Name;
                    plane.Model = message.Model;
                    plane.SerialNumber = message.SerialNumber;
                    _serviceManager.Plane.CreatePlane(plane);
                }
                else // update plane
                {
                    // ** Data Mapper Pattern

                    var plane = _serviceManager.Plane.GetPlane(message.Id, trackChanges: false);
                    plane.Name = message.Name;
                    plane.Model = message.Model;
                    plane.SerialNumber = message.SerialNumber;
                    _serviceManager.Plane.UpdatePlane(plane);
                }

                _serviceManager.Save();

                _cache.ClearPlanes();
            }
        }
    }
}
