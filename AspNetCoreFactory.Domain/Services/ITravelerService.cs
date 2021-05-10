using AspNetCoreFactory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreFactory.Domain.Services
{
    public interface ITravelerService
    {
        Task<IEnumerable<Traveler>> GetTravelersAsync(bool trackChanges);
        IEnumerable<Traveler> GetTravelers(bool trackChanges);
        Traveler GetTraveler(int id, bool trackChanges);
        Task<Traveler> GetTravelerAsync(int id, bool trackChanges);
        void CreateTraveler(Traveler traveler);
        void DeleteTraveler(Traveler traveler);
        void UpdateTraveler(Traveler traveler);
        int GetCount();
    }
}
