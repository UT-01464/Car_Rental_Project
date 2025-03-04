﻿namespace Carrental.Entities
{
    public class Rental
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CarId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public Boolean OverDue {  get; set; }
        public string Status { get; set; } = "pending";
        public Customer Customer { get; set; }
        public Car Car { get; set; }

    }
}
