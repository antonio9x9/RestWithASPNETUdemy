using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Hypermedia.Filters;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Services.Implementations;

namespace RestWithASPNETUdemy.Controllers.V1
{
    [ApiController]
    [ApiVersion("1")]
    [Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public sealed class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonService _personService;

        public PersonController(ILogger<PersonController> logger, IPersonService personService)
        {
            _logger = logger;
            _personService = personService;
        }

        [HttpGet("{sortDirection}/{pageSize}/{page}")]
        [TypeFilter(typeof(HyperMediaFiler))]
        public IActionResult Get(
            [FromQuery] string name,
            string sortDirection,
            int pageSize,
            int page)
        {
            try
            {
                return Ok(_personService.FindWithPagedSearch(name, sortDirection, pageSize, page));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return BadRequest("Invalid input");
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(HyperMediaFiler))]
        public IActionResult Get(int id)
        {
            try
            {
                var person = _personService.FindById(id);

                if (person == null)
                {
                    return NotFound();
                }

                return Ok(person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return BadRequest("Invalid input");
        }

        [HttpGet("findPersonByName")]
        [TypeFilter(typeof(HyperMediaFiler))]
        public IActionResult FindPersonByName([FromQuery] string firstName, [FromQuery] string lastName)
        {
            try
            {
                var person = _personService.FindByName(firstName, lastName);

                if (person == null)
                {
                    return NotFound();
                }

                return Ok(person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return BadRequest("Invalid input");
        }

        [HttpPost]
        [TypeFilter(typeof(HyperMediaFiler))]
        public IActionResult Post([FromBody] PersonVO person)
        {
            try
            {
                if (person == null)
                {
                    return BadRequest();
                }

                return Ok(_personService.Create(person));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return BadRequest("Invalid input");
        }

        [HttpPut]
        [TypeFilter(typeof(HyperMediaFiler))]
        public IActionResult Put([FromBody] PersonVO person)
        {
            try
            {
                if (person == null)
                {
                    return BadRequest();
                }

                return Ok(_personService.Update(person));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return BadRequest("Invalid input");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var person = _personService.FindById(id);

                if (person == null)
                {
                    return NotFound();
                }

                _personService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return BadRequest("Invalid input");
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(long id)
        {
            _personService.Disable(id);
            return Ok();
        }

    }
}
