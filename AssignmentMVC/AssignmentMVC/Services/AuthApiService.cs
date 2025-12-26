using AssignmentMVC.ViewModels.Auth;
using System.Text.Json;
using System.Text;

namespace AssignmentMVC.Services
{
    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginViewModel model)
        {
            var loginData = new
            {
                email = model.Email,
                password = model.Password
            };

            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AuthResponseDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            var registerData = new
            {
                email = model.Email,
                password = model.Password,
                displayName = model.DisplayName,
                phoneNumber = model.PhoneNumber,
                roleName = model.RoleName
            };

            var json = JsonSerializer.Serialize(registerData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/auth/register", content);

            return response.IsSuccessStatusCode;
        }
    }
}
