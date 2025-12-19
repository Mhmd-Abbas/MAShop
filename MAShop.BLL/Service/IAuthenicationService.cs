using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.BLL.Service
{
    public interface IAuthenicationService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<string> ConfirmEmailAsync(string token, string userId);
        Task<ForgotPasswordResposne> RequestPasswordReset(ForgotPasswordRequest request);
        Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request);
    }
}
