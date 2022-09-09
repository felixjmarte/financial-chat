# FinanChat
[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://github.com/felixjmarte/financial-chat/tree/main)

FinanChat is a chat room with an integrated stocks commands processor built with .NET and Angular 13.

You can go straight to testing, read [installation](#installation) section to setup your environment.

## Features

- Allow users registration with .NET Identity
- Allow registered users to log in and talk with other users in a chatroom.
- Allow users to post messages as commands into the chatroom with the following format
/stock=stock_code *(ex. /stock=aapl.us)*
- Has a commands processor bot, that will response your stock commands with a quote like this: "[stock-code] quote is [open-price] per share.".


***

## Installation
FinanChat requires .NET 6, Angular +13, and RabbitMQ Server (***docker version recommended***) to run and works properly.

#### Instructions

Clone this repository and open a new terminal windows at root folder.

##### Run rabbitmq instance on docker
docker run -d --hostname financhat-rabbit --name financhat-rabbit -p 15672:15672 -p 5672:5672 -e RABBITMQ_DEFAULT_USER=financhat -e RABBITMQ_DEFAULT_PASS=financhat rabbitmq:3-management

##### Run FinanChat WebUI and Worker Applications simultaneously
dotnet restore

dotnet run --project .\src\Worker\Worker.csproj

dotnet run --project .\src\WebUI\WebUI.csproj

##### Open two browser windows and log in with these users

Go to https://localhost:5001 or http://localhost:5000/, then log in into these accounts.

Username: user1@financhat.com	Password: Administrator#2022!

Username: user2@financhat.com	Password: Administrator#2022!

### Start sending messages and commands (ex. /stock=aapl.us)

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
- SignalR
- EF Core
- RabbitMQ
- MediatR
- NUnit
- FluentAssertions

***

