using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreFactory.CQRS.Core.Areas.Admin
{
    // ** Command Query pattern

    public class List
    {
        // Input

        public class Query : IRequest<Result> { }

        // Output

        public class Result
        {
            public int TotalPlanes { get; set; }
            public int TotalFlights { get; set; }
            public int TotalSeats { get; set; }
            public int TotalTravelers { get; set; }
            public int TotalBookings { get; set; }
            public int TotalEvents { get; set; }
        }

        // Process

        public class QueryHandler : RequestHandler<Query, Result>
        {
            // ** DI Pattern
            private readonly IServiceManager _serviceManager;

            public QueryHandler(IServiceManager serviceManager)
            {
                _serviceManager = serviceManager;
            }

            protected override Result Handle(Query query)
            {
                return new Result
                {
                    TotalPlanes = _serviceManager.Plane.GetCount(),
                    TotalFlights = _serviceManager.Flight.GetCount(),
                    TotalSeats = _serviceManager.Seat.GetCount(),
                    TotalTravelers = _serviceManager.Traveler.GetCount(),
                    TotalBookings = _serviceManager.Booking.GetCount(),
                    TotalEvents = _serviceManager.Event.GetCount()
                };
            }
        }
    }
}
