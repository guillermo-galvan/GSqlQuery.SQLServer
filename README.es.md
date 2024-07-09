# GSqlQuery.SQLServer [![en](https://img.shields.io/badge/lang-en-red.svg)](./README.md) [![NuGet](https://img.shields.io/nuget/v/GSqlQuery.SQLServer.svg)](https://www.nuget.org/packages/GSqlQuery.SQLServer)

Una biblioteca para ejecutar las consultas generadas por [GSqlQuery](https://github.com/guillermo-galvan/GSqlQuery) en la base de datos SQL Server para .NET

## Empezar

GSqlQuery.SQLServer se puede instalar utilizando el administrador de [paquetes Nuget](https://www.nuget.org/packages/GSqlQuery.SQLServer) o la `dotnet` CLI

```shell
dotnet add package GSqlQuery.SQLServer --version 1.0.0-beta
```

[Revise nuestra documentación](./docs/es/Config.md) para obtener instrucciones sobre cómo utilizar el paquete.

## Example

```csharp
using GSqlQuery.SQLServer;

SqlServerConnectionOptions connectionOptions = new SqlServerConnectionOptions("<connectionString>");

IEnumerable<Actor> rows = EntityExecute<Actor>.Select(connectionOptions).Build().Execute();
```

## Contributors

GSqlQuery.SQLServer es mantenido activamente por [Guillermo Galván](https://github.com/guillermo-galvan). Las contribuciones son bienvenidas y se pueden enviar mediante pull request.

## Licencia

Copyright (c) Guillermo Galván. All rights reserved.

Licensed under the [Apache-2.0 license](./LICENSE).
