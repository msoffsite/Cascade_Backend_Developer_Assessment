using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CascadeFinTech.Models;

namespace CascadeFinTech.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        [Route("byauthor")]
        [HttpGet]
        public IEnumerable<Book> GetByAuthor()
        {
            return new List<Book>()
            .ToArray();
        }

        [Route("bypublisher")]
        [HttpGet]
        public IEnumerable<Book> GetByPublisher()
        {
            return new List<Book>()
            .ToArray();
        }
    }
}
