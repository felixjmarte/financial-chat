using System;
namespace FinancialChat.Application.Common.Interfaces;

public interface IMessageBrokerProducer
{
    void Publish<T>(T message);
}

