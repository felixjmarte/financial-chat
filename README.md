# FinanChat
## _The best stocks discussion chat room, Ever_

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://github.com/felixjmarte/financial-chat/tree/main)

FinanChat is a global chat room, built with dotNET and AngularJS powered.

With this project, I aim to show up my technology skills, concept knowledge, programming style and architecture design.

- CQRS
- Clean Architecture
- .NET 6
- EF Core
- Mediatr
- Angular +13
- RabbitMQ

## Features

- Allow users registration
- Allow registered users to log in and talk with other users in a chatroom.
- Allow users to post messages as commands into the chatroom with the following format
/stock=stock_code
- Has a commands processor bot, that will response your stock commands with a quote like this: "[stock-code] quote is [open-price] per share.".

## Tech

FinanChat uses a number of open source projects to work properly:

- [Angular] - HTML enhanced for web apps!

And of course FinanChat itself is open source with a [public repository](https://github.com/felixjmarte/financial-chat)
 on GitHub.

## Installation

FinanChat requires .NET 6, Angular +13, and RabbitMQ Server to run and works properly.

## License

MIT

**Free Software**

[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)
   [Angular]: <http://angularjs.org>
