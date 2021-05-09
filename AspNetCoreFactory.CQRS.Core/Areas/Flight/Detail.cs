using AspNetCoreFactory.CQRS.Core.Domain;
using MediatR;
using System.Linq;

namespace AspNetCoreFactory.CQRS.Core.Areas.Flight
{
    // ** Command Query pattern

    public class Detail
    {
        // Input (** DTO Pattern)

        public class Query : IRequest<Result>
        {
            public int Id { get; set; }
        }

        // Output (** DTO Pattern)

        public class Result
        {
            public int Id { get; set; }
            public int PlaneId { get; set; }
            public string Plane { get; set; }
            public string FlightNumber { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            public string Departure { get; set; }
            public string Arrival { get; set; }
            public int TotalBookings { get; set; }
        }

        // Process

        public class QueryHandler : RequestHandler<Query, Result>
        {
            // ** DI Pattern

            private readonly CQRSContext _db;
            private readonly ICache _cache;

            public QueryHandler(CQRSContext db, ICache cache)
            {
                _db = db;
                _cache = cache;
            }

            protected override Result Handle(Query query)
            {
                var result = new Result();

                var flight = _db.Flight.SingleOrDefault(p => p.Id == query.Id);
                if (flight != null)
                {
                    // ** Data Mapper Pattern

                    result.Id = flight.Id;
                    result.PlaneId = flight.PlaneId;
                    result.Plane = _cache.Planes[flight.PlaneId].Model;
                    result.FlightNumber = flight.FlightNumber;
                    result.From = flight.From;
                    result.To = flight.To;
                    result.Departure = flight.Departure.ToString("MM/dd/yyyy hh:mm");
                    result.Arrival = flight.Arrival.ToString("MM/dd/yyyy hh:mm");
                    result.TotalBookings = flight.TotalBookings;
                }

                return result;
            }
        }
    }
}
