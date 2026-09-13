using Microsoft.AspNetCore.Mvc;

namespace Cpa.Presentation.API.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private readonly ISender _mediator = null!;
        protected ISender Mediator => _mediator ?? HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
