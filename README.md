# Hiring Test Project

The test consist in a very simple CRUD App that
manages "TestEntity" objects. Being simple, is not justification
to do it the wrong way, so I made a whole infrastructure around
the API so it is easy to extend and modify. The project consist
of the following modules:

### Domain

Here lives the business models. Inside there exists an Entity
folder which serves as namespace for business entities. These are
the objects that get mapped to the Database using EntityFramework.
There is also a Models folder which owns the system inputs and outputs
(this could be called ViewModels, Commands, Dtos, or whatever).
The mapping between Entities and Models is handled by AutoMapper
and is configured in the `AddDomainMappings()` extension method
for ease of use.

### DataAccess

This namespace holds the Repository Pattern and Unit of Work pattern definition
and implementation. The idea behind Repositories is to abstract data stores
so it is easy to exchange them, use multiple of them, or mock them for testing,
without disrupting the business logic associated with data acquisition. For
this, I leverage EntityFramework and provide a Generic repository to avoid
boilerplate code from which Concrete Repositories can inherit to provide
specific data access patterns.

Unit of Work pattern is the way of achieving atomicity over a set of 
business actions (usually insert/delete/update). Most Database Systems
support the concept of Transactions and Unit of Work leverages this to
provide a higher level of cohesion on business requirements. Also, Unit of Work
serves as an entrypoint for Repositories instances, and there is a couple
of ways of implementing it. I choose to leverage the SaveChanges of 
DbContexts to join repositories together on the same transaction.

### Services

This is the Application Service Layer, where the business logic lives.
Also, I provide here an unified interface for the notification system,
and two different implementations (one to be used by the server and the other
to be used by the client Hub). The beauty of this is that Dependency Injection
allows to exchange implementations and still hold the same API, which means
that the client sending events to a listening signalR connection would feel
the same as the server pushing messages to Azure Service Bus, and both are
interchangeable.

### HiringTest

The glue of our application back-end to conform the service API. It is
automatically documented with swagger and provides the Controllers (The view part
in MVC) as entrypoints to the server. Controllers are in charge of data
manipulation and validation to feed the Service Layer and to change 
the presentation of the data (using the `Domain.Models`). Also, when
necessary, Controllers calls notification system to push events to listeners.

### HiringTest.Test

Simple demo Unit Testing for the HiringTest project. This covers the basics
of testing, it Mocks some data, Mocks the Services using DepedencyInjection
and checks the correct response from Controller and the invocation
of the notification system as a requirement.

### TestClientBlazor

This is the Client for the App. It uses Blazor Server as the hosting model.
It consist of a welcome Page, with a side menu with navigation in it. The main
view is the Test Entities link which provide a Table that display the current
entities in the App. Any changes to this collection (entity updates, additions or
removals) are tracked by this client and displayed in real-time on the table.
Also, a Toast (notification) with a warning color (Light Orange) will pop-up
when an entity is modified outside the client or with green color when a entity
is modified inside the client.

<p align="middle">
    <img src="img/blazor-app.png" alt="blazor-app" width="45%">
    <img src="img/api.png" alt="api" width="45%">
</p>

When we create a entity using the API this is what happens:

<p align="middle">
    <img src="img/blazor-warning.png" alt="warning-blazor" width="45%">
    <img src="img/api-warning.png" alt="api-warning" width="45%">
</p>

This is accomplish using an `IHostedService` for listening to the Azure Broker
and the forwarding the events through a SignalR Hub that the TestEntities razor
page is connecting and listening to.

## How to run it

Is quite simple to run the app, just clone the repo:

```console
$ git clone https://github.com/adriangs1996/DotNET6FullExample.git
```

Then access the project folder and install the depedencies and build the project:

```console
$ dotnet restore HiringTest
$ dotnet restore TestClientBlazor
```

and then run this in two terminals in parallel:

```console
$ # Run the client
$ dotnet watch run -p TestClientBlazor
```

```console
$ # Run the API
$ dotnet watch run -p HiringTest
```

Alternatively, I provide a docker-compose.yml so you can run the project
using docker. Make sure you have installed docker compose plugin and run:

```console
$ docker compose up -d

[+] Running 3/3
 ⠿ Network hiringtest_default         Created                                             0.7s
 ⠿ Container hiringtest-blazor-app-1  Created                                             2.9s
 ⠿ Container hiringtest-net6-api-1    Created                                             2.9s
Attaching to hiringtest-blazor-app-1, hiringtest-net6-api-1
hiringtest-blazor-app-1  | warn: Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository[60]
hiringtest-blazor-app-1  |       Storing keys in a directory '/root/.aspnet/DataProtection-Keys' that may not be persisted outside of the container. Protected data will be unavailable when container is destroyed.
hiringtest-net6-api-1    | info: Microsoft.Hosting.Lifetime[14]
hiringtest-net6-api-1    |       Now listening on: http://[::]:8000
hiringtest-net6-api-1    | info: Microsoft.Hosting.Lifetime[0]
hiringtest-net6-api-1    |       Application started. Press Ctrl+C to shut down.
hiringtest-net6-api-1    | info: Microsoft.Hosting.Lifetime[0]
hiringtest-net6-api-1    |       Hosting environment: Development
hiringtest-net6-api-1    | info: Microsoft.Hosting.Lifetime[0]
hiringtest-net6-api-1    |       Content root path: /app/
hiringtest-blazor-app-1  | warn: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[35]
hiringtest-blazor-app-1  |       No XML encryptor configured. Key {706c4340-4d84-46f7-acc7-d9759c3e0747} may be persisted to storage in unencrypted form.
hiringtest-blazor-app-1  | Starting Azure Subscriber
hiringtest-blazor-app-1  | Connected
hiringtest-blazor-app-1  | info: Microsoft.Hosting.Lifetime[14]
hiringtest-blazor-app-1  |       Now listening on: http://0.0.0.0:5010
hiringtest-blazor-app-1  | info: Microsoft.Hosting.Lifetime[0]
hiringtest-blazor-app-1  |       Application started. Press Ctrl+C to shut down.
hiringtest-blazor-app-1  | info: Microsoft.Hosting.Lifetime[0]
hiringtest-blazor-app-1  |       Hosting environment: Development
hiringtest-blazor-app-1  | info: Microsoft.Hosting.Lifetime[0]
hiringtest-blazor-app-1  |       Content root path: /app/
```

That's it, the app is now being served at 
<a href="http://localhost:8000">http://localhost:8000</a>
and the API is being served at 
<a href="http://localhost:5000"> http://localhost:5000 </a>