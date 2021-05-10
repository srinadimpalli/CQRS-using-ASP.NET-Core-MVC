using AspNetCoreFactory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreFactory.Domain.Services
{
    public interface IPlaneService
    {
        Task<IEnumerable<Plane>> GetPlanesAsync(bool trackChanges);
        IEnumerable<Plane> GetPlanes(bool trackChanges);
        Task<Plane> GetPlaneAsync(int id, bool trackChanges);
        Plane GetPlane(int id, bool trackChanges);
        void CreatePlane(Plane place);
        void DeletePlane(Plane plane);
        void UpdatePlane(Plane plane);
        int GetCount();

    }
}
