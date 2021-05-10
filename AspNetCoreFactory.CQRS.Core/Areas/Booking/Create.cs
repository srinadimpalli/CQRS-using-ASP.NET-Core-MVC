using AspNetCoreFactory.Domain.Entities;
using AspNetCoreFactory.Domain.Services;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AspNetCoreFactory.CQRS.Core.Areas.Booking
{
    // ** Command Query pattern

    public class Create
    {
        // Query Input

        public class Query : IRequest<Command>
        {
            public int FlightId { get; set; }
            public int TravelerId { get; set; }
        }

        // Query Process

        public class QueryHandler : RequestHandler<Query, Command>
        {

            public QueryHandler()
            {

            }

            protected override Command Handle(Query request)
            {
                return new Command();
            }
        }

        // Query Output, Command Input (** DTO Pattern)

        public class Command : IRequest
        {
            [Required(ErrorMessage = "Flight is required")]
            public int FlightId { get; set; }
            [Required(ErrorMessage = "Seat is required")]
            public int SeatId { get; set; }
            [Required(ErrorMessage = "Traveler is required")]
            public int TravelerId { get; set; }
        }

        // Command Process

        public class CommandHandler : RequestHandler<Command>
        {
            // ** DI Pattern
            private readonly IServiceManager _serviceManager;
            private readonly IRollup _rollup;
            private readonly IEvent _event;

            public CommandHandler(IServiceManager serviceManager, IRollup rollup, IEvent @event)
            {
                _serviceManager = serviceManager;
                _rollup = rollup;
                _event = @event;
            }

            protected override void Handle(Command message)
            {
                // ** Data Mapper Pattern

                var booking = new Domain.Entities.Booking();
                booking.FlightId = message.FlightId;
                booking.SeatId = message.SeatId;
                booking.TravelerId = message.TravelerId;
                booking.BookingNumber = BookingNumber.Next();
                booking.BookingDate = DateTime.UtcNow;

                _serviceManager.Booking.CreateBooking(booking);
                _serviceManager.Save();

                // ** Event Sourcing Pattern

                _event.InsertBooking(booking);

                // Update statistics

                _rollup.TotalBookings(booking);
            }
        }

        // Booking number generator

        private static class BookingNumber
        {
            private static Random _random = new Random();
            private static object _locker = new object();
            private static string _c = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";

            public static string Next()
            {
                int length = 6;
                lock (_locker)
                {
                    char[] chars = new char[length];

                    for (int i = 0; i < length; i++)
                    {
                        chars[i] = _c[(int)((_c.Length) * _random.NextDouble())];
                    }

                    return new string(chars);
                }
            }
        }
    }
}
