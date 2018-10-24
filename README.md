# Spike
Before we start, I'd like to let you know that I have two repositories, [Azure DevOps](https://embeprojects.visualstudio.com/PublicShowProject) and [Github](https://github.com/MartinBialkowski/Spike). Github repository is secondary repository, because most of developers like to use Github. I'm working daily on Azure DevOps repository which also is public for you to see. If you would like to check newest code please go to: [Azure DevOps public repository](https://embeprojects.visualstudio.com/PublicShowProject). I'll always post my code on Github, but I can't do it that often, because I'm having problems with forking repository from Azure DevOps to Github. When I'll find solution for it I'll start updating imidiatelly.

## Description
Please see the notes below to see me excuse myself :)

This is learning project. It uses Onion architecture. Data is stored inside MS SQL database. I'm using Entity Framework Core to access database. User can access data thru REST API. Project is solely for learning purpose, functionality is simple so I can focus on learning technologies, instead of algorithm.

Second project `Spike.AuthenticationServer.IdentityServer` is authentication provider using Identity Server 4. User need to register and login here to access rest of application. It uses the same database as `Spike.WebApi` with Identity Core (similar names, but Identity Server 4 != Identity Core).

Project is covered with tests, it's only Spike, so I've created only tests that would introduce something new (normally you should cover everything that is important). There are tests for Controllers, Authorization Handlers, Repository, Mappings, Validators etc. Only purpose to introduce tests for everything was researching, if those are easy to write and maintain for company work. It's possible that I would drop some of them at my work depending on the situation.

## Structure
- Spike.Core contains domain model/entities and migrations.
- Spike.Infrastructure contains repositories and extension methods for EF Core.
- Spike.WebApi contains controllers, mappings, autofac modules and authorization handlers.
- Spike.WebApi.Types contains DTOs and validators for Spike.WebApi

- Spike.AuthenticationServer.IdentityServer is authorization service for Spike project. User can register, login, reset his password. After registration service will send email with confirmation at address you specified during registration. When you click on link you'll be able to confirm your email.
- Spike.Backend.Connect is responsible for sending emails, it uses SendGrid client.

## Run
To run project you need to have MS SQL Server installed, connection string should work just fine if you have default options. Additionally you need to have [.Net Core SDK version 2.1+](https://www.microsoft.com/net/download/dotnet-core/2.1) It is possible that you would need to change `Spike.WebApi/appsettings.json` file and change AutofacConfig -> RepositoryConfig to suit your path. (I've discovered it recently during implementing support for Docker and will change it, after I finish work in my main repository)

In near future I'm going to introduce Docker support which will let you run project with only Docker installed on your machine. Additionally I'm going to split project to separated solution so it will be easier to read. I'll inform about it on [my blog](https://www.progressdesire.com) and will create series of posts about it.

## Using
Api is protected with JWT, user first need to register to `Spike.AuthenticationServer.IdentityServer` using endpoint POST `account/register`. Please see swagger documentation for more information. After that login to application, what will return auth token which will be used to access protected `Spike.WebApi`. Database will be seed with tests data, it uses custom solution for that in `SpikeDbInitializer` class.

## Technologies & Libraries used:
- ASP.Net Core WebAPI
- Entity Framework Core
- Fluent Validation
- AutoMapper
- XUnit
- FluentAssertions
- Moq
- Identity Core
- Serilog
- Autofac Core 
- Send Grid
- Identity Server 4

## Notes
- I highly recommend to read code from visual studio, because when I started working on project I didn't have naming convention. Later I fixed namespaces and projects, but fixing folder names wasn't my highest priority. I decided to fix it later in "rewrite" part.
- I was refactoring code on the fly, but after I learned everything I know now, it became clear that project need a lot of refactor in order to become easy to read and learn from. That's why I'll create series of posts about process of rewriting it. This project helped me countless times at my work. That's why I think rewriting will to pay off.
- I'm using Entity Framework and implement Repository Pattern. First of all, I know that DbContext is already Repository, I did it on purpose. I wanted to create Specification for filtering, paging and sorting data. Using repositories was my best idea to achieve it.
- Docker support isn't finished yet. Currently it's impossible to run project with Docker. Right now fully operation is solution for upgrading database container from another container. Because of problems that I found (code need a lot of refactoring) I was unable to fully support docker. Fear not thou, I'll cover everything, so sooner or later everything will work just fine :)
