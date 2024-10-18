using Carrental.Entities;

namespace Carrental.IRepositries
{
    public interface Icarrepository
    {
        Task<Car> Addcar(Car car);
        Task<Car> DeleteCar(Guid id);
        Task<Car> EditCar(Car car);
        Task<List<Car>> GetAllCars();
        Task<Car> GetCarById(Guid id);
    }
}
