using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map DTO to Domain model
            var walkDomainModel = _mapper.Map<Walks>(addWalkRequestDto);
            await _walkRepository.CreateAsync(walkDomainModel);
            
            //Map Domain model to DTO

            return Ok(_mapper.Map<WalksDto>(walkDomainModel));
        }

        //Get Walks
        //Get: api/walks
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var walksDomainModel = await _walkRepository.GetAllAsync();

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
