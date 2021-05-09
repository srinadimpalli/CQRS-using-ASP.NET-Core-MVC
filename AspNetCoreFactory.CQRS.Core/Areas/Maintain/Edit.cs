using MediatR;

namespace AspNetCoreFactory.CQRS.Core.Areas.Maintain
{
    // ** Command Query pattern

    public class Edit
    {
        // Query Input

        public class Query : IRequest<Command> { }

        // Query Process

        public class QueryHandler : RequestHandler<Query, Command>
        {
            protected override Command Handle(Query message)
            {
                return new Command();
            }
        }

        // Query Output. Command Input (** DTO Pattern)

        public class Command : IRequest
        {
            public string Message { get; set; }
        }

        // Command Process

        public class CommandHandler : RequestHandler<Command>
        {
            private readonly IRollup _rollup;

            public CommandHandler(IRollup rollup)
            {
                _rollup = rollup;
            }

            protected override void Handle(Command message)
            {
                _rollup.All();

                message.Message = "Completed";
            }
        }
    }
}
