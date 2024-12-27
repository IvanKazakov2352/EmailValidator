using EmailValidator.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmailValidator.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class CheckEmailController(IEmailValidatorService service) : ControllerBase
    {
        [HttpGet("validate")]
        public async ValueTask<ValidationResult> ValidateEmail(string email, CancellationToken ct)
        {
            return await service.ValidateEmail(email, ct);
        }
    }
}
