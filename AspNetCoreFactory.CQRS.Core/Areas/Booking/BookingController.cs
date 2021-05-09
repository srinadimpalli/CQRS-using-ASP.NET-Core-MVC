using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreFactory.CQRS.Core.Areas.Booking
{
    [Menu("Bookings")]
    [Route("bookings")]
    public class BookingController : Controller
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator) { _mediator = mediator; }

        #region Queries

        [HttpGet]
        public async Task<IActionResult> List([FromQuery]List.Query query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Create(Create.Query query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute]Edit.Query query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }

        [HttpGet("seats"), AjaxOnly]
        public async Task<JsonResult> Seats([FromQuery]Seats.Query query)
        {
            var model = await _mediator.Send(query);
            return Json(model.AvailableSeats);
        }

        #endregion

        #region Commands

        [HttpPost("edit")]
        public async Task<IActionResult> Create([FromForm]Create.Command command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction("List");
            }

            return View(command);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit([FromForm]Edit.Command command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction("List");
            }

            return View(command);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromForm]Delete.Command command)
        {
            await _mediator.Send(command);
            return RedirectToAction("List");
        }

        #endregion
    }
}
