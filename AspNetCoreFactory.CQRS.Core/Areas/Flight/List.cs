using AspNetCoreFactory.CQRS.Core.Domain;
using MediatR;
using System.Collections.Generic;

namespace AspNetCoreFactory.CQRS.Core.Areas.Flight
{
    // ** Command Query pattern

    public class List
    {
        // Input (** DTO Pattern)

        public class Query : IRequest<Result> { }

        // Output (** DTO Pattern)

        public class Result
        {
            public List<Flight> Flights { get; set; } = new List<Flight>();

            public class Flight
            {
                public int Id { get; set; }
                public string Plane { get; set; }
                public string FlightNumber { get; set; }
                public string From { get; set; }
                public string To { get; set; }
                public string Departure { get; set; }
                public string Arrival { get; set; }
                public int TotalBookings { get; set; }
            }
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

            protected override Result Handle(Query request)
            {
                var result = new Result();

                foreach (var flight in _db.Flight)
                {
                    var plane = _cache.Planes[flight.PlaneId];

                    result.Flights.Add(new Result.Flight
                    {
                        // ** Data Mapper Pattern

                        Id = flight.Id,
                        Plane = plane.Model,
                        FlightNumber = flight.FlightNumber,
                        From = flight.From,
                        To = flight.To,
                        Departure = flight.Departure.ToString("dd MMM yyyy, hh:mm tt"),
                        Arrival = flight.Arrival.ToString("dd MMM yyyy, hh:mm tt"),
                        TotalBookings = flight.TotalBookings
                    });
                }

                return result;
            }
        }
    }
}
