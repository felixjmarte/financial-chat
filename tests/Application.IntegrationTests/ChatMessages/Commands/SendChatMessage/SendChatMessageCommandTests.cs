using FinancialChat.Application.ChatMessages.Commands.SendChatMessages;
using FinancialChat.Application.Common.Exceptions;
using FinancialChat.Domain.Entities;
using FluentAssertions;

namespace Application.IntegrationTests.ChatMessages.Commands.SendChatMessage;

using static Testing;

public class SendChatMessageCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldCreateChatMessage()
    {
        await RunAsDefaultUserAsync();

        var sendCommand = new SendChatMessageCommand()
        {
            ChatRoomCode = ChatRoom.DEFAULT_CODE,
            Message = "TEST MESSAGE"
        };
        
        var result = await SendAsync(sendCommand);

        var message = await FindAsync<ChatMessage>(result);

        Assert.NotNull(message);
        Assert.IsTrue(message?.Message == sendCommand.Message);
    }


    [Test]
    public async Task ShouldRequireMessageText()
    {
        await RunAsDefaultUserAsync();

        SendChatMessageCommand sendCommand = null!;

        sendCommand = new SendChatMessageCommand()
        {
            ChatRoomCode = ChatRoom.DEFAULT_CODE,
            Message = string.Empty
        };
        await FluentActions.Invoking(() =>
            SendAsync(sendCommand)).Should().ThrowAsync<ValidationException>();

        sendCommand = new SendChatMessageCommand()
        {
            ChatRoomCode = ChatRoom.DEFAULT_CODE,
            Message = null
        };
        await FluentActions.Invoking(() =>
            SendAsync(sendCommand)).Should().ThrowAsync<ValidationException>();

        sendCommand = new SendChatMessageCommand()
        {
            ChatRoomCode = ChatRoom.DEFAULT_CODE,
            Message = "test"
        };
        await FluentActions.Invoking(() =>
            SendAsync(sendCommand)).Should().NotThrowAsync();
    }

    [Test]
    public async Task ShouldRequireRoomCode()
    {
        await RunAsDefaultUserAsync();

        SendChatMessageCommand sendCommand = null!;

        sendCommand = new SendChatMessageCommand()
        {
            ChatRoomCode = null,
            Message = "message"
        };
        await FluentActions.Invoking(() =>
            SendAsync(sendCommand)).Should().ThrowAsync<ValidationException>();

        sendCommand = new SendChatMessageCommand()
        {
            ChatRoomCode = string.Empty,
            Message = "message"
        };
        await FluentActions.Invoking(() =>
            SendAsync(sendCommand)).Should().ThrowAsync<ValidationException>();

        sendCommand = new SendChatMessageCommand()
        {
            ChatRoomCode = ChatRoom.DEFAULT_CODE,
            Message = "test"
        };
        await FluentActions.Invoking(() =>
            SendAsync(sendCommand)).Should().NotThrowAsync();
    }
}

