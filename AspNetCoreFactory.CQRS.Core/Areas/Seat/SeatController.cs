using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreFactory.CQRS.Core.Areas.Seat
{
    [Menu("Admin")]
    [AdminMenu("Seats")]
    [Route("admin/seats")]
    public class SeatController : Controller
    {
        private readonly IMediator _mediator;

        public SeatController(IMediator mediator) { _mediator = mediator; }

        #region Queries

        [HttpGet("{planeId?}")]
        public async Task<IActionResult> List([FromQuery]List.Query query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }

        [HttpGet("edit/{id?}")]
        public async Task<IActionResult> Edit(Edit.Query query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }

        #endregion

        #region Commands

        [HttpPost("edit/{id?}")]
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
