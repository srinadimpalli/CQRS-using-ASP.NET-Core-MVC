using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AspNetCoreFactory.CQRS.Core.Areas.Booking
{
    // ** Command Query pattern

    public class Edit
    {
        // Query Input

        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        // Query Process

        public class QueryHandler : RequestHandler<Query, Command>
        {
            // ** DI Pattern
            private readonly IServiceManager _serviceManager;
            private readonly ICache _cache;

            public QueryHandler(ICache cache, IServiceManager serviceManager)
            {
                _serviceManager = serviceManager;
                _cache = cache;
            }

            protected override Command Handle(Query query)
            {
                var command = new Command();

                var booking = _serviceManager.Booking.GetBooking(query.Id, trackChanges: false);//  _db.Booking.SingleOrDefault(p => p.Id == query.Id);

                // ** Data Mapping Pattern

                command.Id = booking.Id;
                command.BookingDate = booking.BookingDate.ToString("dd MMM, yyyy");
                command.BookingNumber = booking.BookingNumber;
                command.Traveler = _cache.Travelers[booking.TravelerId].Name;
                command.FlightId = booking.FlightId;
                command.SeatId = booking.SeatId;

                return command;
            }
        }

        // Query Output, Command Input (** DTO Pattern)

        public class Command : IRequest
        {
            public int Id { get; set; }
            public string BookingDate { get; set; }
            public string BookingNumber { get; set; }
            public string Traveler { get; set; }

            [Required(ErrorMessage = "Flight is required")]
            public int FlightId { get; set; }
            [Required(ErrorMessage = "Seat is required")]
            public int SeatId { get; set; }
        }

        // Command Process

        public class CommandHandler : RequestHandler<Command>
        {
            // ** DI Pattern
            private readonly IServiceManager _serviceManager;
            private readonly IRollup _rollup;
            private readonly IEvent _event;

            public CommandHandler(IRollup rollup, IEvent @event, IServiceManager serviceManager)
            {
                _serviceManager = serviceManager;
                _rollup = rollup;
                _event = @event;
            }

            protected override void Handle(Command message)
            {
                var booking = _serviceManager.Booking.GetBooking(message.Id, trackChanges: true); //   
                var original = new Domain.Entities.Booking { SeatId = booking.SeatId, FlightId = booking.FlightId };

                booking.FlightId = message.FlightId;
                booking.SeatId = message.SeatId;
                _serviceManager.Booking.UpdateBooking(booking);
                _serviceManager.Save();

                // ** Event Sourcing Pattern
                _event.UpdateBooking(original, booking);
                // Update statistics

                _rollup.TotalBookings(booking);
                if (original.FlightId != booking.FlightId) _rollup.TotalBookingsForFlight(original.FlightId);
                if (original.SeatId != booking.SeatId) _rollup.TotalBookingsForSeat(original.SeatId);
            }
        }
    }
}
