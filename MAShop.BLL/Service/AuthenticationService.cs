using Mapster;
using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using MAShop.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.BLL.Service
{
    public class AuthenticationService : IAuthenicationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthenticationService
            (
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }


        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (user is null)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Invalid Email",
                    };
                }

                if(await _userManager.IsLockedOutAsync(user))
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "User is locked out, try again later",
                    };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, true);

                if (result.IsLockedOut)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Account Locked duo to multiple failed attempts",
                    };
                }

                if (result.IsNotAllowed)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "PLease confirm Email",
                    };
                }

                if (!result.Succeeded)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Invalid Password",
                    };
                }

                var accessToken = await _tokenService.GenerateAccessToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                await _userManager.UpdateAsync(user);

                return new LoginResponse()
                {
                    Success = true,
                    Message = "Login Successfully",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "An unexpected error occured",
                    Errors = new List<string> { ex.Message }
                };
            }
          
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            try
            {
                var user = registerRequest.Adapt<ApplicationUser>();
                var result = await _userManager.CreateAsync(user, registerRequest.Password);


                if (!result.Succeeded)
                {
                    return new RegisterResponse()
                    {
                        Success = false,
                        Message = "User Creation Failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                await _userManager.AddToRoleAsync(user, "User");
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = Uri.EscapeDataString(token);

                var emailUrl = $"http://localhost:5257/api/auth/Account/ConfirmEmail?token={token}&userId={user.Id}";

                await _emailSender.SendEmailAsync(user.Email, "Welcome to MAShop", $"<h1>Thank you {user.UserName} for registering at MAShop!</h1>" +
                    $"<a href='{emailUrl}' >Confirm Email</a>");


                return new RegisterResponse()
                {
                    Success = true,
                    Message = "Success"
                };

            }
            catch (Exception ex)
            {
                return new RegisterResponse()
                {
                    Success = false,
                    Message = "An unexpected error occured",
                    Errors = new List<string> { ex.Message }
                };
            }

        }

        public async Task<string> ConfirmEmailAsync(string token, string userId)
        {
            var  user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "user not found";
            }
            var result = _userManager.ConfirmEmailAsync(user, token);
            return "User Confirmed";
        }


        public async Task<ForgotPasswordResposne> RequestPasswordReset(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) 
            {
                return new ForgotPasswordResposne
                {
                    Success = false,
                    Message = "Email not found"
                };
            }

            var random = new Random();
            var code = random.Next(1000, 9990).ToString();
            
            user.CodeResetPassword = code;
            user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(5);

            await _userManager.UpdateAsync(user);

            await _emailSender.SendEmailAsync(user.Email, "Password Reset Request", $"<h1>Password Reset Code</h1>" +
                $"<p>Your password reset code is: <strong>{code}</strong></p>" +
                $"<p>This code will expire in 5 minutes.</p>");

            return new ForgotPasswordResposne
            {
                Success = true,
                Message = "Password reset code sent to email"
            };
        }


        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Email not found"
                };
            }

            else if (user.CodeResetPassword != request.Code )
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Invalid or expired code"
                };
            }

            else if (user.PasswordResetCodeExpiry < DateTime.UtcNow)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Code has expired"
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (!result.Succeeded)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Password reset failed",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            await _emailSender.SendEmailAsync(user.Email, "Password Changed!", $"<p> your password has changed successfully </p>");

            user.CodeResetPassword = null;
            await _userManager.UpdateAsync(user);

            return new ResetPasswordResponse
            {
                Success = true,
                Message = "password reset successful"
            };
        }

        public async Task<LoginResponse> RefreshToken(TokenApiModel req)
        {
            string accessToken = req.AccessToken;
            string refreshToken = req.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userName = principal.Identity.Name;

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid Client Request"
                };
            }

            var newAccessToken = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);
            return new LoginResponse
            {
                Success = true,
                Message = "Token Refreehed",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

        }

    }
}
