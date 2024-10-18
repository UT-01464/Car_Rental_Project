namespace Carrental.Entities
{
    public class Car
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int RegistorNo { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string RentalPrice { get; set; }
        public string CarImage { get; set; }

        public bool IsAvailable { get; set; } = true;

    }
}
