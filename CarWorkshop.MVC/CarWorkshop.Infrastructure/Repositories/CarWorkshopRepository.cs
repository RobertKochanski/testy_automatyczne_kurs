using CarWorkshop.Domain.Interfaces;
using CarWorkshop.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshop.Infrastructure.Repositories
{
    internal class CarWorkshopRepository : ICarWorkshopRepository
    {
        private readonly CarWorkshopDbContext _dbContext;

        public CarWorkshopRepository(CarWorkshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Commit()
         => _dbContext.SaveChangesAsync();

        public async Task Create(Domain.Entities.CarWorkshopEntity carWorkshop)
        {
            _dbContext.Add(carWorkshop);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Domain.Entities.CarWorkshopEntity>> GetAll()
            => await _dbContext.CarWorkshops.ToListAsync();

        public async Task<Domain.Entities.CarWorkshopEntity> GetByEncodedName(string encodedName)
         => await _dbContext.CarWorkshops.FirstAsync(c => c.EncodedName == encodedName);

        public Task<Domain.Entities.CarWorkshopEntity?> GetByName(string name)
         => _dbContext.CarWorkshops.FirstOrDefaultAsync(cw => cw.Name.ToLower() == name.ToLower());
    }
}
