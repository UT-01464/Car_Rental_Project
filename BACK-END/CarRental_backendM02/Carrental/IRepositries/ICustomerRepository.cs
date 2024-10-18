using Carrental.Dtos.RequestDTO;
using Carrental.Entities;

namespace Carrental.IRepositries
{
    public interface  ICustomerRepository 
    {
        Task<Customer> AddCustomer(Customer customer);
        Task<List<Customer>> GetAllCustomer();
        Task<Customer> UpdateCustomerByID(Guid id, CustomerRequestDTO customerRequestDTO);
        Task<bool> DeleteCustomer(Guid id);
        Task<Customer> GetCustomerById(Guid id);
        Task<Customer> AuthenticateCustomer(int nic, string password);
    }
}
