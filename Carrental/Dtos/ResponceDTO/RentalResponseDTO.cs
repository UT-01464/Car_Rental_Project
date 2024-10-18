using Carrental.Entities;

namespace Carrental.Dtos.ResponceDTO
{
    public class RentalResponseDTO
    {
        public Guid id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CCarId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public Boolean OverDue { get; set; }
        public string Status { get; set; } = "pending";
        public Customer Customer { get; set; }
        public Car Car { get; set; }
    }
}
