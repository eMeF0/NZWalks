using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        //GET ALL REGIONS
        //GET: api/Regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from database - Doamin Models
            var regionsDomain = await _regionRepository.GetAllAsync();

            //Map Domain models to DTOs
            //var regionsDto = new List<RegionDto>();
            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl
            //    });
            //}

            //Return DTOs
            return Ok(_mapper.Map<List<RegionDto>>(regionsDomain));
        }

        //Get single region by ID
        //GET: api/Regions/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            //var regionById = _dbContext.Regions.Where(x => x.Id == id); // This returns a collection
            //var regionById = _dbContext.Regions.Find(id); // This returns a single object

            //Get region from database
            var regionDomain = await _regionRepository.GetByIdAsync(id); // This returns a single object
            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map Domain model to DTO
            //var regionDto = new RegionDto()
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl
            //};

            //Return DTO
            return Ok(_mapper.Map<RegionDto>(regionDomain));
        }

        //POST: Create a new region
        //POST: api/Regions
        [HttpPost]
        [ValidateModelAttribute]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
           
                //Map DTO to Domain model
                //var regionDomainModel = new Region
                //{
                //    Code = addRegionRequestDto.Code,
                //    Name = addRegionRequestDto.Name,
                //    RegionImageUrl = addRegionRequestDto.RegionImageUrl
                //};
                var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);

                //Use Domain model to create a new region
                regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

                //Map Domain model back to DTO
                //var regionDto = new RegionDto()
                //{
                //    Id = regionDomainModel.Id,
                //    Code = regionDomainModel.Code,
                //    Name = regionDomainModel.Name,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl
                //};
                var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);

        }

        //Update region
        //PUT: api/Regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModelAttribute]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
                //Map DTO to Domain model
                //var regionDomainModel = new Region
                //{
                //    Code = updateRegionRequestDto.Code,
                //    Name = updateRegionRequestDto.Name,
                //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl
                //};
                var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);

                //Check if region exists
                regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                //Convert Domain model to DTO
                //var regionDto = new RegionDto()
                //{
                //    Id = regionDomainModel.Id,
                //    Code = regionDomainModel.Code,
                //    Name = regionDomainModel.Name,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl
                //};
                return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }

        //Delete region
        //Delete: api/Regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await _regionRepository.DeleteAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //Convert Domain model to DTO
            //return deleted Region back
            //var regionDto = new RegionDto()
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }

    }
}
