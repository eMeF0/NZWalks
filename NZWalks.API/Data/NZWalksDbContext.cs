using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        //To są polaczenia do tabel w bazie danych
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walks> Walks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seed data to Difficulties table
            //Easy,Medium,Hard
            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("4d1e4610-637c-4137-ad19-211851045a11"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("2d17839f-9667-43d9-a04c-44f53341afbf"),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("243ac72b-8bc8-4d6b-a2a7-50217cf943db"),
                    Name = "Hard"
                }

            };

            //Seed difficulties to the database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);


            //Seed data to Regions table
            var regions = new List<Region>()
            {
                new Region()
                {
                    Id = Guid.Parse("1e6f5f3e-3b2a-4c4c-8a8a-1a1a1a1a1a1a"),
                    Code = "AKL",
                    Name = "Auckland",
                    RegionImageUrl = "img/auckland.jpg"
                },
                new Region()
                {
                    Id = Guid.Parse("2b7e5f3e-4c3b-5d5c-9b9b-2b2b2b2b2b2b"),
                    Code = "WLG",
                    Name = "Wellington",
                    RegionImageUrl = null
                },
                new Region()
                {
                    Id = Guid.Parse("3c8f6f4e-5d4c-6e6d-0c0c-3c3c3c3c3c3c"),
                    Code = "CHC",
                    Name = "Christchurch",
                    RegionImageUrl = "img/christchurch.jpg"
                }
            };

            //Seed regions to the database
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
