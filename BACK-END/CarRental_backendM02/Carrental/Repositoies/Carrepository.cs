//using Carrental.Entities;
//using Carrental.IRepositries;
//using System.Data.SqlClient;
//using Carrental.Helpers; // Add this namespace
//using Microsoft.AspNetCore.Http; // Add this namespace
//using System;
//using System.IO;
//using System.Threading.Tasks;


//namespace Carrental.Repositoies
//{
//    public class Carrepository:Icarrepository
//    {
//        private readonly string _connectionString;
//        private readonly ImageUploadHelper _imageUploadHelper;

//        public Carrepository(string connectionString, ImageUploadHelper imageUploadHelper)
//        {
//            _connectionString = connectionString;
//            _imageUploadHelper = imageUploadHelper; // Initialize the helper
//        }

//        public async Task<Car> AddCar(Car car, IFormFile imageFile)
//        {
//            // Use ImageUploadHelper to upload the image and get the filename
//            var imagePath = _imageUploadHelper.UploadImage(imageFile);
//            if (imagePath != null)
//            {
//                car.CarImage = imagePath; // Set the image path to the car object
//            }

//            using (var connection = new SqlConnection(_connectionString))
//            {
//                var query = "INSERT INTO Cars (Id, Title, CarImage, RegistorNo, Brand, Description, Model, IsAvailable) " +
//                            "VALUES (@Id, @Title, @ImagePath, @Regnumber, @Brand, @Description, @Model, @IsAvailable)";
//                using (var command = new SqlCommand(query, connection))
//                {
//                    command.Parameters.AddWithValue("@Id", car.Id);
//                    command.Parameters.AddWithValue("@Title", car.Title);
//                    command.Parameters.AddWithValue("@ImagePath", car.CarImage);
//                    command.Parameters.AddWithValue("@Regnumber", car.RegistorNo);
//                    command.Parameters.AddWithValue("@Brand", car.Brand);
//                    command.Parameters.AddWithValue("@Description", car.Description);
//                    command.Parameters.AddWithValue("@Model", car.Model);
//                    command.Parameters.AddWithValue("@IsAvailable", car.IsAvailable);

//                    connection.Open();
//                    await command.ExecuteNonQueryAsync();
//                    connection.Close();
//                }
//            }
//            return car;
//        }

//        public async Task<Car> GetCarById(Guid id)
//        {
//            Car car = null;
//            using (var connection = new SqlConnection(_connectionString))
//            {
//                var query = "SELECT * FROM Cars WHERE ID = @ID";
//                using (var command = new SqlCommand(query, connection))
//                {
//                    command.Parameters.AddWithValue("@ID", id);

//                    connection.Open();
//                    using (var reader = await command.ExecuteReaderAsync())
//                    {
//                        if (await reader.ReadAsync())
//                        {
//                            car = new Car
//                            {
//                                Id = reader.GetGuid(reader.GetOrdinal("ID")),
//                                Title = reader.GetString(reader.GetOrdinal("Title")),
//                                CarImage = reader.GetString(reader.GetOrdinal("CarImage")),
//                                RegistorNo = reader.GetString(reader.GetOrdinal("RegistorNo")),
//                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
//                                Description = reader.GetString(reader.GetOrdinal("Description")),
//                                Model = reader.GetString(reader.GetOrdinal("Model")),
//                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),


//                            };
//                        }
//                        connection.Close();
//                    }
//                }
//            }
//            return car;
//        }

//        public async Task<List<Car>> GetAllCars()
//        {
//            var bikes = new List<Car>();
//            using (var connection = new SqlConnection(_connectionString))
//            {
//                var query = "SELECT * FROM Cars";
//                using (var command = new SqlCommand(query, connection))
//                {
//                    connection.Open();
//                    using (var reader = await command.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync())
//                        {
//                            bikes.Add(new Car
//                            {
//                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
//                                Title = reader.GetString(reader.GetOrdinal("Title")),
//                                CarImage = reader.GetString(reader.GetOrdinal("ImagePath")),
//                                RegistorNo = reader.GetString(reader.GetOrdinal("RegistorNo")),
//                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
//                                Description = reader.GetString(reader.GetOrdinal("Description")),
//                                Model = reader.GetString(reader.GetOrdinal("Model")),
//                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
//                            });
//                        }
//                        connection.Close();
//                    }
//                }
//            }
//            return bikes;
//        }

//        public async Task<Car> DeleteCar(Guid id)
//        {
//            Car car = null;
//            using (var connection = new SqlConnection(_connectionString))
//            {

//                var selectQuery = "SELECT * FROM Cars WHERE Id = @ID";
//                using (var selectCommand = new SqlCommand(selectQuery, connection))
//                {
//                    selectCommand.Parameters.AddWithValue("@ID", id);

//                    connection.Open();
//                    using (var reader = await selectCommand.ExecuteReaderAsync())
//                    {
//                        if (await reader.ReadAsync())
//                        {
//                            car = new Car
//                            {
//                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
//                                Title = reader.GetString(reader.GetOrdinal("Title")),
//                                CarImage = reader.GetString(reader.GetOrdinal("CarImage")),
//                                RegistorNo = reader.GetString(reader.GetOrdinal("RegistorNo")),
//                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
//                                Description = reader.GetString(reader.GetOrdinal("Description")),
//                                Model = reader.GetString(reader.GetOrdinal("Model")),
//                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
//                            };
//                        }
//                        connection.Close();
//                    }
//                }


//                var deleteQuery = "DELETE FROM Cars WHERE Id = @ID";
//                using (var deleteCommand = new SqlCommand(deleteQuery, connection))
//                {
//                    deleteCommand.Parameters.AddWithValue("@ID", id);

//                    connection.Open();
//                    await deleteCommand.ExecuteNonQueryAsync();
//                    connection.Close();
//                }
//            }
//            return car;
//        }

//        public async Task<Car> EditCar(Car car)
//        {
//            Car existingcar = null;
//            using (var connection = new SqlConnection(_connectionString))
//            {

//                var selectQuery = "SELECT * FROM Cars WHERE Id = @ID";
//                using (var selectCommand = new SqlCommand(selectQuery, connection))
//                {
//                    selectCommand.Parameters.AddWithValue("@ID", car.Id);

//                    connection.Open();
//                    using (var reader = await selectCommand.ExecuteReaderAsync())
//                    {
//                        if (await reader.ReadAsync())
//                        {
//                            existingcar = new Car
//                            {
//                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
//                                Title = reader.GetString(reader.GetOrdinal("Title")),
//                                CarImage = reader.GetString(reader.GetOrdinal("CarImage")),
//                                RegistorNo = reader.GetString(reader.GetOrdinal("RegistorNo")),
//                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
//                                Description = reader.GetString(reader.GetOrdinal("Description")),
//                                Model = reader.GetString(reader.GetOrdinal("Model")),
//                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
//                            };
//                        }
//                        connection.Close();
//                    }
//                }


//                if (existingcar != null)
//                {
//                    var updateQuery = "UPDATE Cars SET Title=@Title,CarImage=@CarImage,RegistorNo=@Regnumber, Brand = @Brand,Description=@Description,Model = @Model,IsAvailable=@IsAvailable WHERE ID = @ID";
//                    using (var updateCommand = new SqlCommand(updateQuery, connection))
//                    {

//                        updateCommand.Parameters.AddWithValue("@ID", car.Id);
//                        updateCommand.Parameters.AddWithValue("@Title", car.Title);
//                        updateCommand.Parameters.AddWithValue("@CarImage", car.CarImage);
//                        updateCommand.Parameters.AddWithValue("@Regnumber", car.RegistorNo);
//                        updateCommand.Parameters.AddWithValue("@Brand", car.Brand);
//                        updateCommand.Parameters.AddWithValue("@Description", car.Description);
//                        updateCommand.Parameters.AddWithValue("@Model", car.Model);
//                        updateCommand.Parameters.AddWithValue("@IsAvailable", car.IsAvailable);

//                        connection.Open();
//                        await updateCommand.ExecuteNonQueryAsync();
//                        connection.Close();
//                    }
//                }
//            }
//            return existingcar;
//        }



//    }
//}

using Carrental.Dtos.RequestDTO;
using Carrental.Dtos.ResponceDTO;
using Carrental.Dtos.ResponseDTO;
using Carrental.Entities;
using Carrental.Helpers;
using Carrental.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Carrental.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly string _connectionString;
        private readonly ImageUploadHelper _imageUploadHelper;

        public CarRepository(string connectionString, ImageUploadHelper imageUploadHelper)
        {
            _connectionString = connectionString;
            _imageUploadHelper = imageUploadHelper;
        }

        // Method to add a new car
        public async Task<Car> AddCarAsync(Car car, IFormFile imageFile)
        {
            // Use ImageUploadHelper to upload the image and get the filename
            var imageUrl = await _imageUploadHelper.UploadImageAsync(imageFile);
            if (imageUrl != null)
            {
                car.CarImage = imageUrl; // Set the image path to the car object
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                // Ensure the connection is opened before executing the command
                await connection.OpenAsync();

                var query = "INSERT INTO Cars (Id, Title, CarImage, RegistorNo,RentalPrice, Brand,Category, Description, Model, IsAvailable) " +
                            "VALUES (@Id, @Title, @ImagePath, @RegistorNo, @RentalPrice, @Brand,@Category, @Description, @Model, @IsAvailable)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", car.Id);
                    command.Parameters.AddWithValue("@Title", car.Title);
                    command.Parameters.AddWithValue("@ImagePath", car.CarImage);
                    command.Parameters.AddWithValue("@RegistorNo", car.RegistorNo);
                    command.Parameters.AddWithValue("@RentalPrice",car.RentalPrice);
                    command.Parameters.AddWithValue("@Brand", car.Brand);
                    command.Parameters.AddWithValue("@Description", car.Description);
                    command.Parameters.AddWithValue("@Category", car.Category);
                    command.Parameters.AddWithValue("@Model", car.Model);
                    command.Parameters.AddWithValue("@IsAvailable", car.IsAvailable);

                    // Execute the command asynchronously
                    await command.ExecuteNonQueryAsync();
                }

                // No need to explicitly close the connection; using statement takes care of it
            }

            return car; // Return the car object after adding it to the database
        }


        // Method to retrieve a car by its ID
        //public async Task<CarResponseDTO> GetCarByIdAsync(Guid carId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        var query = "SELECT * FROM Cars WHERE ID = @CarId";

        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@CarId", carId);
        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                if (await reader.ReadAsync())
        //                {
        //                    return new CarResponseDTO
        //                    {
        //                        ID = (Guid)reader["ID"],
        //                        Title = (string)reader["Title"],
        //                        RegistorNo = (string)reader["RegistorNo"],
        //                        Brand = (string)reader["Brand"],
        //                        Category = (string)reader["Category"],
        //                        Description = (string)reader["Description"],
        //                        Model = (string)reader["Model"],
        //                        ImageUrl = (string)reader["ImageUrl"],
        //                        RentalPrice = (string)reader["RentalPrice"], // Ensure this is correctly retrieved
        //                        IsAvailable = (bool)reader["IsAvailable"]
        //                    };
        //                }
        //            }
        //        }
        //    }
        //    return null; // Return null if car not found
        //}

        public async Task<CarResponseDTO> GetCarByIdAsync(Guid carId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Cars WHERE ID = @CarId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CarId", carId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new CarResponseDTO
                            {
                                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? Guid.Empty : (Guid)reader["ID"],
                                Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : (string)reader["Title"],
                                RegistorNo = reader.IsDBNull(reader.GetOrdinal("RegistorNo")) ? null : (string)reader["RegistorNo"],
                                Brand = reader.IsDBNull(reader.GetOrdinal("Brand")) ? null : (string)reader["Brand"],
                                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : (string)reader["Category"],
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : (string)reader["Description"],
                                Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : (string)reader["Model"],
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : (string)reader["ImageUrl"],
                                RentalPrice = reader.IsDBNull(reader.GetOrdinal("RentalPrice")) ? "0" : (string)reader["RentalPrice"], // Adjust based on actual type
                                IsAvailable = reader.IsDBNull(reader.GetOrdinal("IsAvailable")) ? false : (bool)reader["IsAvailable"]
                            };
                        }
                    }
                }
            }
            return null; // Return null if car not found
        }






        // Method to edit a car's details
        //public async Task<CarResponseDTO> EditCar(Guid carId, CarRequestDTO carRequest, string imageUrl)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        var query = "UPDATE Cars SET Title = @Title, RegistorNo = @RegistorNo, Brand = @Brand, Category = @Category, Description = @Description, Model = @Model, ImageUrl = @ImageUrl, RentalPrice = @RentalPrice WHERE ID = @CarId";

        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@Title", carRequest.Title);
        //            command.Parameters.AddWithValue("@RegistorNo", carRequest.RegistorNo);
        //            command.Parameters.AddWithValue("@Brand", carRequest.Brand);
        //            command.Parameters.AddWithValue("@Category", carRequest.Category);
        //            command.Parameters.AddWithValue("@Description", carRequest.Description);
        //            command.Parameters.AddWithValue("@Model", carRequest.Model);
        //            command.Parameters.AddWithValue("@ImageUrl", imageUrl);
        //            command.Parameters.AddWithValue("@RentalPrice", carRequest.RentalPrice); // Include RentalPrice
        //            command.Parameters.AddWithValue("@CarId", carId);

        //            await command.ExecuteNonQueryAsync();
        //            return await GetCarByIdAsync(carId);
        //        }
        //    }
        //}

        public async Task<CarResponseDTO> EditCar(Guid carId, CarRequestDTO carRequest, string imageUrl)
        {
            if (carRequest == null)
            {
                throw new ArgumentNullException(nameof(carRequest), "Car request cannot be null.");
            }

            // Validate that all required fields are not null
            if (string.IsNullOrWhiteSpace(carRequest.RegistorNo))
            {
                throw new ArgumentException("RegistorNo cannot be null or empty.", nameof(carRequest.RegistorNo));
            }
            if (string.IsNullOrWhiteSpace(carRequest.Brand))
            {
                throw new ArgumentException("Brand cannot be null or empty.", nameof(carRequest.Brand));
            }


            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "UPDATE Cars SET Title = @Title, " +
                            "RegistorNo = @RegistorNo, " +
                            "Brand = @Brand, " +
                            "Category = @Category, " +
                            "Description = @Description, " +
                            "Model = @Model, " +
                            "ImageUrl = @ImageUrl, " +
                            "RentalPrice = @RentalPrice " +
                            "WHERE ID = @CarId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", carRequest.Title ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@RegistorNo", carRequest.RegistorNo); // No DBNull.Value for non-null fields
                    command.Parameters.AddWithValue("@Brand", carRequest.Brand ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Category", carRequest.Category ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Description", carRequest.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Model", carRequest.Model ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ImageUrl", imageUrl ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@RentalPrice", carRequest.RentalPrice ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CarId", carId);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        // Log the error (use your logging framework)
                        throw new Exception("Database operation failed.", ex);
                    }
                }

                return await GetCarByIdAsync(carId);
            }
        }




        // Method to delete a car
        //public async Task<bool> DeleteCarAsync(Guid carId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        var query = "DELETE FROM Cars WHERE ID = @CarId";

        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@CarId", carId);
        //            var affectedRows = await command.ExecuteNonQueryAsync();
        //            return affectedRows > 0; // Return true if a car was deleted
        //        }
        //    }
        //}


        public async Task<string> DeleteCarAsync(Guid carId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "DELETE FROM Cars WHERE ID = @CarId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CarId", carId);
                    var affectedRows = await command.ExecuteNonQueryAsync();

                    // Return a message based on the outcome
                    if (affectedRows > 0)
                    {
                        return "Car deleted successfully.";
                    }
                    else
                    {
                        return "Car not found or could not be deleted.";
                    }
                }
            }
        }






        // Method to retrieve all cars
        //public async Task<List<CarResponseDTO>> GetAllCars()
        //{
        //    var cars = new List<CarResponseDTO>();
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        var query = "SELECT * FROM Cars";

        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    cars.Add(new CarResponseDTO
        //                    {
        //                        ID = (Guid)reader["ID"],
        //                        Title = (string)reader["Title"],
        //                        RegistorNo = (string)reader["RegistorNo"],
        //                        Brand = (string)reader["Brand"],
        //                        Category = (string)reader["Category"],
        //                        Description = (string)reader["Description"],
        //                        Model = (string)reader["Model"],
        //                        ImageUrl = (string)reader["ImageUrl"],
        //                        RentalPrice = (string)reader["RentalPrice"], // Ensure this is retrieved
        //                        IsAvailable = (bool)reader["IsAvailable"]
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    return cars;
        //}

        public async Task<List<CarResponseDTO>> GetAllCars()
        {
            var cars = new List<CarResponseDTO>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Cars";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            cars.Add(new CarResponseDTO
                            {
                                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? Guid.Empty : (Guid)reader["ID"], // Handle Guid
                                Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : (string)reader["Title"],
                                RegistorNo = reader.IsDBNull(reader.GetOrdinal("RegistorNo")) ? null : (string)reader["RegistorNo"],
                                Brand = reader.IsDBNull(reader.GetOrdinal("Brand")) ? null : (string)reader["Brand"],
                                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : (string)reader["Category"],
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : (string)reader["Description"],
                                Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : (string)reader["Model"],
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("CarImage")) ? null : (string)reader["CarImage"],
                                RentalPrice = reader.IsDBNull(reader.GetOrdinal("RentalPrice")) ? "0" : (string)reader["RentalPrice"], // Assuming it's a string, adjust as needed
                                IsAvailable = reader.IsDBNull(reader.GetOrdinal("IsAvailable")) ? false : (bool)reader["IsAvailable"]
                            });
                        }
                    }
                }
            }
            return cars;
        }





    }
}

