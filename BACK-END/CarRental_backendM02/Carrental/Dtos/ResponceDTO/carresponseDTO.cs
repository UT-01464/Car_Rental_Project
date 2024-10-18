//namespace Carrental.Dtos.ResponceDTO
//{
//    public class carresponseDTO
//    {
//        public Guid ID { get; set; }
//        public string Title { get; set; }
//        public string ImageUrl { get; set; }
//        public string Regnumber { get; set; }
//        public string Brand { get; set; }
//        public string Category { get; set; }
//        public string Description { get; set; }
//        public string Model { get; set; }
//        public bool IsAvailable { get; set; }
//    }
//}


using System;

namespace Carrental.Dtos.ResponseDTO
{
    public class CarResponseDTO
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string RegistorNo { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string ImageUrl { get; set; }

        public string RentalPrice {  get; set; }
        public bool IsAvailable { get; set; }
    }
}

