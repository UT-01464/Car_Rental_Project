using Carrental.Entities;
using Carrental.IRepositries;
using System.Data.SqlClient;

namespace Carrental.Repositoies
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            if (customer.Id == Guid.Empty)
            {
                customer.Id = Guid.NewGuid();
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "INSERT INTO Customer(Id,Name,PhoneNo,NIC,Licence,Password,Email)" +
                    "VALUES (@Id,@Name,@PhoneNo,@NIC,@Licence,@Password,@Email)", connection
                    );

                command.Parameters.AddWithValue("@Id", customer.Id);
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@PhoneNo", customer.PhoneNo);
                command.Parameters.AddWithValue("@NIC", customer.NIC);
                command.Parameters.AddWithValue("@Licence", customer.Licence);
                command.Parameters.AddWithValue("@Password", customer.Password);
                command.Parameters.AddWithValue("@Email", customer.Email);

                await command.ExecuteNonQueryAsync();

                return customer;

            }
        }
        public async Task<List<Customer>> GetCustomer()
        {
            var customers = new List<Customer>();


            return customers;
        }
    }
}
