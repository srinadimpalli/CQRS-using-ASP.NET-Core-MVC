using AspNetCoreFactory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreFactory.Domain.Services
{
    public interface IFlightService
    {
        Task<IEnumerable<Flight>> GetFlightsAsync(bool trackChanges);
        IEnumerable<Flight> GetFlights(bool trackChanges);
        Task<Flight> GetFlightAsync(int id, bool trackChanges);
        Flight GetFlight(int id, bool trackChanges);
        void CreateFlight(Flight flight);
        void DeleteFlight(Flight flight);
        void UpdateFlight(Flight flight);
        IQueryable<Flight> AsQueryable();
        int GetCount();
    }
}
