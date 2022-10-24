using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using CascadeFinTech.Data;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<BookDto>> GetByAuthor()
        {
            var output = await BookDto.GetBooksSortedByAuthorLastFirstPublisherAsync(ConnectionString);
            return output;
        }

        [Route("bypublisher")]
        [HttpGet]
        public async Task<IEnumerable<BookDto>> GetByPublisher()
        {
            var output = await BookDto.GetBooksSortedByPublisherAuthorLastFirstAsync(ConnectionString);
            return output;
        }
    }
}
