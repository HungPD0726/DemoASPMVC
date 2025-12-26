using AssignmentMVC.ViewModels.Auth;

namespace AssignmentMVC.Services
{
    public interface IAuthApiService
    {
        Task<AuthResponseDto> LoginAsync(LoginViewModel model);
        Task<bool> RegisterAsync(RegisterViewModel model);
    }

    public class AuthResponseDto
    {
        public string Token { get; set; }
    }
}
