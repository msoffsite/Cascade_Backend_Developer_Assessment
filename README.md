# Cascade_Backend_Developer_Assessment
NOTE: All work for this position is done using .NET, C#, and SQL.

# Overview
This exercise is intended to take no longer than 8 hours.  Please limit the detail of your solution with that time in mind.  Please include a README with your submission detailing your solution.

## Data definition

```
public class Book
{
	public string Publisher { get; set; }
	public string Title { get; set; }
	public string AuthorLastName { get;set }
	public string AuthorFirstName { get; set; }
	public decimal Price {get;set;}
}
```

## Problem
1.	Create a REST API using ASP.NET MVC and write a method to return a sorted list of these by Publisher, Author (last, first), then title.

2.	Write another API method to return a sorted list by Author (last, first) then title.

3.	If you had to create one or more tables to store the Book data in a MS SQL database, outline the table design along with fields and their datatypes. 

4.	Write stored procedures in MS SQL Server for steps 1 and 2, and use them in separate API methods to return the same results.

5.	Write an API method to return the total price of all books in the database.

6.	If you have a large list of these in memory and want to save the entire list to the MS SQL Server database, what is the most efficient way to save the list with only one call to the DB server?

7.	Add a property to the Book class that outputs the MLA (Modern Language Association) style citation as a string (https://images.app.goo.gl/YkFgbSGiPmie9GgWA). Please add whatever additional properties the Book class needs to generate the citation.

8.	Add another property to generate a Chicago style citation (Chicago Manual of Style) (https://images.app.goo.gl/w3SRpg2ZFsXewdAj7).

___

## Submission
- Send an email to your Cascade contact with a link to your solution on your github account when completed.

Do not submit a PR. 
Do not ask for external assistance. 
Do not share solution or assessment with outside sources.
Do not reuse previously written code.