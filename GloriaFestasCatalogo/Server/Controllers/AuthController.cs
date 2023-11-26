using GloriaFestasCatalogo.Shared.Models.Users;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GloriaFestasCatalogo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLogin request)
        {
            var response = await _authService.Login(request.Username, request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);

        }

    }

}

