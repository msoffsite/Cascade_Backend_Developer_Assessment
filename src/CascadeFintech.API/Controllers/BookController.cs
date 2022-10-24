using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using CascadeFinTech.Data;

namespace CascadeFinTech.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private string ConnectionString { get; }

        public BookController(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [Route("byauthor")]
        [HttpGet]
        public IEnumerable<BookDto> GetByAuthor()
        {
            var test = ConnectionString;
            return new List<BookDto>()
            .ToArray();
        }

        [Route("bypublisher")]
        [HttpGet]
        public IEnumerable<BookDto> GetByPublisher()
        {
            return new List<BookDto>()
            .ToArray();
        }
    }
}
