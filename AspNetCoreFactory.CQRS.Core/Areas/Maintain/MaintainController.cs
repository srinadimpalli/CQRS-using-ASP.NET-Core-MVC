using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreFactory.CQRS.Core.Areas.Maintain
{
    [Menu("Admin")]
    [AdminMenu("Maintain")]
    [Route("admin/maintain")]
    public class MaintainController : Controller
    {
        private readonly IMediator _mediator;

        public MaintainController(IMediator mediator) { _mediator = mediator; }

        #region Queries

        [HttpGet]
        public async Task<IActionResult> Edit(Edit.Query query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }

        #endregion

        #region Commands

        [HttpPost]
        public async Task<IActionResult> Edit(Edit.Command command)
        {
            await _mediator.Send(command);
            return View(command);
        }

        #endregion
    }
}
