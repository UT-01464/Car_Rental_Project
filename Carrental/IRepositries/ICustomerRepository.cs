using Carrental.Entities;

namespace Carrental.IRepositries
{
    public interface  ICustomerRepository 
    {
        Task<Customer> AddCustomer(Customer customer);
    }
}
