using AspNetCoreFactory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreFactory.Domain.Services
{
    public interface ISeatService
    {
        Task<IEnumerable<Seat>> GetSeatssAsync(bool trackChanges);
        IEnumerable<Seat> GetSeats(bool trackChanges);
        Task<Seat> GetSeatAsync(int id, bool trackChanges);
        Seat GetSeat(int id, bool trackChanges);
        IQueryable<Seat> AsQueryable();
        void CreateSeat(Seat seat);
        void DeleteSeat(Seat seat);
        void UpdateSeat(Seat seat);
        int GetCount();
    }
}
