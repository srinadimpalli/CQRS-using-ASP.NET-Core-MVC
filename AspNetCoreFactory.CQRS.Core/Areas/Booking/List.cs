using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreFactory.CQRS.Core.Areas.Booking
{
    // ** Command Query pattern

    public class List
    {
        // Input

        public class Query : IRequest<Result>
        {
            public int? FlightId { get; set; }
            public int? TravelerId { get; set; }
        }

        // Output

        public class Result
        {
            public int? FlightId { get; set; }
            public int? TravelerId { get; set; }

            public List<Booking> Bookings { get; set; } = new List<Booking>();

            public class Booking
            {
                public int Id { get; set; }
                public string BookingDate { get; set; }
                public string BookingNumber { get; set; }

                public int FlightId { get; set; }
                public string Flight { get; set; }
                public string From { get; set; }
                public string To { get; set; }
                public string Date { get; set; }

                public int SeatId { get; set; }
                public string Seat { get; set; }

                public int TravelerId { get; set; }
                public string Traveler { get; set; }
            }
        }

        // Handler

        public class QueryHandler : RequestHandler<Query, Result>
        {
            // ** DI Pattern
            private readonly IServiceManager _serviceManager;
            private readonly ICache _cache;
            public QueryHandler(ICache cache, IServiceManager serviceManager)
            {
                _serviceManager = serviceManager;
                _cache = cache;
            }

            protected override Result Handle(Query query)
            {
                var result = new Result { TravelerId = query.TravelerId, FlightId = query.FlightId };

                var bookings = _serviceManager.Booking.AsQueryable();

                if (query.TravelerId != null)
                    bookings = bookings.Where(b => b.TravelerId == query.TravelerId);

                if (query.FlightId != null)
                    bookings = bookings.Where(b => b.FlightId == query.FlightId);

                foreach (var booking in bookings)
                {
                    var flight = _cache.Flights[booking.FlightId];
                    var traveler = _cache.Travelers[booking.TravelerId];
                    var seat = _cache.Seats[booking.SeatId];

                    // ** Data Mapping pattern

                    result.Bookings.Add(new Result.Booking
                    {
                        Id = booking.Id,
                        BookingDate = booking.BookingDate.ToString("MM/dd/yyyy"),
                        BookingNumber = booking.BookingNumber,
                        TravelerId = booking.TravelerId,
                        Traveler = traveler.Name,
                        FlightId = booking.FlightId,
                        Flight = flight.FlightNumber,
                        From = flight.From,
                        To = flight.To,
                        Date = flight.Departure.ToString("dd MMM yyyy"),
                        Seat = seat.Number
                    });
                }

                return result;
            }
        }
    }
}
