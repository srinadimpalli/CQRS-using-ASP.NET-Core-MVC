using AspNetCoreFactory.CQRS.Core.Domain;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AspNetCoreFactory.CQRS.Core.Areas.Flight
{
    // ** Command Query pattern

    public class Edit
    {
        // Query Input (** DTO Pattern)

        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        // Query Process

        public class QueryHandler : RequestHandler<Query, Command>
        {
            // ** DI Pattern

            private readonly CQRSContext _db;

            public QueryHandler(CQRSContext db)
            {
                _db = db;
            }

            protected override Command Handle(Query message)
            {
                var command = new Command();

                var flight = _db.Flight.SingleOrDefault(p => p.Id == message.Id);
                if (flight != null)
                {
                    // ** Data Mapper Pattern

                    command.Id = flight.Id;
                    command.PlaneId = flight.PlaneId;
                    command.FlightNumber = flight.FlightNumber;
                    command.From = flight.From;
                    command.To = flight.To;
                    command.DepartureDate = flight.Departure.ToString("MM/dd/yyyy");
                    command.ArrivalDate = flight.Arrival.ToString("MM/dd/yyyy");
                    command.DepartureTime = flight.Departure.ToString("HH:mm");
                    command.ArrivalTime = flight.Arrival.ToString("HH:mm");
                    command.TotalBookings = flight.TotalBookings;
                }

                return command;
            }
        }

        // Query Output. Command Input (** DTO Pattern)

        public class Command : IRequest
        {
            public int Id { get; set; }
            [Required(ErrorMessage = "Plane is required")]
            public int PlaneId { get; set; }
            [Required(ErrorMessage = "Flight Number is required")]
            public string FlightNumber { get; set; }
            [Required(ErrorMessage = "From is required")]
            public string From { get; set; }
            [Required(ErrorMessage = "To is required")]
            public string To { get; set; }
            [Required(ErrorMessage = "Departure date is required")]
            public string DepartureDate { get; set; }
            [Required(ErrorMessage = "Departure time is required")]
            public string DepartureTime { get; set; }
            [Required(ErrorMessage = "Arrival date is required")]
            public string ArrivalDate { get; set; }
            [Required(ErrorMessage = "Arrival time is required")]
            public string ArrivalTime { get; set; }
            public int TotalBookings { get; set; }
        }

        // Command Process

        public class CommandHandler : RequestHandler<Command>
        {
            // ** DI Pattern

            private readonly CQRSContext _db;
            private readonly ICache _cache;

            public CommandHandler(CQRSContext db, ICache cache)
            {
                _db = db;
                _cache = cache;
            }

            protected override void Handle(Command message)
            {
                if (message.Id == 0)
                    InsertFlight(message);
                else
                    UpdateFlight(message);
            }

            private void InsertFlight(Command message)
            {
                // ** Data Mapper Pattern

                var flight = new Domain.Flight
                {
                    PlaneId = message.PlaneId,
                    FlightNumber = message.FlightNumber,
                    From = message.From,
                    To = message.To,
                    Departure = DateTime.Parse(message.DepartureDate),
                    Arrival = DateTime.Parse(message.ArrivalDate)
                };

                // Add time

                try
                {
                    string[] tokens = message.DepartureTime.Split(":");
                    flight.Departure = flight.Departure.AddHours(int.Parse(tokens[0]));
                    flight.Departure = flight.Departure.AddMinutes(int.Parse(tokens[1]));
                }
                catch { /* noop */ }

                try
                {
                    string[] tokens = message.ArrivalTime.Split(":");
                    flight.Arrival = flight.Arrival.AddHours(int.Parse(tokens[0]));
                    flight.Arrival = flight.Arrival.AddMinutes(int.Parse(tokens[1]));
                }
                catch { /* noop */ }

                _db.Flight.Add(flight);
                _db.SaveChanges();

                _cache.AddFlight(flight);
            }

            private void UpdateFlight(Command message)
            {
                // ** Data Mapper Pattern

                var flight = _db.Flight.Find(message.Id);

                flight.PlaneId = message.PlaneId;
                flight.FlightNumber = message.FlightNumber;
                flight.From = message.From;
                flight.To = message.To;
                flight.Departure = DateTime.Parse(message.DepartureDate);
                flight.Arrival = DateTime.Parse(message.ArrivalDate);

                // Add time

                try
                {
                    string[] tokens = message.DepartureTime.Split(":");
                    flight.Departure = flight.Departure.AddHours(int.Parse(tokens[0]));
                    flight.Departure = flight.Departure.AddMinutes(int.Parse(tokens[1]));
                }
                catch { /* noop */ }

                try
                {
                    string[] tokens = message.ArrivalTime.Split(":");
                    flight.Arrival = flight.Arrival.AddHours(int.Parse(tokens[0]));
                    flight.Arrival = flight.Arrival.AddMinutes(int.Parse(tokens[1]));
                }
                catch { /* noop */ }


                _db.Flight.Update(flight);
               _db.SaveChanges();

                _cache.UpdateFlight(flight);
            }
        }
    }
}
