using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreFactory.CQRS.Core.Areas.Plane
{
    public class PlaneService : ServiceBase<Domain.Entities.Plane>, IPlaneService
    {
        private readonly CQRSContext _cQRSContext;
        public PlaneService(CQRSContext cQRSContext) : base(cQRSContext)
        {
            _cQRSContext = cQRSContext;
        }
        public void CreatePlane(Domain.Entities.Plane place)
        {
            Create(place);
        }

        public void DeletePlane(Domain.Entities.Plane plane)
        {
            Delete(plane);
        }
        public void UpdatePlane(Domain.Entities.Plane plane)
        {
            Update(plane);
        }

        public Domain.Entities.Plane GetPlane(int id, bool trackChanges)
        {
            return FindByCondition(p => p.Id.Equals(id), trackChanges)
                    .SingleOrDefault();
        }

        public async Task<Domain.Entities.Plane> GetPlaneAsync(int id, bool trackChanges)
        {
            return await FindByCondition(p => p.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }

        public IEnumerable<Domain.Entities.Plane> GetPlanes(bool trackChanges)
        {
            return FindAll(trackChanges).ToList().OrderBy(p => p.Name).ToList();
        }

        public async Task<IEnumerable<Domain.Entities.Plane>> GetPlanesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                  .OrderBy(p => p.Name)
                  .ToListAsync();
        }

        public int GetCount() => Count();

    }
}
