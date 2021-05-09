using AspNetCoreFactory.CQRS.Core.Domain;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AspNetCoreFactory.CQRS.Core.Areas.Seat
{
    // ** Command Query pattern

    public class Edit
    {
        // Query Input

        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
            public int? PlaneId { get; set; }
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
                if (message.PlaneId.HasValue) command.PlaneId = message.PlaneId.Value;

                var seat = _db.Seat.SingleOrDefault(p => p.Id == message.Id);
                if (seat != null)
                {
                    // ** Data Mapper Pattern

                    command.Id = seat.Id;
                    command.PlaneId = seat.PlaneId;
                    command.Number = seat.Number;
                    command.Location = seat.Location;
                    command.TotalBookings = seat.TotalBookings;
                }

                return command;
            }
        }

        // Request Output. Command Input (** DTO Pattern)

        public class Command : IRequest
        {
            public int Id { get; set; }
            [Required(ErrorMessage = "Plane is required")]
            public int PlaneId { get; set; }
            [Required(ErrorMessage = "Number is required")]
            public string Number { get; set; }
            [Required(ErrorMessage = "Location is required")]
            public string Location { get; set; }
            public int TotalBookings { get; set; }
        }

        // Command Process

        public class CommandHandler : RequestHandler<Command>
        {
            // ** DI Pattern

            private readonly CQRSContext _db;
            private readonly ICache _cache;
            private readonly IRollup _rollup;

            public CommandHandler(CQRSContext db, ICache cache, IRollup rollup)
            {
                _db = db;
                _cache = cache;
                _rollup = rollup;
            }

            protected override void Handle(Command message)
            {
                if (message.Id == 0)
                    InsertSeat(message);
                else
                    UpdateSeat(message);
                
            }

            private void InsertSeat(Command message)
            {
                // ** Data Mapper pattern

                var seat = new Domain.Seat();
                seat.PlaneId = message.PlaneId;
                seat.Number = message.Number;
                seat.Location = message.Location;
                seat.TotalBookings = message.TotalBookings;

                _db.Seat.Add(seat);
                _db.SaveChanges();

                _cache.AddSeat(seat);
                _rollup.TotalSeatsByPlane(seat.PlaneId);
            }

            private void UpdateSeat(Command message)
            {
                // ** Data Mapper Pattern

                var seat = _db.Seat.Find(message.Id);
                seat.PlaneId = message.PlaneId;
                seat.Number = message.Number;
                seat.Location = message.Location;
                seat.TotalBookings = message.TotalBookings;

                _db.Seat.Update(seat);
                _db.SaveChanges();

                _cache.UpdateSeat(seat);
            }
        }
    }
}

