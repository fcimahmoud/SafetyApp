
global using Services.Abstractions;
global using Shared.AuthenticationModels;

namespace Presentation
{
    public class AuthenticationController (IServiceManager serviceManager)
        : ApiController
    {
        [HttpGet("Login")]
        public async Task<ActionResult<UserResultDTO>> Login(LoginDTO login)
            => Ok(await serviceManager.AuthenticationService.LoginAsync(login));

        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDTO>> Register(RegisterDTO register)
            => Ok(await serviceManager.AuthenticationService.RegisterAsync(register));

        [HttpPost("Confirm-Email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var result = await serviceManager.AuthenticationService.ConfirmEmailAsync(email, token);
            if (!result) return BadRequest("Email confirmation failed. Invalid or expired token.");

            return Ok("Email confirmed successfully. You can now log in.");
        }

        [HttpGet("Refresh-Token")]
        public async Task<ActionResult<UserResultDTO>> RefreshToken([FromBody] RefreshTokenRequestDTO request)
            => Ok(await serviceManager.AuthenticationService.RefreshTokenAsync(request.RefreshToken));

        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
        {
            var result = await serviceManager.AuthenticationService.ForgotPasswordAsync(dto);
            if (!result) return BadRequest("Email not found or failed to send email.");

            return Ok("Password reset link sent successfully.");
        }

        [HttpPut("Reset-Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
        {
            var result = await serviceManager.AuthenticationService.ResetPasswordAsync(dto);
            if (!result) return BadRequest("Invalid token or password reset failed.");

            return Ok("Password reset successfully.");
        }
    }
}
