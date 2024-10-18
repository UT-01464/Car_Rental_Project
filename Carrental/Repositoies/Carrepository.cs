using Carrental.Entities;
using Carrental.IRepositries;
using System.Data.SqlClient;

namespace Carrental.Repositoies
{
    public class Carrepository:Icarrepository
    {
        private readonly string _connectionString;

        public Carrepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Car> Addcar(Car car)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Car (Id,Title,CarImage,RegistorNo,Brand,Description,Model,IsAvailable) VALUES (@ID,@Title,@ImagePath,@Regnumber,@Brand,@Description,@Model,@IsAvailable)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", car.Id);
                    command.Parameters.AddWithValue("@Title", car.Title);
                    command.Parameters.AddWithValue("@ImagePath", car.CarImage);
                    command.Parameters.AddWithValue("@Regnumber", car.RegistorNo);
                    command.Parameters.AddWithValue("@Brand", car.Brand);
                    command.Parameters.AddWithValue("@Description", car.Description);
                    command.Parameters.AddWithValue("@Model", car.Model);
                    command.Parameters.AddWithValue("@IsAvailable", car.IsAvailable);

                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    connection.Close();
                }
            }
            return car;
        }

        public async Task<Car> GetCarById(Guid id)
        {
            Car car = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Car WHERE ID = @ID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            car = new Car
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("ID")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                CarImage = reader.GetString(reader.GetOrdinal("CarImage")),
                                RegistorNo = reader.GetInt32(reader.GetOrdinal("RegistorNo")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Model = reader.GetString(reader.GetOrdinal("Model")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),


                            };
                        }
                        connection.Close();
                    }
                }
            }
            return car;
        }

        public async Task<List<Car>> GetAllCars()
        {
            var bikes = new List<Car>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Car";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            bikes.Add(new Car
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                CarImage = reader.GetString(reader.GetOrdinal("ImagePath")),
                                RegistorNo = reader.GetInt32(reader.GetOrdinal("RegistorNo")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Model = reader.GetString(reader.GetOrdinal("Model")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                            });
                        }
                        connection.Close();
                    }
                }
            }
            return bikes;
        }

        public async Task<Car> DeleteCar(Guid id)
        {
            Car car = null;
            using (var connection = new SqlConnection(_connectionString))
            {

                var selectQuery = "SELECT * FROM Car WHERE Id = @ID";
                using (var selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    using (var reader = await selectCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            car = new Car
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                CarImage = reader.GetString(reader.GetOrdinal("CarImage")),
                                RegistorNo = reader.GetInt32(reader.GetOrdinal("RegistorNo")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Model = reader.GetString(reader.GetOrdinal("Model")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                            };
                        }
                        connection.Close();
                    }
                }


                var deleteQuery = "DELETE FROM Car WHERE Id = @ID";
                using (var deleteCommand = new SqlCommand(deleteQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    await deleteCommand.ExecuteNonQueryAsync();
                    connection.Close();
                }
            }
            return car;
        }

        public async Task<Car> EditCar(Car car)
        {
            Car existingcar = null;
            using (var connection = new SqlConnection(_connectionString))
            {

                var selectQuery = "SELECT * FROM Car WHERE Id = @ID";
                using (var selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@ID", car.Id);

                    connection.Open();
                    using (var reader = await selectCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            existingcar = new Car
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                CarImage = reader.GetString(reader.GetOrdinal("CarImage")),
                                RegistorNo = reader.GetInt32(reader.GetOrdinal("RegistorNo")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Model = reader.GetString(reader.GetOrdinal("Model")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                            };
                        }
                        connection.Close();
                    }
                }

           
                if (existingcar != null)
                {
                    var updateQuery = "UPDATE Car SET Title=@Title,CarImage=@CarImage,RegistorNo=@Regnumber, Brand = @Brand,Description=@Description,Model = @Model,IsAvailable=@IsAvailable WHERE ID = @ID";
                    using (var updateCommand = new SqlCommand(updateQuery, connection))
                    {

                        updateCommand.Parameters.AddWithValue("@ID", car.Id);
                        updateCommand.Parameters.AddWithValue("@Title", car.Title);
                        updateCommand.Parameters.AddWithValue("@CarImage", car.CarImage);
                        updateCommand.Parameters.AddWithValue("@Regnumber", car.RegistorNo);
                        updateCommand.Parameters.AddWithValue("@Brand", car.Brand);
                        updateCommand.Parameters.AddWithValue("@Description", car.Description);
                        updateCommand.Parameters.AddWithValue("@Model", car.Model);
                        updateCommand.Parameters.AddWithValue("@IsAvailable", car.IsAvailable);

                        connection.Open();
                        await updateCommand.ExecuteNonQueryAsync();
                        connection.Close();
                    }
                }
            }
            return existingcar;
        }



    }
}
