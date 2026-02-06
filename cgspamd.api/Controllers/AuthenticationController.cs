using cgspamd.api.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using cgspamd.api.Models;
using cgspamd.core.Models.APIModels;

namespace cgspamd.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private UserAuthenticationApplication application;
        public AuthenticationController(UserAuthenticationApplication application)
        {
            this.application = application;
        }
        [HttpPost]
        public async Task<IResult> Post([FromBody] UserLoginRequest request)
        {
            string? res = await application.Authenticate(request);
            if (res == null)
            {
                return Results.Unauthorized();
            }
            return Results.Ok(new { token = res });
        }
    }
}
