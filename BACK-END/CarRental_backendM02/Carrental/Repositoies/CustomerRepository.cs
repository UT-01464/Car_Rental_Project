using Carrental.Dtos.RequestDTO;
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

        //public async Task<Customer> AddCustomer(Customer customer)
        //{
        //    if (customer.Id == Guid.Empty)
        //    {
        //        customer.Id = Guid.NewGuid();
        //    }

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        var command = new SqlCommand(
        //            "INSERT INTO Customers(Id,Name,PhoneNo,NIC,Licence,Password,Email)" +
        //            "VALUES (@Id,@Name,@PhoneNo,@NIC,@Licence,@Password,@Email)", connection
        //            );

        //        command.Parameters.AddWithValue("@Id", customer.Id);
        //        command.Parameters.AddWithValue("@Name", customer.Name);
        //        command.Parameters.AddWithValue("@PhoneNo", customer.PhoneNo);
        //        command.Parameters.AddWithValue("@NIC", customer.NIC);
        //        command.Parameters.AddWithValue("@Licence", customer.Licence);
        //        command.Parameters.AddWithValue("@Password", customer.Password);
        //        command.Parameters.AddWithValue("@Email", customer.Email);

        //        await command.ExecuteNonQueryAsync();

        //        return customer;

        //    }
        //}


        public async Task<Customer> AddCustomer(Customer customer)
        {
            // Generate a new GUID if the customer Id is empty
            if (customer.Id == Guid.Empty)
            {
                customer.Id = Guid.NewGuid();
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Check for existing customer with the same NIC or Email
                var checkQuery = "SELECT COUNT(*) FROM Customers WHERE NIC = @NIC OR Email = @Email";
                using (var checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@NIC", customer.NIC);
                    checkCommand.Parameters.AddWithValue("@Email", customer.Email);

                    var existingCount = (int)await checkCommand.ExecuteScalarAsync();
                    if (existingCount > 0)
                    {
                        throw new InvalidOperationException("A customer with this NIC or Email already exists.");
                    }
                }

                // Insert the new customer into the database
                var insertQuery = "INSERT INTO Customers (Id, Name, PhoneNo, NIC, Licence, Password, Email) " +
                                  "VALUES (@Id, @Name, @PhoneNo, @NIC, @Licence, @Password, @Email)";

                using (var command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = customer.Id;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = customer.Name;
                    command.Parameters.Add("@PhoneNo", SqlDbType.NVarChar).Value = customer.PhoneNo;
                    command.Parameters.Add("@NIC", SqlDbType.NVarChar).Value = customer.NIC;
                    command.Parameters.Add("@Licence", SqlDbType.NVarChar).Value = customer.Licence;
                    command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = customer.Password;
                    command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = customer.Email;

                    await command.ExecuteNonQueryAsync();
                }

                // Optionally, retrieve the customer from the database (if you want to return the created customer)
                return customer; // Return the customer with the ID assigned
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

        //public async Task<Customer> UpdateCustomerByID(Customer customer)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        var command = new SqlCommand(
        //            "UPDATE Customer SET Name =@Name,PhoneNo=@PhoneNo,Licence=@Licence,NIC=@NIC,Password=@Password, Email = @Email WHERE Id = @Id", connection);
        //        command.Parameters.AddWithValue("@Id", customer.Id);
        //        command.Parameters.AddWithValue("@Name", customer.Name);
        //        command.Parameters.AddWithValue("@PhoneNo", customer.PhoneNo);
        //        command.Parameters.AddWithValue("@NIC", customer.NIC);
        //        command.Parameters.AddWithValue("@Licence", customer.Licence);
        //        command.Parameters.AddWithValue("@Password", customer.Password);
        //        command.Parameters.AddWithValue("@Email", customer.Email);

        //        await command.ExecuteNonQueryAsync();
        //        return customer;
        //    }
        //}

        public async Task<Customer> UpdateCustomerByID(Guid id, CustomerRequestDTO customerRequestDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Validate the Email field
                if (string.IsNullOrEmpty(customerRequestDTO.Email))
                {
                    throw new ArgumentException("Email cannot be null or empty");
                }

                await connection.OpenAsync();

                var command = new SqlCommand(
                    @"UPDATE Customers 
              SET Name = @Name,
                  PhoneNo = @PhoneNo,
                  Licence = @Licence,
                  NIC = @NIC,
                  Password = @Password,
                  Email = @Email
              WHERE Id = @Id", connection);

                // Parameters
                command.Parameters.AddWithValue("@Id", id); // Use the provided id parameter
                command.Parameters.AddWithValue("@Name", customerRequestDTO.Name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PhoneNo", customerRequestDTO.PhoneNo);
                command.Parameters.AddWithValue("@NIC", customerRequestDTO.NIC);
                command.Parameters.AddWithValue("@Licence", customerRequestDTO.Licence ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Password", customerRequestDTO.Password ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", customerRequestDTO.Email ?? (object)DBNull.Value);

                await command.ExecuteNonQueryAsync();

                // Optionally, you might want to return the updated customer. If you need to fetch it again, you can do that here.

                return new Customer // Create a new Customer object from the DTO for return if needed
                {
                    Id = id,
                    Name = customerRequestDTO.Name,
                    PhoneNo = customerRequestDTO.PhoneNo,
                    NIC = customerRequestDTO.NIC,
                    Licence = customerRequestDTO.Licence,
                    Password = customerRequestDTO.Password,
                    Email = customerRequestDTO.Email
                };
            }
        }


        public async Task<bool> DeleteCustomer(Guid id)
        {
            Customer customer = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string sql = "DELETE FROM Customers WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id });

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
        }


        //public async Task<Customer> GetCustomerById(Guid id)
        //{
        //    Customer customer = null;
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        var command = new SqlCommand("SELECT * FROM Customers WHERE Id = @Id", connection);
        //        command.Parameters.AddWithValue("@Id", id);

        //        var reader = await command.ExecuteReaderAsync();
        //        if (await reader.ReadAsync())
        //        {
        //            customer = new Customer
        //            {
        //                Id = reader.GetGuid(reader.GetOrdinal("Id")),
        //                Name = reader.GetString(reader.GetOrdinal("Name")),
        //                PhoneNo = reader.GetInt32(reader.GetOrdinal("PhoneNo")),
        //                Licence = reader.GetString(reader.GetOrdinal("Licence")),
        //                NIC = reader.GetInt32(reader.GetOrdinal("Nic")),
        //                Password = reader.GetString(reader.GetOrdinal("Password")),
        //                Email = reader.GetString(reader.GetOrdinal("Email"))
        //            };
        //        }
        //    }
        //    return customer;
        //}

        public async Task<Customer> GetCustomerById(Guid id)
        {
            Customer customer = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Customers WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    customer = new Customer
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                        PhoneNo = reader.IsDBNull(reader.GetOrdinal("PhoneNo")) ? 0 : reader.GetInt32(reader.GetOrdinal("PhoneNo")), // Provide default value
                        Licence = reader.IsDBNull(reader.GetOrdinal("Licence")) ? null : reader.GetString(reader.GetOrdinal("Licence")),
                        NIC = reader.IsDBNull(reader.GetOrdinal("NIC")) ? 0 : reader.GetInt32(reader.GetOrdinal("NIC")), // Provide default value
                        Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? null : reader.GetString(reader.GetOrdinal("Password")),
                        Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email"))
                    };
                }
            }
            return customer;
        }



        public async Task<Customer> AuthenticateCustomer(int nic, string password)
        {
            Customer customer = null;  // This will hold the customer if found
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Query to check if a customer with the provided NIC and Password exists
                var command = new SqlCommand("SELECT * FROM Customers WHERE NIC = @nic AND Password = @password", connection);

                // Add parameters to prevent SQL injection
                command.Parameters.AddWithValue("@nic", nic);
                command.Parameters.AddWithValue("@password", password);

                var reader = await command.ExecuteReaderAsync();

                // If a record is found, populate the customer object
                if (await reader.ReadAsync())
                {
                    customer = new Customer
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        PhoneNo = reader.GetInt32(reader.GetOrdinal("PhoneNo")),
                        Licence = reader.GetString(reader.GetOrdinal("Licence")),
                        NIC = reader.GetInt32(reader.GetOrdinal("Nic")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                        Email = reader.GetString(reader.GetOrdinal("Email"))
                    };
                }
            }

            // Return the customer if found, or null if not
            return customer;
        }


















    }
}
