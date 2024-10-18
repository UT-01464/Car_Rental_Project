using Carrental.Dtos.RequestDTO;
using Carrental.Dtos.ResponceDTO;

namespace Carrental.IsServices
{
    public interface ICustomerService 
    {
        Task<CustomerResponceDTO> AddCustomer(CustomerRequestDTO customer);
    }
}
