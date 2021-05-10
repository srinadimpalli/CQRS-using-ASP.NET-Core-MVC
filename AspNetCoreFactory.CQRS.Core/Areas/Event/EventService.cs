using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreFactory.CQRS.Core.Areas.Event
{
    public class EventService : ServiceBase<Domain.Entities.Event>, IEventService
    {
        public EventService(CQRSContext cQRSContext) : base(cQRSContext) { }
        public IEnumerable<Domain.Entities.Event> GetEvents(bool trackChanges)
        {
            return FindAll(trackChanges).ToList().OrderBy(e => e.Id).ToList();
        }
        public int GetCount() => Count();

        public void CreateEvent(Domain.Entities.Event evnt)
        {
            Create(evnt);
        }

    }
}
