# SqlServerConnectionOptions

Class to configure the connection to the database.

### Constructors

|                                                                                                       |                                                                                                                                                          |
| ----------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------- |
| SqlServerConnectionOptions(string connectionString)                                                   | Initialize an instance with the connection string passed as a parameter                                                                                  |
| SqlServerConnectionOptions(string connectionString, DatabaseManagementEvents events)                  | Initialize an instance with the connection string passed as a parameter and a class derived from [DatabaseManagementEvents](DatabaseManagementEvents.md) |
| SqlServerConnectionOptions(IFormats formats, SqlServerDatabaseManagement sqlServerDatabaseManagement) | Initialize an instance with the format to use and an instance of [SqlServerDatabaseManagement](SqlServerDatabaseManagement.md)                           |

### Properties

|                    |                                   |
| ------------------ | --------------------------------- |
| Format             | Formats the column and table name |
| DatabaseManagement | Database Connection Manager       |

```csharp
using GSqlQuery.SQLServer;

SqlServerConnectionOptions sqlServerConnectionOptions = new SqlServerConnectionOptions("<connectionString>");

SqlServerConnectionOptions sqlServerConnectionOptions = new SqlServerConnectionOptions("<connectionString>", new SqlServerDatabaseManagement());

SqlServerConnectionOptions sqlServerConnectionOptions = new SqlServerConnectionOptions(new SqlServerFormats(), new SqlServerDatabaseManagement("<connectionString>"));

```
