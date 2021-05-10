using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreFactory.CQRS.Core.Areas.Booking
{
    // ** Command Query pattern

    public class Seats
    {
        public class Query : IRequest<Result>
        {
            public int Id { get; set; }
            public int SeatId { get; set; }  // used in editing bookings. Existing seat must be included.
        }

        public class Result : IRequest
        {
            public List<SelectListItem> AvailableSeats { get; set; } = new List<SelectListItem>();
        }

        public class Handler : RequestHandler<Query, Result>
        {
            private readonly IServiceManager _serviceManager;
            private readonly ICache _cache;

            public Handler(IServiceManager serviceManager, ICache cache)
            {
                _serviceManager = serviceManager;
                _cache = cache;
            }

            protected override Result Handle(Query query)
            {
                var result = new Result();
                if (query.Id > 0)
                {
                    var flight = _cache.Flights[query.Id];
                    var plane = _cache.Planes[flight.PlaneId];
                }
                var seats = _cache.Seats.Values;
                var bookingsQuery = _serviceManager.Booking.AsQueryable();
                var bookings = bookingsQuery.Where(b => b.FlightId == query.Id).Select(b => b.SeatId).ToList();

                foreach (var seat in seats)
                {
                    if (!bookings.Contains(seat.Id) || seat.Id == query.SeatId)
                        result.AvailableSeats.Add(
                            new SelectListItem { Value = seat.Id.ToString(), Text = seat.Number });
                }

                return result;
            }
        }
    }
}
