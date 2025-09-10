using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET ALL REGIONS
        //GET: api/Regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //Get data from database - Doamin Models
            var regionsDomain = _dbContext.Regions.ToList();

            //Map Domain models to DTOs
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }
            //Return DTOs
            return Ok(regionsDto);
        }

        //Get single region by ID
        //GET: api/Regions/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetById([FromRoute]Guid id)
        {
            //var regionById = _dbContext.Regions.Where(x => x.Id == id); // This returns a collection
            //var regionById = _dbContext.Regions.Find(id); // This returns a single object

            //Get region from database
            var regionDomain = _dbContext.Regions.FirstOrDefault(x => x.Id == id); // This returns a single object
            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map Domain model to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            //Return DTO
            return Ok(regionDto);
        }

        //POST: Create a new region
        //POST: api/Regions
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map DTO to Domain model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            //Use Domain model to create a new region
            _dbContext.Regions.Add(regionDomainModel);
            _dbContext.SaveChanges();

            //Map Domain model back to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
        }

        //Update region
        //PUT: api/Regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Check if region exists
            var regionDomainModel = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Map DTO to Domain model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            _dbContext.SaveChanges();

            //Convert Domain model to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        //Delete region
        //Delete: api/Regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //Delete region
            _dbContext.Regions.Remove(regionDomainModel);
            _dbContext.SaveChanges();

            //Convert Domain model to DTO
            //return deleted Region back
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

    }
}
