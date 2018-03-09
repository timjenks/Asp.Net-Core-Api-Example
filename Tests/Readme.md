# Tests

## Project structure
    .
    ├── ControllersTests
    │   ├── AccountControllerTest.cs
    │   ├── TodoControllerTest.cs
    │   └── UserControllerTest.cs
    ├── Helpers
    │   ├── EndSystems
    │   │   ├── MockResponse.cs
    │   │   ├── MockServerAndClient.cs
    │   │   └── StartUp.cs
    │   ├── Json
    │   │   ├── JsonStringBuilder.cs
    │   │   └── JsonStringDeserializer.cs
    │   └── Models
    │       └── ModelValidator.cs
    ├── IntegrationTests
    │   ├── AccountIntegrationTest.cs
    │   ├── TodoIntegrationTest.cs
    │   └── UserIntegrationTest.cs
    ├── MockData
    │   ├── Controllers
    │   │   └── MockClaims.cs
    │   ├── Data
    │   │   ├── InMemoryAppDataContext.cs
    │   │   ├── MockConfiguration.cs
    │   │   ├── MockNormalizer.cs
    │   │   ├── MockPasswordValidator.cs
    │   │   ├── MockSignInManager.cs
    │   │   ├── MockUserManager.cs
    │   │   └── MockUserValidator.cs
    │   ├── DtoModels
    │   │   ├── MockApplicationUserDto.cs
    │   │   └── MockTodoDto.cs
    │   ├── EntityModels
    │   │   ├── MockApplicationUsers.cs
    │   │   ├── MockRoles.cs
    │   │   ├── MockTodos.cs
    │   │   └── MockUserRoles.cs
    │   ├── Services
    │   │   ├── MockAccountService.cs
    │   │   ├── MockTodoService.cs
    │   │   └── MockUserService.cs
    │   └── ViewModels
    │       ├── MockCreateTodoViewModel.cs
    │       ├── MockEditTodoViewModel.cs
    │       ├── MockLoginViewModel.cs
    │       └── MockRegisterViewModel.cs
    ├── ModelsTests
    │   ├── DtoModelsTests
    │   │   ├── ApplicationUserDtoTest.cs
    │   │   └── TodoDtoTest.cs
    │   ├── EntityModelsTests
    │   │   ├── ApplicationUserTest.cs
    │   │   └── TodoTest.cs
    │   └── ViewModelsTests
    │       ├── CreateTodoViewModelTest.cs
    │       ├── EditTodoViewModelTest.cs
    │       ├── LoginViewModelTest.cs
    │       └── RegisterViewModelTest.cs
    ├── ServicesTests
    │   ├── AccountServiceTest.cs
    │   ├── TodoServiceTest.cs
    │   └── UserServiceTest.cs
    ├── UtilsTests
    │   ├── Constants
    │   │   ├── CacheConstantsTest.cs
    │   │   └── PasswordLimitsTest.cs
    │   └── TimeUtils
    │       └── QueryDateBuilderTest.cs
    └── Tests.csproj