using AssignmentMVC.Exceptions;
using AssignmentMVC.Services;
using AssignmentMVC.ViewModels.House;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentMVC.Controllers
{
    public class HouseController : Controller
    {
        private readonly IHouseApiService _houseApiService;

        public HouseController(IHouseApiService houseApiService)
        {
            _houseApiService = houseApiService;
        }

        // GET: /House/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var houses = await _houseApiService.GetAllHousesAsync();
                return View(houses);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Không thể tải danh sách nhà: {ex.Message}";
                return View(new List<HouseViewModel>());
            }
        }

        // GET: /House/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var house = await _houseApiService.GetHouseByIdAsync(id);
                return View(house);
            }
            catch (ApiException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Không thể tải thông tin nhà: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: /House/Create
        public IActionResult Create()
        {
            // Check if user is authenticated
            if (!Request.Cookies.ContainsKey("AuthToken"))
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để thêm nhà mới!";
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        // POST: /House/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HouseCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var success = await _houseApiService.CreateHouseAsync(model);

                if (success)
                {
                    TempData["SuccessMessage"] = "Thêm nhà mới thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Không thể thêm nhà mới!");
                    return View(model);
                }
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                return View(model);
            }
        }

        // GET: /House/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            // Check if user is authenticated
            if (!Request.Cookies.ContainsKey("AuthToken"))
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để chỉnh sửa!";
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var house = await _houseApiService.GetHouseByIdAsync(id);

                var updateModel = new HouseUpdateViewModel
                {
                    HouseName = house.HouseName,
                    Information = house.Information,
                    PowerPrice = house.PowerPrice,
                    WaterPrice = house.WaterPrice
                };

                ViewBag.HouseId = id;
                return View(updateModel);
            }
            catch (ApiException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Không thể tải thông tin nhà: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: /House/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HouseUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.HouseId = id;
                return View(model);
            }

            try
            {
                var success = await _houseApiService.UpdateHouseAsync(id, model);

                if (success)
                {
                    TempData["SuccessMessage"] = "Cập nhật nhà thành công!";
                    return RedirectToAction("Details", new { id = id });
                }
                else
                {
                    ModelState.AddModelError("", "Không thể cập nhật nhà!");
                    ViewBag.HouseId = id;
                    return View(model);
                }
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.HouseId = id;
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                ViewBag.HouseId = id;
                return View(model);
            }
        }

        // GET: /House/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            // Check if user is authenticated
            if (!Request.Cookies.ContainsKey("AuthToken"))
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để xóa!";
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var house = await _houseApiService.GetHouseByIdAsync(id);
                return View(house);
            }
            catch (ApiException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Không thể tải thông tin nhà: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: /House/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _houseApiService.DeleteHouseAsync(id);

                if (success)
                {
                    TempData["SuccessMessage"] = "Xóa nhà thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể xóa nhà!";
                }

                return RedirectToAction("Index");
            }
            catch (ApiException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
