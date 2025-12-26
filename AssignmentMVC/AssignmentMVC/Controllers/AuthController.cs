using AssignmentMVC.Services;
using AssignmentMVC.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

namespace AssignmentMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthApiService _authApiService;

        public AuthController(IAuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        // GET: Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var response = await _authApiService.LoginAsync(model);

                if (response == null)
                {
                    ModelState.AddModelError(string.Empty, "Sai email hoặc mật khẩu!");
                    return View(model);
                }

                // Lưu JWT token vào cookie
                Response.Cookies.Append("AuthToken", response.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Chỉ gửi qua HTTPS
                    SameSite = SameSiteMode.Lax, // Lax cho phép cross-site navigation
                    Expires = DateTimeOffset.UtcNow.AddHours(24) // Token hết hạn sau 24 giờ
                });

                // Redirect về trang House Index sau khi đăng nhập thành công
                TempData["SuccessMessage"] = "Đăng nhập thành công!";
                return RedirectToAction("Index", "House");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi kết nối: {ex.Message}");
                return View(model);
            }
        }

        // GET: Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _authApiService.RegisterAsync(model);

                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Email đã tồn tại hoặc lỗi hệ thống!");
                    return View(model);
                }

                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi kết nối: {ex.Message}");
                return View(model);
            }
        }

        // GET: Auth/GoogleLogin
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(properties, "Google");
        }

        // GET: Auth/GoogleResponse
        public async Task<IActionResult> GoogleResponse()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync("Cookies");

                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = "Đăng nhập Google thất bại!";
                    return RedirectToAction("Login");
                }

                var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
                var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var googleId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    TempData["ErrorMessage"] = "Không thể lấy thông tin email từ Google!";
                    return RedirectToAction("Login");
                }

                // Tạo LoginViewModel với thông tin từ Google
                // Password sẽ là Google ID (cho user login bằng Google)
                var loginModel = new LoginViewModel
                {
                    Email = email,
                    Password = googleId ?? "google-oauth-" + email
                };

                // Thử đăng nhập với API
                var response = await _authApiService.LoginAsync(loginModel);

                if (response == null)
                {
                    // Nếu user chưa tồn tại, tự động đăng ký
                    var registerModel = new RegisterViewModel
                    {
                        Email = email,
                        DisplayName = name ?? email.Split('@')[0],
                        Password = loginModel.Password,
                        ConfirmPassword = loginModel.Password,
                        RoleName = "Tenant" // Mặc định là người thuê
                    };

                    var registerResult = await _authApiService.RegisterAsync(registerModel);

                    if (!registerResult)
                    {
                        TempData["ErrorMessage"] = "Không thể tạo tài khoản từ Google account!";
                        return RedirectToAction("Login");
                    }

                    // Đăng nhập lại sau khi đăng ký
                    response = await _authApiService.LoginAsync(loginModel);

                    if (response == null)
                    {
                        TempData["ErrorMessage"] = "Đăng ký thành công nhưng đăng nhập thất bại!";
                        return RedirectToAction("Login");
                    }
                }

                // Lưu JWT token vào cookie
                Response.Cookies.Append("AuthToken", response.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax, // Lax cho phép cookie được gửi từ external redirect (Google OAuth)
                    Expires = DateTimeOffset.UtcNow.AddHours(24)
                });

                TempData["SuccessMessage"] = $"Đăng nhập Google thành công! Xin chào {name ?? email}";
                return RedirectToAction("Index", "House");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi trong quá trình đăng nhập Google: {ex.Message}";
                return RedirectToAction("Login");
            }
        }

        // GET: Auth/Logout
        public async Task<IActionResult> Logout()
        {
            // Xóa JWT token khỏi cookie
            Response.Cookies.Delete("AuthToken");

            // Sign out khỏi Google authentication
            await HttpContext.SignOutAsync("Cookies");

            TempData["SuccessMessage"] = "Đăng xuất thành công!";
            return RedirectToAction("Login");
        }
    }
}
