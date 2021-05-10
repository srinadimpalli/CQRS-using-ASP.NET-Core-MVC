using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreFactory.CQRS.Core.Areas.Seat
{
    // ** Command Query pattern

    public class List
    {
        // Input 

        public class Query : IRequest<Result>
        {
            public int? PlaneId { get; set; }
        }

        // Output

        public class Result
        {
            public int? PlaneId { get; set; }
            public List<Seat> Seats { get; set; } = new List<Seat>();

            public class Seat
            {
                public int Id { get; set; }
                public int PlaneId { get; set; }
                public string Plane { get; set; }
                public string Number { get; set; }
                public string Location { get; set; }
                public int TotalBookings { get; set; }
            }
        }

        // Process

        public class QueryHandler : RequestHandler<Query, Result>
        {
            // ** DI Pattern
            private readonly IServiceManager _serviceManager;
            private readonly ICache _cache;

            public QueryHandler(IServiceManager serviceManager, ICache cache)
            {
                _serviceManager = serviceManager;
                _cache = cache;
            }

            protected override Result Handle(Query query)
            {
                var result = new Result { PlaneId = query.PlaneId };
                var seats = _serviceManager.Seat.AsQueryable();
                if (query.PlaneId != null)
                    seats = seats.Where(s => s.PlaneId == query.PlaneId);

                seats = seats.OrderBy(s => s.PlaneId).ThenBy(s => s.Number);

                // ** Iterator Pattern

                foreach (var seat in seats)
                {
                    var plane = _cache.Planes[seat.PlaneId];
                    result.Seats.Add(new Result.Seat
                    {
                        // ** Data Mapper Pattern

                        Id = seat.Id,
                        PlaneId = seat.PlaneId,
                        Plane = plane.Model + ": " + plane.Name,
                        Number = seat.Number,
                        Location = seat.Location,
                        TotalBookings = seat.TotalBookings
                    });
                }

                return result;
            }
        }
    }
}