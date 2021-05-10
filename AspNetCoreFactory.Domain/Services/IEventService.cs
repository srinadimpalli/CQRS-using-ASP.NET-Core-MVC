using AspNetCoreFactory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreFactory.Domain.Services
{
    public interface IEventService
    {
        IEnumerable<Event> GetEvents(bool trackChanges);
        int GetCount();
        void CreateEvent(Event evnt);
    }
}
