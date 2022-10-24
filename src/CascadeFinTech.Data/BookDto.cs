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
using System.ComponentModel;
using System.Linq;

namespace CascadeFinTech.Data
{
    public class BookDto : BaseGuid
    {
        public string Publisher { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }

        private BookDto() { }

        private BookDto(BookModel book, PublisherModel publisher, AuthorModel author, PriceModel price)
        {
            Id = book.Id;
            Publisher = publisher.Name;
            Author = $"{author.LastName}, {author.FirstName}";
            Price = price.Value;
            Title = book.Title;
        }

        public static async Task<List<BookDto>> GetBooksAsync(string connectionString)
        {
            BookTable bookTable = new BookTable(connectionString);
            var books = await bookTable.GetBooksAsync();
            return await BuildDtoAsync(connectionString, books);
        }

        public static async Task<List<BookDto>> GetBooksSortedByAuthorLastFirstPublisherAsync(string connectionString)
        {
            BookTable bookTable = new BookTable(connectionString);
            var books = await bookTable.GetBooksSortedByAuthorLastFirstPublisherAsync();
            return await BuildDtoAsync(connectionString, books);
        }

        public static async Task<List<BookDto>> GetBooksSortedByPublisherAuthorLastFirstAsync(string connectionString)
        {
            BookTable bookTable = new BookTable(connectionString);
            var books = await bookTable.GetBooksSortedByPublisherAuthorLastFirstAsync();
            return await BuildDtoAsync(connectionString, books);
        }

        public static async Task<decimal> GetTotalPriceForAllBooks(string connectionString)
        {
            var priceTable = new PriceTable(connectionString);
            var output = await priceTable.GetPriceForAllBooksByCurrencyAsync(dbo.Price.Enumeration.Currency.USD);
            return output;
        }

        private static async Task<List<BookDto>> BuildDtoForEachAsync(string connectionString, List<BookModel> books)
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

        private static async Task<List<BookDto>> BuildDtoAsync(string connectionString, List<BookModel> books)
        {
            var authorTable = new AuthorTable(connectionString);
            var authors = await authorTable.GetAuthorsAsync();

            var priceTable = new PriceTable(connectionString);
            var prices = await priceTable.GetPricesAsync();

            var publisherTable = new PublisherTable(connectionString);
            var publishers = await publisherTable.GetPublishersAsync();

            return (from book in books
                    let publisher = publishers.FirstOrDefault(x => x.Id == book.PublisherId)
                    let author = authors.FirstOrDefault(x => x.Id == book.AuthorId)
                    let price = prices.FirstOrDefault(x => x.BookId == book.Id && x.Currency == dbo.Price.Enumeration.Currency.USD)
                    let outItem = new BookDto(book, publisher, author, price)
                    select outItem).ToList();
        }
    }
}
