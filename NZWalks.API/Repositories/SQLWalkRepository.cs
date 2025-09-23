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

        public async Task<List<Walks>> GetAllAsync(string? filterOn = null, string? filterQuery = null, 
            string? sortBy = null, bool isAscending = true, 
            int pageNumber = 1, int pageSize = 1000)
        {
            var walks = _dbContext.Walks.Include(x => x.Difficulty)
                .Include(x => x.Region)
                .AsQueryable();

            //Filter
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
            }

            //Sort
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
            //return await _dbContext.Walks
            //  .Include(x => x.Difficulty)
            //  .Include("Region")
            //  .ToListAsync();
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
