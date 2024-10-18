namespace Carrental.Dtos.RequestDTO
{
    public class RentalRequestDTO
    {
        public Guid CustomerId { get; set; }
        public Guid CCarId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public Boolean OverDue { get; set; }
        public string Status { get; set; } = "pending";
    }
}
