using AssignmentMVC.ViewModels.House;

namespace AssignmentMVC.Services
{
    public class HouseApiService : IHouseApiService
    {
        private readonly IApiClient _apiClient;

        public HouseApiService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<HouseViewModel>> GetAllHousesAsync()
        {
            try
            {
                return await _apiClient.GetAsync<List<HouseViewModel>>("/api/house");
            }
            catch
            {
                return new List<HouseViewModel>();
            }
        }

        public async Task<HouseDetailViewModel> GetHouseByIdAsync(int id)
        {
            return await _apiClient.GetAsync<HouseDetailViewModel>($"/api/house/{id}");
        }

        public async Task<bool> CreateHouseAsync(HouseCreateViewModel model)
        {
            var createData = new
            {
                houseName = model.HouseName,
                information = model.Information,
                powerPrice = model.PowerPrice,
                waterPrice = model.WaterPrice
            };

            var response = await _apiClient.PostAsync<object>("/api/house", createData);
            return response != null;
        }

        public async Task<bool> UpdateHouseAsync(int id, HouseUpdateViewModel model)
        {
            var updateData = new
            {
                houseName = model.HouseName,
                information = model.Information,
                powerPrice = model.PowerPrice,
                waterPrice = model.WaterPrice
            };

            var response = await _apiClient.PutAsync<object>($"/api/house/{id}", updateData);
            return response != null;
        }

        public async Task<bool> DeleteHouseAsync(int id)
        {
            return await _apiClient.DeleteAsync($"/api/house/{id}");
        }
    }
}
