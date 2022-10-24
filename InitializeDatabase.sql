USE [master]
GO

CREATE DATABASE [CascadeBooks]
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

ALTER DATABASE [CascadeBooks] SET COMPATIBILITY_LEVEL = 150
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled')) BEGIN
    EXEC [CascadeBooks].[dbo].[sp_fulltext_database] @action = 'enable'
END
GO

ALTER DATABASE [CascadeBooks] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CascadeBooks] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CascadeBooks] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CascadeBooks] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CascadeBooks] SET ARITHABORT OFF 
GO
ALTER DATABASE [CascadeBooks] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CascadeBooks] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CascadeBooks] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CascadeBooks] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CascadeBooks] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CascadeBooks] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CascadeBooks] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CascadeBooks] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CascadeBooks] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CascadeBooks] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CascadeBooks] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CascadeBooks] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CascadeBooks] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CascadeBooks] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CascadeBooks] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CascadeBooks] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CascadeBooks] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CascadeBooks] SET RECOVERY FULL 
GO
ALTER DATABASE [CascadeBooks] SET  MULTI_USER 
GO
ALTER DATABASE [CascadeBooks] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CascadeBooks] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CascadeBooks] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CascadeBooks] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CascadeBooks] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CascadeBooks] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'CascadeBooks', N'ON'
GO

ALTER DATABASE [CascadeBooks] SET QUERY_STORE = OFF
GO

USE [CascadeBooks]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Author](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](150) NOT NULL,
	[LastName] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_dbo_Author_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Book](
	[Id] [uniqueidentifier] NOT NULL,
	[AuthorId] [uniqueidentifier] NOT NULL,
	[PublisherId] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_dbo_Book_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Price](
	[Id] [uniqueidentifier] NOT NULL,
	[BookId] [uniqueidentifier] NOT NULL,
	[Currency] [nvarchar](25) NOT NULL,
	[Value] [decimal](19, 6) NOT NULL,
 CONSTRAINT [PK_dbo_Price] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Publisher](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_dbo_Publisher_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_dbo_Author_FirstName_LastName] ON [dbo].[Author]
(
	[FirstName] ASC,
	[LastName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_dbo_Book_AuthorId_Title] ON [dbo].[Book]
(
	[AuthorId] ASC,
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_dbo_Price_BookId_Currency] ON [dbo].[Price]
(
	[BookId] ASC,
	[Currency] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_dbo_Publisher_Name] ON [dbo].[Publisher]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Price] ADD  CONSTRAINT [DF_Price_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Book]  WITH CHECK ADD  CONSTRAINT [FK_dbo_Book_Author] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Author] ([Id])
GO

ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [FK_dbo_Book_Author]
GO

ALTER TABLE [dbo].[Book]  WITH CHECK ADD  CONSTRAINT [FK_dbo_Book_Publisher] FOREIGN KEY([PublisherId])
REFERENCES [dbo].[Publisher] ([Id])
GO

ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [FK_dbo_Book_Publisher]
GO

ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK_dbo_Price_Book] FOREIGN KEY([BookId])
REFERENCES [dbo].[Book] ([Id])
GO

ALTER TABLE [dbo].[Price] CHECK CONSTRAINT [FK_dbo_Price_Book]
GO

USE [master]
GO

ALTER DATABASE [CascadeBooks] SET  READ_WRITE 
GO

USE [CascadeBooks]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mark Stone
-- Create date: 10/24/2022
-- Description:	Get author by Id.
-- =============================================
CREATE PROCEDURE [dbo].[GetAuthorById]
(
    @Id     UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT authors.*
      FROM [dbo].[Author] authors
     WHERE (authors.Id = @Id)
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mark Stone
-- Create date: 10/23/2022
-- Description:	Get all authors.
-- =============================================
CREATE PROCEDURE [dbo].[GetAuthors]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT authors.*
      FROM [dbo].[Author] authors

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mark Stone
-- Create date: 10/23/2022
-- Description:	Get books.
-- =============================================
CREATE PROCEDURE [dbo].[GetBooks]
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT books.*
      FROM [dbo].[Book] books

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mark Stone
-- Create date: 10/23/2022
-- Description:	Get books sorted by author last 
-- and first name then publisher.
-- =============================================
CREATE PROCEDURE [dbo].[GetBooksSortedByAuthorLastFirstPublisher]
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT books.*
      FROM [dbo].[Book] books
      JOIN [dbo].[Author] authors
        ON (books.AuthorId = authors.Id)
      JOIN [dbo].[Publisher] publishers
        ON (books.PublisherId = publishers.Id)
     ORDER BY authors.LastName,
              authors.FirstName,
              publishers.[Name],
              books.Title

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mark Stone
-- Create date: 10/23/2022
-- Description:	Get books sorted by publisher
-- the author last and first name.
-- =============================================
CREATE PROCEDURE [dbo].[GetBooksSortedByPublisherAuthorLastFirst]
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT books.*
      FROM [dbo].[Book] books
      JOIN [dbo].[Author] authors
        ON (books.AuthorId = authors.Id)
      JOIN [dbo].[Publisher] publishers
        ON (books.PublisherId = publishers.Id)
     ORDER BY publishers.[Name],
              authors.LastName,
              authors.FirstName,
              books.[Title]
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mark Stone
-- Create date: 10/24/2022
-- Description:	Get price by book id 
-- and currency.
-- =============================================
CREATE PROCEDURE [dbo].[GetPriceByBookIdCurrency]
(
    @BookId     UNIQUEIDENTIFIER,
    @Currency   NVARCHAR(25)
)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT prices.*
      FROM [dbo].[Price] prices
     WHERE (prices.BookId = @BookId)
       AND (prices.Currency = @Currency)
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mark Stone
-- Create date: 10/23/2022
-- Description:	Get total price for all books
-- by currency.
-- =============================================
CREATE PROCEDURE [dbo].[GetPriceForAllBooksByCurrency]
(
    @Currency   NVARCHAR(25)
)	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT SUM(prices.[Value])
      FROM [dbo].[Price] prices
     WHERE (prices.Currency = @Currency)

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Price:		Mark Stone
-- Create date: 10/23/2022
-- Description:	Get all prices.
-- =============================================
CREATE PROCEDURE [dbo].[GetPrices]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT prices.*
      FROM [dbo].[Price] prices

END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Publisher:		Mark Stone
-- Create date: 10/24/2022
-- Description:	Get publisher by Id.
-- =============================================
CREATE PROCEDURE [dbo].[GetPublisherById]
(
    @Id     UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT publishers.*
      FROM [dbo].[Publisher] publishers
     WHERE (publishers.Id = @Id)
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Publisher:		Mark Stone
-- Create date: 10/23/2022
-- Description:	Get all publishers.
-- =============================================
CREATE PROCEDURE [dbo].[GetPublishers]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT publishers.*
      FROM [dbo].[Publisher] publishers

END
GO