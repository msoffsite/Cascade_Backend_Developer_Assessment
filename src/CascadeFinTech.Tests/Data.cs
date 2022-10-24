using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

using BookDto = CascadeFinTech.Data.BookDto;

namespace CascadeFinTech.Tests
{
    public class Data
    {
        private const string ConnectionString = "Server=OMNIAPRIMELAPTO\\SQL2019;Database=CascadeBooks;Trusted_Connection=True;MultipleActiveResultSets=true";

        [Fact]
        public async Task TestByAuthorLastFirstPublisher()
        {
            var sortedBooks = await BookDto.GetBooksSortedByAuthorLastFirstPublisherAsync(ConnectionString);
            var sortedBooksTitles = sortedBooks.Select(x => x.Title);
            var expectedOrder = await BookDto.GetBooksAsync(ConnectionString);
            expectedOrder = expectedOrder
                .OrderBy(x => x.Author)
                .ThenBy(x => x.Publisher)
                .ThenBy(x => x.Title).ToList();
            var expectedOrderTitles = expectedOrder.Select(x => x.Title);
            Assert.Equal(expectedOrderTitles, sortedBooksTitles);
        }

        [Fact]
        public async Task TestByPublisherAuthorLastFirst()
        {
            var sortedBooks = await BookDto.GetBooksSortedByPublisherAuthorLastFirstAsync(ConnectionString);
            var sortedBooksTitles = sortedBooks.Select(x => x.Title);
            var expectedOrder = await BookDto.GetBooksAsync(ConnectionString);
            expectedOrder = expectedOrder
                .OrderBy(x => x.Publisher)
                .ThenBy(x => x.Author)
                .ThenBy(x => x.Title).ToList();
            var expectedOrderTitles = expectedOrder.Select(x => x.Title);
            Assert.Equal(expectedOrderTitles, sortedBooksTitles);
        }

        [Fact]
        public async Task TestTotalPriceForAllBooks()
        {
            var totalPrice = await BookDto.GetTotalPriceForAllBooks(ConnectionString);
            Assert.True(totalPrice > 0);
        }
    }
}
