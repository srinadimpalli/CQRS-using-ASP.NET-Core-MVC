using AspNetCoreFactory.CQRS.Core.Domain;
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

            private readonly CQRSContext _db;

            public QueryHandler(CQRSContext db)
            {
                _db = db;
            }

            protected override Result Handle(Query query)
            {
                return new Result
                {
                    TotalPlanes = _db.Plane.Count(),
                    TotalFlights = _db.Flight.Count(),
                    TotalSeats = _db.Seat.Count(),
                    TotalTravelers = _db.Traveler.Count(),
                    TotalBookings = _db.Booking.Count(),
                    TotalEvents = _db.Event.Count()
                };
            }
        }
    }
}
