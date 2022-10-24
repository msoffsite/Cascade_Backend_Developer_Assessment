using CascadeFinTech.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

using AuthorModel = CascadeFinTech.Data.dbo.Author.Model;
using BookModel = CascadeFinTech.Data.dbo.Book.Model;
using PublisherModel = CascadeFinTech.Data.dbo.Publisher.Model;
using PriceModel = CascadeFinTech.Data.dbo.Price.Model;

using AuthorTable = CascadeFinTech.Data.dbo.Author.Table;
using BookTable = CascadeFinTech.Data.dbo.Book.Table;
using PublisherTable = CascadeFinTech.Data.dbo.Publisher.Table;
using PriceTable = CascadeFinTech.Data.dbo.Price.Table;

namespace CascadeFinTech.Data
{
    public class BookDto : BaseGuid
    {
        public string Publisher { get; set; }
        public string Title { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public decimal Price { get; set; }

        private BookDto() { }

        private BookDto(BookModel book, PublisherModel publisher, AuthorModel author, PriceModel price)
        {
            Id = book.Id;
            Publisher = publisher.Name;
            AuthorFirstName = author.FirstName;
            AuthorLastName = author.LastName;
            Price = price.Value;
            Title = book.Title;
        }

        public static async Task<List<BookDto>> GetBooksSortedByAuthorLastFirstPublisherAsync(string connectionString)
        {
            BookTable bookTable = new BookTable(connectionString);
            var books = await bookTable.GetBooksSortedByAuthorLastFirstPublisherAsync();
            return await BuildDto(connectionString, books);
        }

        public static async Task<List<BookDto>> GetBooksSortedByPublisherAuthorLastFirst(string connectionString)
        {
            BookTable bookTable = new BookTable(connectionString);
            var books = await bookTable.GetBooksSortedByPublisherAuthorLastFirstAsync();
            return await BuildDto(connectionString, books);
        }

        private static async Task<List<BookDto>> BuildDto(string connectionString, List<BookModel> books)
        {
            var authorTable = new AuthorTable(connectionString);
            var priceTable = new PriceTable(connectionString);
            var publisherTable = new PublisherTable(connectionString);

            var output = new List<BookDto>();
            foreach (var book in books)
            {
                var publisher = await publisherTable.GetPublisherByIdAsync(book.PublisherId);
                var author = await authorTable.GetAuthorByIdAsync(book.AuthorId);
                var price = await priceTable.GetPriceByBookIdCurrencyAsync(book.Id, dbo.Price.Enumeration.Currency.USD);
                var outItem = new BookDto(book, publisher, author, price);
                output.Add(outItem);
            }
            return output;
        }
    }
}
