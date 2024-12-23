using EmailValidator.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmailValidator.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class CheckEmailController(IEmailValidatorService service) : ControllerBase
    {
        [HttpGet("validate")]
        public async ValueTask<ValidationResult> CheckEmail(string email, CancellationToken ct)
        {
            return await service.CheckEmail(email, ct);
        }
    }
}
