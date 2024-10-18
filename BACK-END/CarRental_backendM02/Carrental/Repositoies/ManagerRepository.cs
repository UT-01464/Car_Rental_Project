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
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        CustomerId = reader.GetGuid(reader.GetOrdinal("CustomerId")),
                        CarId = reader.GetGuid(reader.GetOrdinal("CarID")),
                        RentalDate = reader.GetDateTime(reader.GetOrdinal("RentalDate")),
                        ReturnDate = reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                        OverDue = reader.GetBoolean(reader.GetOrdinal("OverDue")),
                        Status = reader.GetString(reader.GetOrdinal("Status"))

                    });
                }
            }
            return rentals;
        }

        //public async Task<Rental> GetRentalByID(Guid rentalId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        var command = new SqlCommand(
        //            "SELECT * FROM Rentals WHERE Id = @Id", connection);
        //        command.Parameters.AddWithValue("@Id", rentalId);

        //        var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);
        //        if (await reader.ReadAsync())
        //        {
        //            return new Rental
        //            {
        //                Id = reader.GetGuid(reader.GetOrdinal("Id")),
        //                CustomerId = reader.GetGuid(reader.GetOrdinal("CustomerId")),
        //                CarId = reader.GetGuid(reader.GetOrdinal("CarID")),
        //                RentalDate = reader.GetDateTime(reader.GetOrdinal("RentalDate")),
        //                ReturnDate = reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
        //                OverDue = reader.GetBoolean(reader.GetOrdinal("OverDue")),
        //                Status = reader.GetString(reader.GetOrdinal("Status")),
        //            };
        //        }
        //        return null;
        //    }
        //}

        public async Task<Rental> GetRentalByID(Guid rentalId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("SELECT * FROM Rentals WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", rentalId);

                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
                {
                    if (await reader.ReadAsync())
                    {
                        return new Rental
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            CustomerId = reader.GetGuid(reader.GetOrdinal("CustomerId")),
                            CarId = reader.GetGuid(reader.GetOrdinal("CarId")), // Ensure the case matches your database column
                            RentalDate = reader.GetDateTime(reader.GetOrdinal("RentalDate")),
                            ReturnDate = reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                            OverDue = reader.GetBoolean(reader.GetOrdinal("OverDue")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                        };
                    }
                }
            }

            return null; // Rental not found
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
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        // Populate other properties
                    });
                }
            }
            return rentals;
        }


        //public async Task<Rental> AddRental(Rental rental)
        //{
        //    rental.id = Guid.NewGuid();

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        var command = new SqlCommand(
        //            "INSERT INTO Rentals (Id, CustomerId, CarId,RentalDate,ReturnDate,OverDue, Status) VALUES (@Id, @CustomerId, @CarId,@RentalDate,@ReturnDate,@OverDue, @Status); SELECT SCOPE_IDENTITY();", connection);
        //        command.Parameters.AddWithValue("@Id", rental.id);
        //        command.Parameters.AddWithValue("@CustomerId", rental.CustomerId);
        //        command.Parameters.AddWithValue("@CarId", rental.CCarId);
        //        command.Parameters.AddWithValue("@RentalDate", DateTime.Now);
        //        command.Parameters.AddWithValue("@ReturnDate", DBNull.Value);
        //        command.Parameters.AddWithValue("@OverDue", rental.OverDue);
        //        command.Parameters.AddWithValue("@Status", rental.Status);

        //        await command.ExecuteScalarAsync();

        //        return rental;
        //    }
        //}


        public async Task<Rental> AddRental(Rental rental)
        {
            // Generate a new ID for the rental
            rental.Id = Guid.NewGuid();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Check if the CustomerId exists in the Customers table
                var customerCheckCommand = new SqlCommand(
                    "SELECT COUNT(*) FROM Customers WHERE Id = @CustomerId;", connection);
                customerCheckCommand.Parameters.AddWithValue("@CustomerId", rental.CustomerId);

                int customerExists = (int)await customerCheckCommand.ExecuteScalarAsync();
                if (customerExists == 0)
                {
                    throw new ArgumentException($"Customer with ID {rental.CustomerId} does not exist.");
                }

                // Check if the CarId exists in the Cars table
                var carCheckCommand = new SqlCommand(
                    "SELECT COUNT(*) FROM Cars WHERE Id = @CarId;", connection);
                carCheckCommand.Parameters.AddWithValue("@CarId", rental.CarId);

                int carExists = (int)await carCheckCommand.ExecuteScalarAsync();
                if (carExists == 0)
                {
                    throw new ArgumentException($"Car with ID {rental.CarId} does not exist.");
                }

                // Set default values for OverDue and Status
                rental.OverDue = false; // Assume new rentals are not overdue
                rental.Status = "Pending"; // Set initial status to Pending

                // Calculate the ReturnDate based on your business logic, e.g., 3 days after RentalDate
                DateTime rentalDate = DateTime.Now;
                DateTime returnDate = rentalDate.AddDays(3); // Change the duration as needed

                // Prepare the SQL command to insert the rental record
                var command = new SqlCommand(
                    "INSERT INTO Rentals (Id, CustomerId, CarId, RentalDate, ReturnDate, OverDue, Status) VALUES (@Id, @CustomerId, @CarId, @RentalDate, @ReturnDate, @OverDue, @Status);",
                    connection);

                // Add parameters for the command
                command.Parameters.AddWithValue("@Id", rental.Id);
                command.Parameters.AddWithValue("@CustomerId", rental.CustomerId);
                command.Parameters.AddWithValue("@CarId", rental.CarId);
                command.Parameters.AddWithValue("@RentalDate", rentalDate);
                command.Parameters.AddWithValue("@ReturnDate", returnDate);
                command.Parameters.AddWithValue("@OverDue", rental.OverDue);
                command.Parameters.AddWithValue("@Status", rental.Status);

                // Execute the command
                await command.ExecuteNonQueryAsync();

                // Set the rental properties that may be needed after insertion
                rental.RentalDate = rentalDate; // Set the RentalDate
                rental.ReturnDate = returnDate; // Set the ReturnDate

                return rental; // Return the rental object with the populated values
            }
        }





        //public async Task<Rental> AcceptRental(Rental rental)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        var command = new SqlCommand(
        //            "UPDATE Rentals SET Status = @Status WHERE Id = @Id", connection);
        //        command.Parameters.AddWithValue("@Id", rental.Id);
        //        command.Parameters.AddWithValue("@Status", "Rent");

        //        await command.ExecuteNonQueryAsync();
        //        var selectCommand = new SqlCommand(
        //            "SELECT * FROM Rentals WHERE Id = @Id", connection);
        //        selectCommand.Parameters.AddWithValue("@Id", rental.Id);

        //        using (var reader = await selectCommand.ExecuteReaderAsync())
        //        {
        //            if (await reader.ReadAsync())
        //            {
        //                // Map the updated rental data
        //                rental.Status = reader["Status"].ToString();
        //                rental.ReturnDate = reader.GetDateTime(reader.GetOrdinal("ReturnDate"));
        //                rental.OverDue = reader.GetBoolean(reader.GetOrdinal("OverDue"));
        //                // Map other necessary fields
        //            }
        //        }

        //        return rental;
        //    }
        //}
        public async Task<Rental> AcceptRental(Guid rentalId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // First, retrieve the rental to check its status
                var rental = await GetRentalByID(rentalId);
                if (rental == null)
                {
                    throw new InvalidOperationException("Rental not found.");
                }

                // Check if the rental is in 'Pending' status
                if (rental.Status != "Pending")
                {
                    throw new InvalidOperationException($"Rental cannot be accepted, current status: {rental.Status}");
                }

                // Update the rental status to "Rent" and set the ReturnDate
                var command = new SqlCommand(
                    "UPDATE Rentals SET Status = @Status, ReturnDate = @ReturnDate WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", rental.Id);
                command.Parameters.AddWithValue("@Status", "Rent");

                // Set ReturnDate to a specific duration from now (e.g., 7 days)
                var returnDate = DateTime.Now.AddDays(7); // Example: 7-day rental
                command.Parameters.AddWithValue("@ReturnDate", returnDate);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("Failed to update rental status.");
                }

                // Optionally, retrieve the updated rental data to return
                rental.Status = "Rent"; // Update status in the local object
                rental.ReturnDate = returnDate; // Update return date in the local object

                return rental; // Return the updated rental object
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
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                CustomerId = reader.GetGuid(reader.GetOrdinal("CustomerId")),
                                CarId = reader.GetGuid(reader.GetOrdinal("CarID")),
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
                command.Parameters.AddWithValue("@Id", rental.Id);
                command.Parameters.AddWithValue("@Status", "Return");

                await command.ExecuteNonQueryAsync();
                return rental;
            }
        }





        //public async Task<List<Guid>> CheckAndUpdateOverdueRentals()
        //{
        //    var overdueRentalIds = new List<Guid>();

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();

        //        var selectCommand = new SqlCommand(
        //            "SELECT * FROM Rentals WHERE ReturnDate IS NOT NULL AND OverDue = 0", connection);

        //        using (var reader = await selectCommand.ExecuteReaderAsync())
        //        {
        //            while (await reader.ReadAsync())
        //            {
        //                var returndate = reader.GetDateTime(reader.GetOrdinal("ReturnDate"));
        //                var rentalId = reader.GetGuid(reader.GetOrdinal("Id"));

        //                if (DateTime.Now > returndate.AddHours(24))
        //                {
        //                    overdueRentalIds.Add(rentalId);
        //                }
        //            }

        //        }

        //        foreach (var rentalId in overdueRentalIds)
        //        {
        //            var updateCommand = new SqlCommand(
        //                "UPDATE Rentals SET OverDue = 1 WHERE Id = @Id", connection);
        //            updateCommand.Parameters.AddWithValue("@Id", rentalId);

        //            await updateCommand.ExecuteNonQueryAsync();
        //        }
        //    }

        //    return overdueRentalIds;
        //}

        public async Task<List<Guid>> CheckAndUpdateOverdueRentals()
        {
            var overdueRentalIds = new List<Guid>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Identify overdue rentals
                var command = new SqlCommand(@"
            SELECT Id, RentalDate, ReturnDate
            FROM Rentals
            WHERE OverDue = 0", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var rentalId = reader.GetGuid(reader.GetOrdinal("Id"));
                        var rentalDate = reader.GetDateTime(reader.GetOrdinal("RentalDate"));
                        var returnDate = reader.GetDateTime(reader.GetOrdinal("ReturnDate"));

                        // Check if the actual return date is greater than the rental period
                        if (returnDate > rentalDate.AddDays(3)) // Assuming 3 days as the rental period
                        {
                            overdueRentalIds.Add(rentalId);
                        }
                    }
                }

                // Update the OverDue status for the identified rentals
                foreach (var rentalId in overdueRentalIds)
                {
                    var updateCommand = new SqlCommand(
                        "UPDATE Rentals SET OverDue = 1 WHERE Id = @Id", connection);
                    updateCommand.Parameters.AddWithValue("@Id", rentalId);
                    await updateCommand.ExecuteNonQueryAsync();
                }
            }

            return overdueRentalIds; // Return the list of overdue rental IDs
        }





    }
}
