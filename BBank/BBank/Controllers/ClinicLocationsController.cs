using BBank.Model;
using BBank.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BBank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicLocationsController : ControllerBase
    {
        private readonly IClinicLocationRepository _repository;

        public ClinicLocationsController(IClinicLocationRepository clinicLocationRepository)
        {
            _repository = clinicLocationRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClinicLocation>>> Get()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClinicLocation>> Get(int id)
        {
            var clinicLocation = await _repository.GetByIdAsync(id);
            if (clinicLocation == null)
                return NotFound();
            return clinicLocation;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ClinicLocation clinicLocation)
        {
            clinicLocation.UserAdded = true;
            await _repository.AddAsync(clinicLocation);
            return CreatedAtAction(nameof(Get), new { id = clinicLocation.Id }, clinicLocation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClinicLocation clinicLocation)
        {
            if (id != clinicLocation.Id)
                return BadRequest();

            await _repository.UpdateAsync(clinicLocation);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var loc = await _repository.GetByIdAsync(id);
            if (loc.UserAdded)
            {
                await _repository.DeleteAsync(id);
            }
            else
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
