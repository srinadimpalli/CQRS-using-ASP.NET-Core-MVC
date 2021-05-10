using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreFactory.CQRS.Core.Areas.Seat
{
    public class SeatService : ServiceBase<Domain.Entities.Seat>, ISeatService
    {
        private readonly CQRSContext _cQRSContext;
        public SeatService(CQRSContext cQRSContext) : base(cQRSContext)
        {
            _cQRSContext = cQRSContext;
        }
        public void CreateSeat(Domain.Entities.Seat seat)
        {
            Create(seat);
        }
        public void UpdateSeat(Domain.Entities.Seat seat)
        {
            Update(seat);
        }
        public void DeleteSeat(Domain.Entities.Seat seat)
        {
            Delete(seat);
        }
        public IQueryable<Domain.Entities.Seat> AsQueryable()
        {
            return _cQRSContext.Seat.AsQueryable();
        }
        public Domain.Entities.Seat GetSeat(int id, bool trackChanges)
        {
            return FindByCondition(s => s.Id.Equals(id), trackChanges: false)
                    .SingleOrDefault();
        }

        public async Task<Domain.Entities.Seat> GetSeatAsync(int id, bool trackChanges)
        {
            return await FindByCondition(s => s.Id.Equals(id), trackChanges: false)
                    .SingleOrDefaultAsync();
        }

        public IEnumerable<Domain.Entities.Seat> GetSeats(bool trackChanges)
        {
            return FindAll(trackChanges).ToList().OrderBy(s => s.Number).ToList();
        }

        public async Task<IEnumerable<Domain.Entities.Seat>> GetSeatssAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                 .OrderBy(s => s.Number)
                 .ToListAsync();
        }
        public int GetCount() => Count();

    }
}
