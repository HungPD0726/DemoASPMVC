using AssignmentMVC.ViewModels.Room;

namespace AssignmentMVC.ViewModels.House
{
    public class HouseDetailViewModel
    {
        public int HouseID { get; set; }
        public string HouseName { get; set; }
        public string Information { get; set; }
        public decimal PowerPrice { get; set; }
        public decimal WaterPrice { get; set; }
        public int LandlordID { get; set; }
        public List<RoomViewModel> Rooms { get; set; } = new List<RoomViewModel>();
    }
}
