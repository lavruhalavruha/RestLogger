# RestLogger


## About
RestLogger is an application solution based on REST to log incoming messages from 3rd party applications. The solution handles a simple log message as json and stores it in database.


## API Description

### Authentication

```
POST /auth
```

In RestLogger.WebApi project Bearer Token Authentication is used. To get token, client must send POST request to /auth endpoint.

**Request Header**

|Name|Value|
|----|-----|
|Content-Type|application/x-www-form-urlencoded|

**Request Body**

|Name|Value|
|----|-----|
|grant_type|password|
|username|*\[application display name]*|
|password|*\[application password]*|

**Response Body**

|Name|Value|
|----|-----|
|access_token|*\[token]*|
|token_type|bearer|
|expires_in|*\[expiration period]*|

### Registering Application

```
POST /register
```

Registering a new application within the Logger Service, unauthenticated end point.

**Request Header**

|Name|Value|
|----|-----|
|Content-Type|application/json|

**Request Body**

|Name|Value|
|----|-----|
|DisplayName|*\[application display name]*|
|Password|*\[application password]*|

**Response Body**

Id of registered application.

### Logging

```
POST /log
```

Sends a new application log message to be stored in the Logger Service, an access token is required.

**Request Header**

|Name|Value|
|----|-----|
|Content-Type|application/json|
|Authorization|Bearer *\[token]*|

**Request Body**

|Name|Value|
|----|-----|
|ApplicationId|*\[application id]*|
|Logger|*\[logger]*|
|Level|*\[log level]*|
|Message|*\[log message]*|

**Response Body**

|Name|Value|
|----|-----|
|success|true or false|


## Project Structure

Solution “RestLogger” consists of the following projects:
- RestLogger.Domain
- RestLogger.Infrastructure
- RestLogger.Service
- RestLogger.Storage
- RestLogger.WebApi
- RestLogger.Tests.UnitTests
- RestLogger.Tests.IntegrationTests

**RestLogger.Domain**  
This project contains domain entities which are stored to database through repositories.

**RestLogger.Infrastructure**  
This project contains infrastructrure which includes: Repository interfaces, Service interfaces, Generic Repository interface, Unit of Work interface. Also there are definitions of Data Transfer Objects (DTOs) which are used with services.

**RestLogger.Service**  
This project contains implementation of Services which perform business logic of application.

**RestLogger.Storage**  
This project contains implementation of Repositories and Unit of Work which are based on Entity Framework.

**RestLogger.WebApi**  
This project implements REST API. Here SSL is enabled and forced in authentication scheme and LoggerController. To work with SSL, you need to configure certificate on you machine. To disable SSL forcing, set "ForceSecuredConnection" setting to "false" in Web.config file.

**RestLogger.Tests.UnitTests**  
This project contains unit tests.

**RestLogger.Tests.IntegrationTests**  
This project contains integration tests.

## Patterns Used
Repository, Unit of Work, Dependency Injection, Factory, Data Transfer Object. Main purpose of using these patterns was to avoid tight coupling of application’s components.
