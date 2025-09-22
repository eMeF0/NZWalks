using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Walks> CreateAsync(Walks walks)
        {
            await _dbContext.Walks.AddAsync(walks);
            await _dbContext.SaveChangesAsync();
            return walks;
        }

        public async Task<List<Walks>> GetAllAsync()
        {
            return await _dbContext.Walks
                .Include(x => x.Difficulty)
                .Include("Region")
                .ToListAsync();
        }

        public async Task<Walks?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Walks
                .Include(x => x.Difficulty)
                .Include(x => x.Region)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walks?> UpdateAsync(Guid id, Walks walks)
        {
            var existingWalks = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalks == null)
                return null;

            existingWalks.Name = walks.Name;
            existingWalks.Description = walks.Description;
            existingWalks.LengthInKm = walks.LengthInKm;
            existingWalks.WalkImageUrl = walks.WalkImageUrl;
            existingWalks.DifficultyId = walks.DifficultyId;
            existingWalks.RegionId = walks.RegionId;

            await _dbContext.SaveChangesAsync();

            return existingWalks;
        }

        public async Task<Walks?> DeleteAsync(Guid id)
        {
            var existingWalks = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalks == null)
            {
                return null;
            }

            _dbContext.Walks.Remove(existingWalks);
            await _dbContext.SaveChangesAsync();
            return existingWalks;
        }
    }
}
