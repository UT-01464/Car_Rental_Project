using Carrental.Dtos.RequestDTO;
using Carrental.Dtos.ResponceDTO;
using Microsoft.AspNetCore.Mvc;

namespace Carrental.IsServices
{
    public interface ICustomerService
    {
        Task<CustomerResponceDTO> AddCustomer(CustomerRequestDTO customer);
        Task<List<CustomerResponceDTO>> GetAllCustomer();
        Task<CustomerResponceDTO> UpdateCustomerByID(Guid id, CustomerRequestDTO customerRequestDTO);
        Task<bool> DeleteCustomer(Guid id);
    }
}