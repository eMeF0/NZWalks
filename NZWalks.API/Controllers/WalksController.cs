using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }

        //Create WalksController
        //Post: api/walks
        [HttpPost]
        [ValidateModelAttribute]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
                //Map DTO to Domain model
                var walkDomainModel = _mapper.Map<Walks>(addWalkRequestDto);
                await _walkRepository.CreateAsync(walkDomainModel);

                //Map Domain model to DTO

                return Ok(_mapper.Map<WalksDto>(walkDomainModel));
        }

        //Get Walks
        //Get: api/walks?filterOn=Name&filterQuery=Track%sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            //Create an exception
            throw new Exception("This is a new exception");

            //Map Domain model to DTO
            return Ok(_mapper.Map<List<WalksDto>>(walksDomainModel));
        }

        //Get Walks by Id
        //Get: api/walks/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var walksDomain = await _walkRepository.GetByIdAsync(id);
            if(walksDomain == null)
                return NotFound();

            //Map Domain model to DTO
            return Ok(_mapper.Map<WalksDto>(walksDomain));
        }


        //Update Walks
        //Put: api/walks/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModelAttribute]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequest)
        {

                //Map DTO to Domain model
                var walkDomainModel = _mapper.Map<Walks>(updateWalkRequest);

                await _walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                    return NotFound();

                //Map Domain model to DTO
                return Ok(_mapper.Map<WalksDto>(walkDomainModel));
        }

        //Delete Walks by id 
        //Delete: api/walks/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.DeleteAsync(id);

            if(walkDomainModel == null)
                return NotFound();

            //Map Domain model to DTO
            return Ok(_mapper.Map<WalksDto>(walkDomainModel));
        }
    }
}
