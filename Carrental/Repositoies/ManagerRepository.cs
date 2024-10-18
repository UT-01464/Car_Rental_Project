using Carrental.Entities;
using Carrental.IRepositries;
using System.Data;
using System.Data.SqlClient;

namespace Carrental.Repositoies
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly string _connectionString;

        public ManagerRepository(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        public async Task<List<Rental>> GetAllRentals()
        {
            var rentals = new List<Rental>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Rentals", connection);

                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    rentals.Add(new Rental
                    {
                        id = reader.GetGuid(reader.GetOrdinal("Id")),
                        CustomerId = reader.GetGuid(reader.GetOrdinal("CustomerId")),
                        CCarId = reader.GetGuid(reader.GetOrdinal("CCarID")),
                        RentalDate = reader.GetDateTime(reader.GetOrdinal("RentalDate")),
                        ReturnDate = reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                        OverDue = reader.GetBoolean(reader.GetOrdinal("OverDue")),
                        Status = reader.GetString(reader.GetOrdinal("Status"))

                    });
                }
            }
            return rentals;
        }

        public async Task<Rental> GetRentalByID(Guid rentalId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "SELECT * FROM Rentals WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", rentalId);

                var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);
                if (await reader.ReadAsync())
                {
                    return new Rental
                    {
                        id = reader.GetGuid(reader.GetOrdinal("Id")),
                        CustomerId = reader.GetGuid(reader.GetOrdinal("CustomerId")),
                        CCarId = reader.GetGuid(reader.GetOrdinal("CarID")),
                        RentalDate = reader.GetDateTime(reader.GetOrdinal("RentalDate")),
                        ReturnDate = reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                        OverDue = reader.GetBoolean(reader.GetOrdinal("OverDue")),
                        Status = reader.GetString(reader.GetOrdinal("Status")),
                    };
                }
                return null;
            }
        }

        public async Task<List<Rental>> GetAllRentalsByCustomerID(Guid customerId)
        {
            var rentals = new List<Rental>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "SELECT * FROM Rentals WHERE CustomerId = @CustomerId", connection);
                command.Parameters.AddWithValue("@CustomerId", customerId);

                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    rentals.Add(new Rental
                    {
                        id = reader.GetGuid(reader.GetOrdinal("Id")),
                        // Populate other properties
                    });
                }
            }
            return rentals;
        }


        public async Task<Rental> AddRental(Rental rental)
        {
            rental.id = Guid.NewGuid();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "INSERT INTO Rentals (Id, CustomerId, CarId,RentalDate,ReturnDate,OverDue, Status) VALUES (@Id, @CustomerId, @CarId,@RentalDate,@ReturnDate,@OverDue, @Status); SELECT SCOPE_IDENTITY();", connection);
                command.Parameters.AddWithValue("@Id", rental.id);
                command.Parameters.AddWithValue("@CustomerId", rental.CustomerId);
                command.Parameters.AddWithValue("@CarId", rental.CCarId);
                command.Parameters.AddWithValue("@RentalDate", DateTime.Now);
                command.Parameters.AddWithValue("@ReturnDate", DBNull.Value);
                command.Parameters.AddWithValue("@OverDue", rental.OverDue);
                command.Parameters.AddWithValue("@Status", rental.Status);

                await command.ExecuteScalarAsync();

                return rental;
            }
        }

        public async Task<Rental> AcceptRental(Rental rental)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "UPDATE Rentals SET Status = @Status WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", rental.id);
                command.Parameters.AddWithValue("@Status", "Rent");

                await command.ExecuteNonQueryAsync();
                var selectCommand = new SqlCommand(
                    "SELECT * FROM Rentals WHERE Id = @Id", connection);
                selectCommand.Parameters.AddWithValue("@Id", rental.id);

                using (var reader = await selectCommand.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        // Map the updated rental data
                        rental.Status = reader["Status"].ToString();
                        rental.ReturnDate = reader.GetDateTime(reader.GetOrdinal("ReturnDate"));
                        rental.OverDue = reader.GetBoolean(reader.GetOrdinal("OverDue"));
                        // Map other necessary fields
                    }
                }

                return rental;
            }
        }


        public async Task<Rental> RejectRental(Guid id)
        {
            Rental rental = null;
            using (var connection = new SqlConnection(_connectionString))
            {

                var selectQuery = "SELECT * FROM Rentals WHERE Id = @Id";
                using (var selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    using (var reader = await selectCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            rental = new Rental
                            {
                                id = reader.GetGuid(reader.GetOrdinal("Id")),
                                CustomerId = reader.GetGuid(reader.GetOrdinal("CustomerId")),
                                CCarId = reader.GetGuid(reader.GetOrdinal("CarID")),
                                RentalDate = reader.GetDateTime(reader.GetOrdinal("RentalDate")),
                                ReturnDate = reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                                OverDue = reader.GetBoolean(reader.GetOrdinal("OverDue")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                            };
                        }
                        connection.Close();
                    }
                }


                var deleteQuery = "DELETE FROM Rentals WHERE Id = @Id";
                using (var deleteCommand = new SqlCommand(deleteQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    await deleteCommand.ExecuteNonQueryAsync();
                    connection.Close();
                }
            }
            return rental;
        }

        public async Task<Rental> UpdateReturn(Rental rental)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "UPDATE Rentals SET Status = @Status WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", rental.id);
                command.Parameters.AddWithValue("@Status", "Return");

                await command.ExecuteNonQueryAsync();
                return rental;
            }
        }

       



        public async Task<List<Guid>> CheckAndUpdateOverdueRentals()
        {
            var overdueRentalIds = new List<Guid>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var selectCommand = new SqlCommand(
                    "SELECT * FROM Rentals WHERE ReturnDate IS NOT NULL AND OverDue = 0", connection);

                using (var reader = await selectCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var returndate = reader.GetDateTime(reader.GetOrdinal("ReturnDate"));
                        var rentalId = reader.GetGuid(reader.GetOrdinal("Id"));

                        if (DateTime.Now > returndate.AddHours(24))
                        {
                            overdueRentalIds.Add(rentalId);
                        }
                    }

                }

                foreach (var rentalId in overdueRentalIds)
                {
                    var updateCommand = new SqlCommand(
                        "UPDATE Rentals SET OverDue = 1 WHERE Id = @Id", connection);
                    updateCommand.Parameters.AddWithValue("@Id", rentalId);

                    await updateCommand.ExecuteNonQueryAsync();
                }
            }

            return overdueRentalIds;
        }

    }
}
