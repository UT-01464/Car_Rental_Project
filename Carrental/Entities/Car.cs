namespace Carrental.Entities
{
    public class Car
    {
        public Guid Id { get; set; }
        public string RegistorNo { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string RentalPrice { get; set; }
        public string CarImage { get; set; }
        
    }
}
