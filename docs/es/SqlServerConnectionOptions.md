# SqlServerConnectionOptions

Clase para poder configurar la conexión a base de datos

### Constructores

|                                                                                                       |                                                                                                                                                                |
| ----------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| SqlServerConnectionOptions(string connectionString)                                                   | Inicializar una instancia con la cadena de conexion que se pasa como parametro                                                                                 |
| SqlServerConnectionOptions(string connectionString, DatabaseManagementEvents events)                  | Inicializar una instancia con la cadena de conexion que se pasa como parametro y una clase derivada de [DatabaseManagementEvents](DatabaseManagementEvents.md) |
| SqlServerConnectionOptions(IFormats formats, SqlServerDatabaseManagement sqlServerDatabaseManagement) | Inicializar una instancia con el formato a utlizar y una intancia de [SqlServerDatabaseManagement](SqlServerDatabaseManagement.md)                             |

### Propiedades

|                    |                                               |
| ------------------ | --------------------------------------------- |
| Format             | Da el formato al nombre de la columna y tabla |
| DatabaseManagement | Administrador de la conexión a base de datos  |

```csharp
using GSqlQuery.SQLServer;

SqlServerConnectionOptions sqlServerConnectionOptions = new SqlServerConnectionOptions("<connectionString>");

SqlServerConnectionOptions sqlServerConnectionOptions = new SqlServerConnectionOptions("<connectionString>", new SqlServerDatabaseManagement());

SqlServerConnectionOptions sqlServerConnectionOptions = new SqlServerConnectionOptions(new SqlServerFormats(), new SqlServerDatabaseManagement("<connectionString>"));

```
