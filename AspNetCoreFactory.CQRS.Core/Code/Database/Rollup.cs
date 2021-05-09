using AspNetCoreFactory.CQRS.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreFactory.CQRS.Core
{
    // Recalculate rollup values in database

    #region Interface

    public interface IRollup
    {
        void All();

        void TotalSeats();
        void TotalBookings();

        void TotalSeatsByPlane(int planeId);
        void TotalBookings(Booking booking);

        void TotalBookingsForFlight(int flightId);
        void TotalBookingsForSeat(int seatId);
    }

    #endregion

    public class Rollup : IRollup
    {
        #region Dependency Injection

        private readonly CQRSContext _db;

        public Rollup(CQRSContext db){_db = db;}

        #endregion

        #region Rollup Methods

        public void All()
        {
            TotalSeats();
            TotalBookings();
        }

        public void TotalSeats()
        {
            var sql = @"UPDATE Plane 
                           SET TotalSeats = (SELECT COUNT(Id)
                                               FROM Seat
                                              WHERE PlaneId = [Plane].Id);";

            _db.Database.ExecuteSqlRaw(sql);
        }

        public void TotalBookings()
        {
            var sql = @"UPDATE Seat 
                           SET TotalBookings = (SELECT COUNT(Id) FROM Booking WHERE SeatId = [Seat].Id);
                        UPDATE Flight
                           SET TotalBookings = (SELECT COUNT(Id) FROM Booking WHERE FlightId = [Flight].Id);
                        UPDATE Traveler 
                           SET TotalBookings = (SELECT COUNT(Id) FROM Booking WHERE TravelerId = [Traveler].Id);";
                        
            _db.Database.ExecuteSqlRaw(sql);
        }

        public void TotalSeatsByPlane(int planeId)
        {
            var sql = $@"UPDATE Plane 
                            SET TotalSeats = (SELECT COUNT(Id)
                                                FROM Seat
                                               WHERE PlaneId = {planeId});";

            _db.Database.ExecuteSqlRaw(sql);
        }

        public void TotalBookings(Booking booking)
        {
            var sql = $@"UPDATE Seat 
                            SET TotalBookings = (SELECT COUNT(Id) FROM Booking WHERE SeatId = [Seat].Id)
                          WHERE Id = {booking.SeatId};
                         UPDATE Flight
                            SET TotalBookings = (SELECT COUNT(Id) FROM Booking WHERE FlightId = [Flight].Id)
                          WHERE Id = {booking.FlightId};
                         UPDATE Traveler 
                            SET TotalBookings = (SELECT COUNT(Id) FROM Booking WHERE TravelerId = [Traveler].Id)
                          WHERE Id = {booking.TravelerId};";

            _db.Database.ExecuteSqlRaw(sql);
        }

        public void TotalBookingsForFlight(int flightId)
        {
            var sql = $@"UPDATE Flight
                            SET TotalBookings = (SELECT COUNT(Id) FROM Booking WHERE FlightId = [Flight].Id)
                          WHERE Id = {flightId};";

            _db.Database.ExecuteSqlRaw(sql);
        }

        public void TotalBookingsForSeat(int seatId)
        {
            var sql = $@"UPDATE Seat 
                           SET TotalBookings = (SELECT COUNT(Id) FROM Booking WHERE SeatId = [Seat].Id)
                         WHERE Id = {seatId};";

           _db.Database.ExecuteSqlRaw(sql);
        }

        #endregion
    }
}
