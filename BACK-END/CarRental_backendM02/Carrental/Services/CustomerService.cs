using Carrental.Dtos.RequestDTO;
using Carrental.Dtos.ResponceDTO;
using Carrental.Entities;
using Carrental.IRepositries;
using Carrental.IsServices;
using Carrental.Repositoies;

namespace Carrental.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _Repository;

        public CustomerService(ICustomerRepository repository)
        {

            _Repository = repository;
        }

        public async Task<CustomerResponceDTO> AddCustomer(CustomerRequestDTO customer)
        {
            var customerrequest = new Customer
            {
                Name = customer.Name,
                PhoneNo = customer.PhoneNo,
                NIC = customer.NIC,
                Licence = customer.Licence,
                Password = customer.Password,
                Email = customer.Email
            };

            var Data = await _Repository.AddCustomer(customerrequest);

            var customerresponce = new CustomerResponceDTO
            {
                Name = Data.Name,
                PhoneNo = Data.PhoneNo,
                NIC = Data.NIC,
                Licence = Data.Licence,
                Password = Data.Password,
                Email = Data.Email
            };
            return customerresponce;
        }


        public async Task<List<CustomerResponceDTO>> GetAllCustomer()
        {
            var customer = await _Repository.GetAllCustomer();

            var data = new List<CustomerResponceDTO>();
            foreach (var item in customer)
            {
                var customerrespo = new CustomerResponceDTO
                {
                    Name = item.Name,
                    NIC = item.NIC,
                    Id = item.Id,
                    PhoneNo = item.PhoneNo,
                    Licence = item.Licence,
                    Email = item.Email,
                    RentalHistory = item.RentalHistory,
                    Password = item.Password,
                };
                data.Add(customerrespo);
            }
            return data;
        }

        //public async Task<CustomerResponceDTO> UpdateCustomerByID(Guid id, CustomerRequestDTO customerRequestDTO)
        //{
        //    var customerreque = new Customer
        //    {
        //        Id = id,
        //        Name = customerRequestDTO.Name,
        //        PhoneNo = customerRequestDTO.PhoneNo,
        //        Licence = customerRequestDTO.Licence,
        //        Password = customerRequestDTO.Password,
        //        NIC = customerRequestDTO.NIC,
        //    };

        //    var data = await _Repository.UpdateCustomerByID(customerreque);

        //    var customerObj = new CustomerResponceDTO
        //    {
        //        Id = id,
        //        Name = data.Name,
        //        PhoneNo = data.PhoneNo,
        //        Licence = data.Licence,
        //        NIC = data.NIC,
        //        Password = data.Password,
        //        RentalHistory = data.RentalHistory,
        //        Email = data.Email,
        //    };

        //    return customerObj;
        //}


        public async Task<CustomerResponceDTO> UpdateCustomerByID(Guid id, CustomerRequestDTO customerRequestDTO)
        {
            // Ensure you have the required Email property set in customerRequestDTO
            if (string.IsNullOrEmpty(customerRequestDTO.Email))
            {
                throw new ArgumentException("Email cannot be null or empty");
            }

            // Pass the required parameters to the repository method
            var updatedCustomer = await _Repository.UpdateCustomerByID(id, customerRequestDTO);

            // Check if the updatedCustomer is null
            if (updatedCustomer == null)
            {
                throw new Exception("Customer update failed"); // Handle error accordingly
            }

            // Create a response DTO from the updated customer data
            var customerResponse = new CustomerResponceDTO
            {
                Id = updatedCustomer.Id,
                Name = updatedCustomer.Name,
                PhoneNo = updatedCustomer.PhoneNo,
                Licence = updatedCustomer.Licence,
                NIC = updatedCustomer.NIC,
                RentalHistory = updatedCustomer.RentalHistory, // Ensure this property exists
                Email = updatedCustomer.Email,
            };

            return customerResponse;
        }


        public async Task<bool> DeleteCustomer(Guid id)
        {
            var data = await _Repository.DeleteCustomer(id);
            return data;
        }


        public async Task<Customer> GetCustomerById(Guid id)
        {
            var customer = await _Repository.GetCustomerById(id);
            if (customer == null)
            {
                throw new InvalidOperationException("Customer not found.");
            }
            return customer;
        }

        public async Task<Customer> AuthenticateCustomer(int nic, string password)
        {
            // Perform any business logic here before authentication, if necessary

            // Call the repository to authenticate the customer
            var customer = await _Repository.AuthenticateCustomer(nic, password);

            // Optional: additional checks or logic after authentication, e.g. checking account status
            if (customer == null)
            {
                throw new Exception("Invalid NIC or password");
            }

            return customer;
        }
    }
}

