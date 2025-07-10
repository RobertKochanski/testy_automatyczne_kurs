namespace CarWorkshop.Domain.Interfaces
{
    public interface ICarWorkshopRepository
    {
        Task Create(Domain.Entities.CarWorkshopEntity carWorkshop);
        Task<Domain.Entities.CarWorkshopEntity?> GetByName(string name);
        Task<IEnumerable<Domain.Entities.CarWorkshopEntity>> GetAll();
        Task<Domain.Entities.CarWorkshopEntity> GetByEncodedName(string encodedName);
        Task Commit();
    }
}
