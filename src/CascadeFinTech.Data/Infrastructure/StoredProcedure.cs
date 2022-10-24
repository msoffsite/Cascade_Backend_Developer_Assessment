namespace CascadeFinTech.Data.Infrastructure
{
    internal static class StoredProcedure
    {
        internal static string GetAuthorById => ProcName(
            "dbo",
            "GetAuthorById"
        );

        internal static string GetAuthors => ProcName(
            "dbo",
            "GetAuthors"
        );

        internal static string GetBooks => ProcName(
            "dbo",
            "GetBooks"
        );

        internal static string GetBooksSortedByAuthorLastFirstPublisher => ProcName(
            "dbo",
            "GetBooksSortedByAuthorLastFirstPublisher"
        );

        internal static string GetBooksSortedByPublisherAuthorLastFirst => ProcName(
            "dbo",
            "GetBooksSortedByPublisherAuthorLastFirst"
        );

        internal static string GetPriceByBookIdCurrency => ProcName(
            "dbo",
            "GetPriceByBookIdCurrency"
        );

        internal static string GetPriceForAllBooksByCurrency => ProcName(
            "dbo",
            "GetPriceForAllBooksByCurrency"
        );

        internal static string GetPrices => ProcName(
            "dbo",
            "GetPrices"
        );

        internal static string GetPublisherById => ProcName(
            "dbo",
            "GetPublisherById"
        );

        internal static string GetPublishers => ProcName(
            "dbo",
            "GetPublishers"
        );

        private static string ProcName(string schema, string action)
        {
            return $"[{schema}].[{action}]";
        }
    }
}
