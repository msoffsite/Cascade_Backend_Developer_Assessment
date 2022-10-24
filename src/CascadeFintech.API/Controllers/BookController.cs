using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CascadeFinTech.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
