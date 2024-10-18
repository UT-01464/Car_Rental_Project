//namespace Carrental.Dtos.RequestDTO
//{
//    public class carrequestDTO
//    {
//        public string Title { get; set; }
//        public string Regnumber { get; set; }
//        public string Brand { get; set; }
//        public string Category { get; set; }
//        public string Description { get; set; }
//        public string Model { get; set; }
//        public IFormFile ImageFile { get; set; }
//    }
//}

namespace Carrental.Dtos.RequestDTO
{
    public class CarRequestDTO // Capitalized to follow C# naming conventions
    {
        public string Title { get; set; }
        public string RegistorNo { get; set; } // Match the SQL column name
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public string Category {  get; set; }
        public string RentalPrice { get; set; } // Added RentalPrice field
        public IFormFile ImageFile { get; set; }
    }
}

