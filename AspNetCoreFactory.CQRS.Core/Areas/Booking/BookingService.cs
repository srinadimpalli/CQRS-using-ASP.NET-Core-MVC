using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreFactory.CQRS.Core.Areas.Booking
{
    public class BookingService : ServiceBase<Domain.Entities.Booking>, IBookingService
    {
        private readonly CQRSContext _cQRSContext;
        public BookingService(CQRSContext cQRSContext) : base(cQRSContext)
        {
            _cQRSContext = cQRSContext;
        }

        public void CreateBooking(Domain.Entities.Booking booking)
        {
            Create(booking);
        }

        public void DeleteBooking(Domain.Entities.Booking booking)
        {
            Delete(booking);
        }
        public void UpdateBooking(Domain.Entities.Booking booking)
        {
            Update(booking);
        }
        public IQueryable<Domain.Entities.Booking> AsQueryable()
        {
            return _cQRSContext.Booking.AsQueryable();
        }
        public Domain.Entities.Booking GetBooking(int id, bool trackChanges)
        {
            return FindByCondition(b => b.Id.Equals(id), trackChanges: false)
                   .OrderBy(b => b.BookingNumber)
                   .SingleOrDefault();
        }

        public async Task<Domain.Entities.Booking> GetBookingAsync(int id, bool trackChanges)
        {
            return await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }

        public IEnumerable<Domain.Entities.Booking> GetBookings(bool trackChanges)
        {
            return FindAll(trackChanges).ToList().OrderBy(b => b.BookingNumber).ToList();
        }

        public async Task<IEnumerable<Domain.Entities.Booking>> GetBookingsAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                  .OrderBy(b => b.BookingNumber)
                  .ToListAsync();
        }
        public int GetCount() => Count();
    }
}
