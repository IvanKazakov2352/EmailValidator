using EmailValidator.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmailValidator.Controllers
{
    [ApiController]
    [Route("api")]
    public class CheckEmailController(ICheckEmailService service) : ControllerBase
    {
        [HttpGet("checkEmail")]
        public async ValueTask<bool> CheckEmail(string email, CancellationToken ct)
        {
            return await service.CheckEmail(email, ct);
        }
    }
}
