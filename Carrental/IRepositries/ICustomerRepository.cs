using Carrental.Entities;

namespace Carrental.IRepositries
{
    public interface ICustomerRepository
    {
        Task<Customer> AddCustomer(Customer customer);
        Task<List<Customer>> GetAllCustomer();
        Task<Customer> UpdateCustomerByID(Customer customer);
        Task<bool> DeleteCustomer(Guid id);
    }
}