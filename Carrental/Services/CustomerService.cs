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

    }
}

