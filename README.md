# FinanChat
[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://github.com/felixjmarte/financial-chat/tree/main)

FinanChat is a chat room with an integrated stocks commands processor built with .NET and Angular 13.
***
## Purpose
With this project, I aim to show up my technology skills, coding style and architecture design.

Concepts:
- Clean Architecture
- Unit Testing
- CQRS

Technologies:
- .NET 6
- Angular 13
- EF Core
- RabbitMQ
- MediatR
- NUnit
- FluentAssertions

***
## Features

- Allow users registration with .NET Identity
- Allow registered users to log in and talk with other users in a chatroom.
- Allow users to post messages as commands into the chatroom with the following format
/stock=stock_code
- Has a commands processor bot, that will response your stock commands with a quote like this: "[stock-code] quote is [open-price] per share.".


***
## Installation
FinanChat requires .NET 6, Angular +13, and RabbitMQ Server (***docker version recommended***) to run and works properly.

##### Tips
- Set ("UseInMemoryDatabase": true) if you prefer to database server configuration.
- Set visual studio to run WebUI and Worker together.
- Verify your configurations files before running the project.
- Verify your rabbitmq server availability before running the project.

##### Configuration files
 Before running projects, you should set up your environment configurations.

**WebUI**:
"src/WebUI/appsettings.Production.json" serve as a template for final configuration file, you should copy its content into "src/WebUI/appsettings.Development.json" and replace all [fields] with you environment values.

**Worker**:
"src/Worker/appsettings.Production.json" serve as a template for final configuration file, you should copy its content into "src/Worker/appsettings.Development.json" and replace all [fields] with you environment values.

**BOT**:
At startup, a default bot user is created, you should use these credentials:
"Bot": { "UserName": "bot@localhost", "Password": "Bot#2022!" } 

**IntegrationTests**:
"test/Application.IntegrationTests/appsettings.tests.json" serve as a template for final configuration file, you should replace all [fields] with you environment values.
