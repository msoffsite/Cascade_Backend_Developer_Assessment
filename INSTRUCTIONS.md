1. Create SQL Server database by running InitializeDatabase.sql.
2. Seed database by running SeedDatabase.sql.
3. Modify connection string in CascadeFinTech.API.appsettings.json.
4. Test data by running those tests found in CascadeFinTech.Tests.
    4a. Make sure to modify ConnectionString near top of class.
5. Test controller methods by debugging CascadeFinTech.API. API urls include:
    api/books/byauthor
    api/books/bypublisher
    api/books/totalprice (default)