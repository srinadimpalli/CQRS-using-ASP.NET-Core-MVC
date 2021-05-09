using AspNetCoreFactory.CQRS.Core.Domain;
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

            private readonly CQRSContext _db;
            private readonly ICache _cache;

            public QueryHandler(CQRSContext db, ICache cache)
            {
                _db = db;
                _cache = cache;
            }

            protected override Command Handle(Query query)
            {
                var command = new Command();

                var booking = _db.Booking.SingleOrDefault(p => p.Id == query.Id);

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

            private readonly CQRSContext _db;
            private readonly IRollup _rollup;
            private readonly IEvent _event;

            public CommandHandler(CQRSContext db, IRollup rollup, IEvent @event)
            {
                _db = db;
                _rollup = rollup;
                _event = @event;
            }

            protected override void Handle(Command message)
            {
                var booking = _db.Booking.Find(message.Id);
                var original = new Domain.Booking { SeatId = booking.SeatId, FlightId = booking.FlightId };

                booking.FlightId = message.FlightId;
                booking.SeatId = message.SeatId;

                _db.Booking.Update(booking);
                _db.SaveChanges();

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
