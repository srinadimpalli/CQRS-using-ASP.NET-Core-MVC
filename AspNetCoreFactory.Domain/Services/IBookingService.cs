using AspNetCoreFactory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreFactory.Domain.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetBookingsAsync(bool trackChanges);
        IEnumerable<Booking> GetBookings(bool trackChanges);
        Task<Booking> GetBookingAsync(int id, bool trackChanges);
        Booking GetBooking(int id, bool trackChanges);
        void CreateBooking(Booking booking);
        void DeleteBooking(Booking booking);
        void UpdateBooking(Booking booking);
        IQueryable<Domain.Entities.Booking> AsQueryable();
        int GetCount();
    }
}
