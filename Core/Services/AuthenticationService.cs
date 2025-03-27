
namespace Services
{
    public class AuthenticationService(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IOptions<JwtOptions> options,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor
        )
        : IAuthenticationService
    {
        public async Task<UserResultDTO> LoginAsync(LoginDTO loginModel)
        {
            var user = await userManager.FindByEmailAsync(loginModel.Email);
            if (user == null) throw new UnAuthorizedException($"Email {loginModel.Email} doesn't Exist.");

            if (!user.EmailConfirmed)
                throw new UnAuthorizedException("Email not confirmed. Please check your email.");

            var result = await userManager.CheckPasswordAsync(user, loginModel.Password);
            if (!result) throw new UnAuthorizedException();

            // Generate refresh token and store it in the database
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(user);

            return new UserResultDTO(
              user.FirstName,
              user.LastName,
              user.Email!,
              await CreateAccessTokenAsync(user),
              user.RefreshToken!);

        }
        public async Task<bool> LogoutAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return false;

            // Invalidate Refresh Token
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            var result = await userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<UserResultDTO> RegisterAsync(RegisterDTO registerModel)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserType = "Client",
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                UserName = registerModel.Email,
                PhoneNumber = registerModel.PhoneNumber
            };

            var result = await userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description).ToList();
                throw new ValidationException(errors);
            }

            // Add the user to the appropriate role
            var roleAssignmentResult = await userManager.AddToRoleAsync(user, "ClientRole");
            if (!roleAssignmentResult.Succeeded)
            {
                var errors = roleAssignmentResult.Errors.Select(error => error.Description).ToList();
                throw new ValidationException(errors);
            }

            var individualRepo = unitOfWork.GetRepository<Client, string>();
            await individualRepo.AddAsync(new Client { Id = Guid.NewGuid().ToString(), ApplicationUserId = user.Id });
            await unitOfWork.SaveChangesAsync();

            // Generate OTP (6-digit code)
            var otp = new Random().Next(100000, 999999).ToString();
            user.EmailConfirmationOTP = otp;
            user.OTPExpiryTime = DateTime.UtcNow.AddMinutes(10); // OTP expires in 10 minutes
            await userManager.UpdateAsync(user);

            // Send OTP via email
            var emailBody = $@"
                            <h2>Email Verification</h2>
                            <p>Your OTP code for email verification is: <strong>{otp}</strong></p>
                            <p>This OTP will expire in 10 minutes.</p>";

            await emailService.SendEmailAsync(user.Email, "Verify Your Email", emailBody);

            return new UserResultDTO(
             user.FirstName,
             user.LastName,
             user.Email!,
             await CreateAccessTokenAsync(user),
             user.RefreshToken!);
        }
        public async Task<UserResultDTO> RefreshTokenAsync(string refreshToken)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnAuthorizedException("Invalid or expired refresh token.");

            // Generate new tokens
            var newAccessToken = await CreateAccessTokenAsync(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(user);

            return new UserResultDTO(
                user.FirstName,
                user.LastName,
                user.Email!,
                newAccessToken,
                newRefreshToken);
        }

        public async Task<bool> ConfirmEmailAsync(string email, string otp)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return false;

            if (user.EmailConfirmationOTP != otp || user.OTPExpiryTime <= DateTime.UtcNow)
                throw new ValidationException(new List<string> { "Invalid or expired OTP." });

            // Confirm email
            user.EmailConfirmed = true;
            user.EmailConfirmationOTP = null; // Clear OTP after verification
            user.OTPExpiryTime = null;
            await userManager.UpdateAsync(user);

            return true;
        }
        private async Task<string> CreateAccessTokenAsync(ApplicationUser user)
        {
            var jwtOptions = options.Value;

            // Create Claims 
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , user.FirstName! + user.LastName!),
                new Claim(ClaimTypes.Email , user.Email!),
                new Claim(ClaimTypes.UserData, user.UserType),
                new Claim(ClaimTypes.NameIdentifier , user.Id),
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: authClaims,
                signingCredentials: creds,
                audience: jwtOptions.Audience,
                issuer: jwtOptions.Issure,
                expires: DateTime.UtcNow.AddDays(jwtOptions.DurationInDays)
                );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null) return false;  // Email doesn't exist

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = $"https://safety.com/reset-password?email={dto.Email}&token={token}";
            // var resetUrl = $"{_config["AppSettings:FrontendUrl"]}/Reset-Password?email={email}&token={token}";


            var emailBody = $@"
            <h2>Password Reset Request</h2>
            <p>Click the link below to reset your password:</p>
            <a href='{resetUrl}'>Reset Password</a>
            <p>If you didn't request this, ignore this email.</p>";

            return await emailService.SendEmailAsync(dto.Email, "Reset Your Password", emailBody);
        }
        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null) return false;  // Email doesn't exist

            // Validate new password strength
            var passwordValidator = new PasswordValidator<ApplicationUser>();
            var result = await passwordValidator.ValidateAsync(userManager, user, dto.NewPassword);
            if (!result.Succeeded) return false;  // Password is not strong enough

            var resetResult = await userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            return resetResult.Succeeded;
        }
    }
}
