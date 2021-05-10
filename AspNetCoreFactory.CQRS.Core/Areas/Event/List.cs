using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using MediatR;
using System.Collections.Generic;

namespace AspNetCoreFactory.CQRS.Core.Areas.Event
{
    // ** Command Query pattern

    public class List
    {
        // Input

        public class Query : IRequest<Result> { }

        // Output

        public class Result
        {
            public List<Event> Events { get; set; } = new List<Event>();

            public class Event
            {
                public int Id { get; set; }
                public string Transaction { get; set; }
                public string EventDate { get; set; }
                public string Action { get; set; }
                public string Table { get; set; }
                public int TableId { get; set; }
                public int Version { get; set; }
                public string Content { get; set; }
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
                var events = _serviceManager.Event.GetEvents(trackChanges: false);

                foreach (var evt in events)
                {
                    result.Events.Add(new Result.Event
                    {
                        // ** Data Mapper Pattern

                        Id = evt.Id,
                        Transaction = evt.Transaction,
                        EventDate = evt.EventDate.ToString(),
                        Action = evt.Action,
                        Table = evt.Table,
                        TableId = evt.TableId,
                        Version = evt.Version,
                        Content = evt.Content
                    });
                }

                return result;
            }
        }
    }
}
