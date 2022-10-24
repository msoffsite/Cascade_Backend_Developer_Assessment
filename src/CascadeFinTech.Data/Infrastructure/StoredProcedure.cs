namespace CascadeFinTech.Data.Infrastructure
{
    internal static class StoredProcedure
    {
        internal static string GetAuthorById => ProcName(
            "dbo",
            "GetAuthorById"
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

        internal static string GetPublisherById => ProcName(
            "dbo",
            "GetPublisherById"
        );

        internal static string GetPriceByBookIdCurrency => ProcName(
            "dbo",
            "GetPriceByBookIdCurrency"
        );

        internal static string GetPriceForAllBooksByCurrency => ProcName(
            "dbo",
            "GetPriceForAllBooksByCurrency"
        );

        private static string ProcName(string schema, string action)
        {
            return $"[{schema}].[{action}]";
        }
    }
}
