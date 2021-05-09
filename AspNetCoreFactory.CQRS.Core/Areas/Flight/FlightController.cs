using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreFactory.CQRS.Core.Areas.Flight
{
    [Menu("Flights")]
    [Route("flights")]
    public class FlightController : Controller
    {
        private readonly IMediator _mediator;

        public FlightController(IMediator mediator) { _mediator = mediator; }

        #region Queries

        [HttpGet]
        public async Task<IActionResult> List(List.Query query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail([FromRoute]Detail.Query query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }

        [HttpGet("edit/{id?}")]
        public async Task<IActionResult> Edit([FromRoute]Edit.Query query)
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
