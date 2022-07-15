using System;
namespace FinancialChat.Application.MessageBroker;

public interface IMessageProducer
{
    void SendMessage<T>(T message);
}

