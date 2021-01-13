# SqlQueryBuilder
SqlQueryBuilder is .NET Core Class Library written in C#.

It helps to build SQL query string in C# for SQL Server. It supports lambda to construct query based on class's property name.


## Example
### Sample Class
```csharp
public class Person
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Address { get; set; }
}
```

### Select All Query
```csharp
string query = SqlQueryBulder.BuildSelectAllQuery<Person>();
```

Output:
```sql
SELECT [Id],[Name],[Email],[Address] FROM [Person] WHERE 1=1
```

### Select Query - Specific Property
```csharp
string query = SqlQueryBulder.BuildSelectQuery<Person>(x => new { x.Name, x.Email } );
```

Output:
```sql
SELECT [Name],[Email] FROM [Person] WHERE 1=1
```

### Select Query - Specific Property with parameters
```csharp
string query = SqlQueryBulder.BuildSelectQuery<Person>(
                    x => new { x.Name, x.Email}, 
                    x => new { x.Id } );
```

Output:
```sql
SELECT [Name],[Email] FROM [Person] WHERE 1=1  AND [Id]=@Id
```

### Insert Query
```csharp
string query = SqlQueryBulder.BuildInsertQuery<Person>();
```

Output:
```sql
DECLARE @t TABLE([Id] UNIQUEIDENTIFIER); INSERT INTO [Person] ([Id],[Name],[Email],[Address]) OUTPUT INSERTED.[Id] INTO @t VALUES (@Id,@Name,@Email,@Address); SELECT [Id] FROM @t
```

For insert query it returns the Id of the inserted record.

### Update Query
```csharp
string query = SqlQueryBulder.BuildUpdateQuery<Person>();
```

Output:
```sql
UPDATE [Person] SET [Id]=@Id,[Name]=@Name,[Email]=@Email,[Address]=@Address WHERE 1=1
```

### Update Query - Specific Property
```csharp
string query = SqlQueryBulder.BuildUpdateQuery<Person>(x => new { x.Email });
```

Output:
```sql
UPDATE [Person] SET [Email]=@Email WHERE 1=1
```

## Additional condition and order
You can always add more condition or order to the query since it is just a string.

Example:
```csharp
string query = SqlQueryBulder.BuildSelectQuery<Person>(x => new { x.Name, x.Email } );
query += " AND [Email]=@Email ";
query += " ORDER BY [Name] "
```
Output:
```sql
SELECT [Name],[Email] FROM [Person] WHERE 1=1 AND [Email]=@Email ORDER BY [Name]
```

> Note: The purpose of 'WHERE 1=1' is to provide the flexiblity to append conditions to the query without worrying when to insert the 'WHERE' keyword.