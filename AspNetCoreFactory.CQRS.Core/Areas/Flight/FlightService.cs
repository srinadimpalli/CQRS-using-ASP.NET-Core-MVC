using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreFactory.CQRS.Core.Areas.Flight
{
    public class FlightService : ServiceBase<Domain.Entities.Flight>, IFlightService
    {
        public FlightService(CQRSContext cQRSContext) : base(cQRSContext) { }
        public IQueryable<Domain.Entities.Flight> AsQueryable()
        {
            throw new NotImplementedException();
        }

        public void CreateFlight(Domain.Entities.Flight flight)
        {
            Create(flight);
        }
        public void UpdateFlight(Domain.Entities.Flight flight)
        {
            Update(flight);
        }
        public void DeleteFlight(Domain.Entities.Flight flight)
        {
            Delete(flight);
        }

        public Domain.Entities.Flight GetFlight(int id, bool trackChanges)
        {
            return FindByCondition(f => f.Id.Equals(id), trackChanges: false)
                  .SingleOrDefault();
        }

        public async Task<Domain.Entities.Flight> GetFlightAsync(int id, bool trackChanges)
        {
            return await FindByCondition(f => f.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }

        public IEnumerable<Domain.Entities.Flight> GetFlights(bool trackChanges)
        {
            return FindAll(trackChanges).ToList().OrderBy(f => f.FlightNumber).ToList();
        }

        public async Task<IEnumerable<Domain.Entities.Flight>> GetFlightsAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                  .OrderBy(f => f.FlightNumber)
                  .ToListAsync();
        }
        public int GetCount() => Count();
    }
}
