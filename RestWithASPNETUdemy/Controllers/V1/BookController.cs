using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNETUdemy.Hypermedia.Filters;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Services;
using RestWithASPNETUdemy.Services.Implementations;

namespace RestWithASPNETUdemy.Controllers.V1
{
    [ApiController]
    [ApiVersion("1")]
    [Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public sealed class BookController : ControllerBase
    {
        #region Fields
        private readonly ILogger<BookController> _logger;
        private readonly IBookService _bookService;
        #endregion

        #region Ctor
        public BookController(ILogger<BookController> logger, IBookService bookService) 
        {
            _logger = logger;
            _bookService = bookService;
        }
        #endregion

        [HttpGet("{sortDirection}/{pageSize}/{page}")]
        [TypeFilter(typeof(HyperMediaFiler))]
        public IActionResult Get(
            [FromQuery] string? name,
            string sortDirection,
            int pageSize,
            int page)
        {
            try
            {
                return Ok(_bookService.FindWithPagedSearch(name = string.Empty, sortDirection, pageSize, page));
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
                var person = _bookService.FindById(id);

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
        public IActionResult Post([FromBody] BookVO book)
        {
            try
            {
                if (book == null)
                {
                    return BadRequest();
                }

                return Ok(_bookService.Create(book));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return BadRequest("Invalid input");
        }

        [HttpPut]
        [TypeFilter(typeof(HyperMediaFiler))]
        public IActionResult Put([FromBody] BookVO book)
        {
            try
            {
                if (book == null)
                {
                    return BadRequest();
                }

                return Ok(_bookService.Update(book));
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
                var book = _bookService.FindById(id);

                if (book == null)
                {
                    return NotFound();
                }

                _bookService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return BadRequest("Invalid input");
        }

    }
}
