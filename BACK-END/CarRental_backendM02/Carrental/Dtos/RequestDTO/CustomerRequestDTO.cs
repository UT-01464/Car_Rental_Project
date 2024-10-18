namespace Carrental.Dtos.RequestDTO
{
    public class CustomerRequestDTO
    {
        public string Name { get; set; }
        public int PhoneNo { get; set; }
        public int NIC { get; set; }
        public string Licence { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }


        public void Validate()
        {
            if (string.IsNullOrEmpty(Email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(Email));
            }

            // You can add more validations for other properties as needed
        }





    }
}
