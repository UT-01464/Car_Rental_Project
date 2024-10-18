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

        public async Task<CustomerResponceDTO> UpdateCustomerByID(Guid id, CustomerRequestDTO customerRequestDTO)
        {
            var customerreque = new Customer
            {
                Id = id,
                Name = customerRequestDTO.Name,
                PhoneNo = customerRequestDTO.PhoneNo,
                Licence = customerRequestDTO.Licence,
                Password = customerRequestDTO.Password,
                NIC = customerRequestDTO.NIC,
            };

            var data = await _Repository.UpdateCustomerByID(customerreque);

            var customerObj = new CustomerResponceDTO
            {
                Id = id,
                Name = data.Name,
                PhoneNo = data.PhoneNo,
                Licence = data.Licence,
                NIC = data.NIC,
                Password = data.Password,
                RentalHistory = data.RentalHistory,
                Email = data.Email,
            };

            return customerObj;
        }

        public async Task<bool> DeleteCustomer(Guid id)
        {
            var data = await _Repository.DeleteCustomer(id);
            return data;
        }
    }
}