namespace CascadeFinTech.Data.Infrastructure
{
    internal static class StoredProcedure
    {
        internal static string GetAuthorById => ProcName(
            "dbo",
            "BookModel",
            "GetAuthorById"
        );

        internal static string GetBooksSortedByAuthorLastFirstPublisher => ProcName(
            "dbo",
            "BookModel",
            "GetBooksSortedByAuthorLastFirstPublisher"
        );

        internal static string GetBooksSortedByPublisherAuthorLastFirst => ProcName(
            "dbo",
            "BookModel",
            "GetBooksSortedByPublisherAuthorLastFirst"
        );

        internal static string GetPublisherById => ProcName(
            "dbo",
            "BookModel",
            "GetPublisherById"
        );

        internal static string GetPriceByBookIdCurrency => ProcName(
            "dbo",
            "BookModel",
            "GetPriceByBookIdCurrency"
        );

        private static string ProcName(string schema, string table, string action)
        {
            return $"[{schema}].[{table}_{action}]";
        }
    }
}
