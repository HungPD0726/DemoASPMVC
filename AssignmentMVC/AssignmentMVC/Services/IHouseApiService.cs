using AssignmentMVC.ViewModels.House;

namespace AssignmentMVC.Services
{
    public interface IHouseApiService
    {
        Task<List<HouseViewModel>> GetAllHousesAsync();
        Task<HouseDetailViewModel> GetHouseByIdAsync(int id);
        Task<bool> CreateHouseAsync(HouseCreateViewModel model);
        Task<bool> UpdateHouseAsync(int id, HouseUpdateViewModel model);
        Task<bool> DeleteHouseAsync(int id);
    }
}
