using System;
using FinancialChat.Application.Common.DTO;
using FinancialChat.Domain.Common;

namespace FinancialChat.Application.Common.Interfaces;

public interface IClientAppNotifier<T> where T : BaseDto 
{
    Task Notify(string eventName, T entity);
}

