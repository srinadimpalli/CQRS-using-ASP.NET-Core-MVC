using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreFactory.CQRS.Core.Areas.Traveler
{
    // ** Command Query pattern

    public class List
    {
        // Input (DTO Pattern)

        public class Query : IRequest<Result> { }

        // Output (DTO Pattern)

        public class Result
        {
            public List<Traveler> Travelers { get; set; } = new List<Traveler>();

            public class Traveler
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public string Email { get; set; }
                public string City { get; set; }
                public string Country { get; set; }
                public int TotalBookings { get; set; }
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

            protected override Result Handle(Query request)
            {
                var result = new Result();
                var travelers = _serviceManager.Traveler.GetTravelers(trackChanges: false);
                foreach (var traveler in travelers)
                {
                    // ** Data Mapper pattern
                    result.Travelers.Add(new Result.Traveler
                    {
                        Id = traveler.Id,
                        Name = traveler.Name,
                        FirstName = traveler.FirstName,
                        LastName = traveler.LastName,
                        Email = traveler.Email,
                        City = traveler.City,
                        Country = traveler.Country,
                        TotalBookings = traveler.TotalBookings
                    });
                }

                return result;
            }
        }
    }
}
