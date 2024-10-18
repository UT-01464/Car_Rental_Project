using Carrental.Entities;
using Carrental.IRepositries;
using System.Data;
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
        public async Task<List<Customer>> GetAllCustomer()
        {
            var customers = new List<Customer>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Customers", connection);

                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    customers.Add(new Customer
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        PhoneNo = reader.GetInt32(reader.GetOrdinal("PhoneNo")),
                        Licence = reader.GetString(reader.GetOrdinal("Licence")),
                        NIC = reader.GetInt32(reader.GetOrdinal("Nic")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                        Email = reader.GetString(reader.GetOrdinal("Email"))

                    });
                }
            }
            return customers;
        }

        public async Task<Customer> UpdateCustomerByID(Customer customer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "UPDATE Customer SET Name =@Name,PhoneNo=@PhoneNo,Licence=@Licence,NIC=@NIC,Password=@Password, Email = @Email WHERE Id = @Id", connection);
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

        public async Task<bool> DeleteCustomer(Guid id)
        {
            Customer customer = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string sql = "DELETE FROM Customer WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id });

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
        }

    }
}