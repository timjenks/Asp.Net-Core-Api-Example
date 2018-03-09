# Api

## Project structure
    .
    ├── Controllers
    │   ├── AccountController.cs
    │   ├── TodoController.cs
    │   └── UserController.cs
    ├── Data
    │   ├── AppDataContext.cs
    │   └── Readme.md
    ├── Exceptions
    │   ├── LoginFailException.cs
    │   ├── PasswordModelException.cs
    │   ├── RegisterFailException.cs
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
    ├── Services
    │   ├── AccountService.cs
    │   ├── Interfaces
    │   │   ├── IAccountService.cs
    │   │   ├── ITodoService.cs
    │   │   └── IUserService.cs
    │   ├── TodoService.cs
    │   └── UserService.cs
    ├── Utils
    │   ├── Constants
    │   │   ├── CacheConstants.cs
    │   │   ├── ErrorMessages.cs
    │   │   ├── MethodNames.cs
    │   │   ├── PasswordLimits.cs
    │   │   ├── Roles.cs
    │   │   ├── Routes.cs
    │   │   ├── TodoLimits.cs
    │   │   └── UserLimits.cs
    │   └── TimeUtils
    │       └── QueryDateBuilder.cs
    ├── wwwroot
    ├── appsettings.Development.json
    ├── appsettings.json
    ├── Program.cs
    ├── Startup.cs
    ├── TodoApi.csproj
    └── TodoApi.csproj.user