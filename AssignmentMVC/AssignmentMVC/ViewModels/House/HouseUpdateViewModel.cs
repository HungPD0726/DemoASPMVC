using System.ComponentModel.DataAnnotations;

namespace AssignmentMVC.ViewModels.House
{
    public class HouseUpdateViewModel
    {
        [Required(ErrorMessage = "Tên nhà là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tên nhà không được quá 200 ký tự")]
        [Display(Name = "Tên nhà")]
        public string HouseName { get; set; }

        [Required(ErrorMessage = "Thông tin là bắt buộc")]
        [Display(Name = "Thông tin chi tiết")]
        public string Information { get; set; }

        [Required(ErrorMessage = "Giá điện là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá điện phải lớn hơn 0")]
        [Display(Name = "Giá điện (VNĐ/kWh)")]
        public decimal PowerPrice { get; set; }

        [Required(ErrorMessage = "Giá nước là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá nước phải lớn hơn 0")]
        [Display(Name = "Giá nước (VNĐ/m³)")]
        public decimal WaterPrice { get; set; }
    }
}
