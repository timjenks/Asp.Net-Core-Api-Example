# Api

## Project structure
    .
    ├── appsettings.Development.json
    ├── appsettings.json
    ├── Constants
    │   ├── CacheConstants.cs
    │   ├── ErrorMessages.cs
    │   ├── MethodNames.cs
    │   ├── PasswordLimits.cs
    │   ├── Roles.cs
    │   ├── Routes.cs
    │   ├── TodoLimits.cs
    │   └── UserLimits.cs
    ├── Controllers
    │   ├── AccountController.cs
    │   ├── TodoController.cs
    │   └── UserController.cs
    ├── Data
    │   ├── AppDataContext.cs
    │   ├── Migrations
    │   │   ├── 20180227172424_InitialCreate.cs
    │   │   ├── 20180227172424_InitialCreate.Designer.cs
    │   │   └── AppDataContextModelSnapshot.cs
    │   └── Readme.md
    ├── Exceptions
    │   ├── LoginFailException.cs
    │   ├── RegisterFailException.cs
    │   ├── RemoveUserFailedException.cs
    │   ├── TodoNotFoundException.cs
    │   └── UserNotFoundException.cs
    ├── Models
    │   ├── DtoModels
    │   │   ├── ApplicationUserDto.cs
    │   │   └── TodoDto.cs
    │   ├── EntityModels
    │   │   ├── ApplicationUser.cs
    │   │   └── Todo.cs
    │   └── ViewModels
    │       ├── CreateTodoViewModel.cs
    │       ├── EditTodoViewModel.cs
    │       ├── LoginViewModel.cs
    │       └── RegisterViewModel.cs
    ├── Program.cs
    ├── Properties
    │   └── launchSettings.json
    ├── Services
    │   ├── AccountService.cs
    │   ├── Interfaces
    │   │   ├── IAccountService.cs
    │   │   ├── ITodoService.cs
    │   │   └── IUserService.cs
    │   ├── TodoService.cs
    │   └── UserService.cs
    ├── Startup.cs
    ├── TodoApi.csproj
    ├── TodoApi.csproj.user
    ├── Utils
    │   └── TimeUtils
    │       └── QueryDateBuilder.cs
    └── wwwroot
