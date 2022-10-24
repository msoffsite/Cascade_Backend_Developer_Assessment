1. Create SQL Server database by running InitializeDatabase.sql.
2. Seed database by running SeedDatabase.sql.
3. Modify connection string in CascadeFinTech.API.appsettings.json.
4. Test data by running those tests found in CascadeFinTech.Tests.
5. Make sure to modify ConnectionString near top of class.
6. Test controller methods by debugging CascadeFinTech.API. API urls include:
    api/books/byauthor
    api/books/bypublisher
    api/books/totalprice (default)

Note: I added a property for MLA citations via a custom attribute that then gets read via reflection. I lacked the time to do so for the Chicago style citations, though had I the time I would have approached 
it via an additional custom attribute.
