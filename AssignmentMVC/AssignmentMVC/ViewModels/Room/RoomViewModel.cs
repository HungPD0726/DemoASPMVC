namespace AssignmentMVC.ViewModels.Room
{
    public class RoomViewModel
    {
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        public decimal Price { get; set; }
        public string Information { get; set; }
        public double Area { get; set; }
        public int MaxAmountOfPeople { get; set; }
        public int CurrentAmountOfPeople { get; set; }
        public int HouseID { get; set; }

        // Computed property
        public bool IsAvailable => CurrentAmountOfPeople < MaxAmountOfPeople;
    }
}
