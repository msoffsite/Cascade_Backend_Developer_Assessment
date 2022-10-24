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
using System.Linq;
using CascadeFinTech.Data.Attributes;
using System.Reflection;

namespace CascadeFinTech.Data
{
    public class BookDto : BaseGuid
    {
        [CitationMLA(Name = "Publisher.")]
        public string Publisher { get; }

        [CitationMLA(Name = "Title.", Description = "(in quotation marks)")]
        public string Title { get; }

        [CitationMLA(Name = "Author.", Description = "(last, first)")]
        public string Author { get; }

        [CitationMLA(Name = "Price.")]
        public decimal Price { get; }

        public string CitationsForMLA { get; }

        private BookDto() { }

        private BookDto(BookModel book, PublisherModel publisher, AuthorModel author, PriceModel price, string citationsForMLA)
        {
            Id = book.Id;
            Publisher = publisher.Name;
            Author = $"{author.LastName}, {author.FirstName}";
            Price = price.Value;
            Title = book.Title;
            CitationsForMLA = citationsForMLA;
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

        private static string GetCitationInfo()
        {
            string output = string.Empty;
            var properties = typeof(BookDto).GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var customAttribute = typeof(BookDto).GetProperty(propertyName).GetCustomAttribute<CitationMLA>();
                if (customAttribute != null)
                {
                    output += customAttribute.Name;
                    if (!string.IsNullOrEmpty(customAttribute.Description))
                    {
                        output += $" {customAttribute.Description}";
                    }
                    output += "|";
                }
            }
            return output.TrimEnd('|');
        }

        private static async Task<List<BookDto>> BuildDtoForEachAsync(string connectionString, List<BookModel> books)
        {
            var citationsForMLA = GetCitationInfo();

            var authorTable = new AuthorTable(connectionString);
            var priceTable = new PriceTable(connectionString);
            var publisherTable = new PublisherTable(connectionString);

            var output = new List<BookDto>();
            foreach (var book in books)
            {
                var publisher = await publisherTable.GetPublisherByIdAsync(book.PublisherId);
                var author = await authorTable.GetAuthorByIdAsync(book.AuthorId);
                var price = await priceTable.GetPriceByBookIdCurrencyAsync(book.Id, dbo.Price.Enumeration.Currency.USD);
                var outItem = new BookDto(book, publisher, author, price, citationsForMLA);
                output.Add(outItem);
            }
            return output;
        }

        private static async Task<List<BookDto>> BuildDtoAsync(string connectionString, List<BookModel> books)
        {
            var citationsForMLA = GetCitationInfo();

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
                    let outItem = new BookDto(book, publisher, author, price, citationsForMLA)
                    select outItem).ToList();
        }
    }
}
