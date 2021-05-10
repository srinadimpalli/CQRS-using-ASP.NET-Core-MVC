using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using MediatR;
using System.Collections.Generic;

namespace AspNetCoreFactory.CQRS.Core.Areas.Plane
{
    // ** Command Query pattern

    public class List
    {
        // Input

        public class Query : IRequest<Result> { }

        // Output

        public class Result
        {
            public List<Plane> Planes { get; set; } = new List<Plane>();

            public class Plane
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public string Model { get; set; }
                public string SerialNumber { get; set; }
                public int TotalSeats { get; set; }
            }
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
                var result = new Result();

                var planes = _serviceManager.Plane.GetPlanes(trackChanges: false);
                foreach (var plane in planes)
                {
                    result.Planes.Add(new Result.Plane
                    {
                        // ** Data Mapper Pattern

                        Id = plane.Id,
                        Name = plane.Name,
                        Model = plane.Model,
                        SerialNumber = plane.SerialNumber,
                        TotalSeats = plane.TotalSeats
                    });
                }

                return result;
            }
        }
    }
}
