# Todo Api

A simple example restful api project in dotnet core.

## Migrations and database
From root folder:
```sh
Add-Migration <name> -o Data/Migrations
Update-Database
```

## Features
- Authentication with bearer tokens
- Role based authentication decorators
- Memory caching
- Todo, User, Role and UserRoles tables.
- MS SQL server database
- Swagger
- Dependency injection
- 100% coverage in all layers
- 100% coverage in integration test

## Doucmentation
There is a static auto generated Swagger documentation [here](https://jonsteinn.github.io/Asp.Net-Core-Api-Example/) and the path `/swagger` also contains a non-static one.

## Tests
Test type | Count
--- | ---
Utils | 10
Models | 44
Service | 33
Controller | 27
Integration | 54
Total | 168