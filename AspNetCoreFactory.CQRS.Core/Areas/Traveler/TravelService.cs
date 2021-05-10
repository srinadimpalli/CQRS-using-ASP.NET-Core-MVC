using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain;
using AspNetCoreFactory.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreFactory.CQRS.Core.Areas.Traveler
{
    public class TravelService : ServiceBase<Domain.Entities.Traveler>, ITravelerService
    {
        public TravelService(CQRSContext cQRSContext) : base(cQRSContext)
        {

        }
        public void CreateTraveler(Domain.Entities.Traveler traveler)
        {
            Create(traveler);
        }

        public void DeleteTraveler(Domain.Entities.Traveler traveler)
        {
            Delete(traveler);
        }
        public void UpdateTraveler(Domain.Entities.Traveler traveler)
        {
            Update(traveler);
        }
        public Domain.Entities.Traveler GetTraveler(int id, bool trackChanges)
        {
            return FindByCondition(t => t.Id.Equals(id), trackChanges).SingleOrDefault();
        }

        public async Task<Domain.Entities.Traveler> GetTravelerAsync(int id, bool trackChanges)
        {
            return await FindByCondition(t => t.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }

        public IEnumerable<Domain.Entities.Traveler> GetTravelers(bool trackChanges)
        {
            return FindAll(trackChanges).ToList().OrderBy(c => c.FirstName).ToList();
        }

        public async Task<IEnumerable<Domain.Entities.Traveler>> GetTravelersAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                   .OrderBy(t => t.FirstName)
                   .ToListAsync();
        }
        public int GetCount() => Count();
    }
}
