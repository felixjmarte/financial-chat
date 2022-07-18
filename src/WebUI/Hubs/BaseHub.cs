using System;
using System.Security.Claims;
using FinancialChat.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.WebUI.Hubs
{
    public abstract class BaseHub : Hub
    {
        private ISender _mediator = null!;

        protected ISender Mediator => _mediator ??= Context.GetHttpContext()!.RequestServices.GetRequiredService<ISender>();
    }
}

