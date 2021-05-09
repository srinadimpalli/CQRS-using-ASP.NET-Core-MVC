using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreFactory.CQRS.Core.Areas.Admin
{
    [Menu("Admin")]
    [Route("admin")]
    [AdminMenu("Overview")]
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator) { _mediator = mediator; }

        #region Queries

        [HttpGet]
        public async Task<IActionResult> List(List.Query query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }

        #endregion
    }
}
