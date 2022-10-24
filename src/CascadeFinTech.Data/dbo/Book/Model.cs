using System;

namespace CascadeFinTech.Data.dbo.Book
{
    internal class Model : BaseGuid
    {
        internal Guid AuthorId { get; set; }
        internal Guid PublisherId { get; set; }
        internal string Title { get; set; }
    }
}
