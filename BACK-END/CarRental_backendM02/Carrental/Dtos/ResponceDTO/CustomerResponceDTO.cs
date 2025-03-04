﻿using Carrental.Entities;

namespace Carrental.Dtos.ResponceDTO
{
    public class CustomerResponceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int PhoneNo { get; set; }
        public int NIC { get; set; }
        public string Licence { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public ICollection<Rental>? RentalHistory { get; set; }
    }
}
