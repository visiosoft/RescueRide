using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RescueRide.Application.DTOs.Authentication;
using LoginRequest = RescueRide.Application.DTOs.Authentication.LoginRequest;

namespace RescueRide.API.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly Application.Services.IAuthenticationService _authenticationService;

        public AuthController(Application.Services.IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authenticationService.Authenticate(request.Username, request.Password);
            if (response != null)
            {
                return Unauthorized(new { message = response.ErrorMessage });
            }
            return Ok(response);
        }

        [HttpPost("generate-otp")]
        public IActionResult GenerateOtp([FromBody] string username)
        {
            _authenticationService.GenerateOtp(username);
            return Ok("OTP sent successfully");
        }

        [HttpPost("validate-otp")]
        public IActionResult ValidateOtp([FromBody] OtpRequest request)
        {
            if (_authenticationService.ValidateOtp(request.Username, request.Otp))
                return Ok("OTP validated successfully");

            return Unauthorized("Invalid OTP");
        }
    }
}
