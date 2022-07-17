
namespace Application.IntegrationTests.ChatRooms.Queries.GetChatRooms;

using FinancialChat.Application.ChatMessages.Commands.SendChatMessages;
using FinancialChat.Application.ChatRooms.Queries.GetChatRooms;
using FinancialChat.Domain.Entities;
using FluentAssertions;
using static Testing;

public class GetChatRoomTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnChatRooms()
    {
        await RunAsDefaultUserAsync();

        var query = new GetChatRoomsQuery();

        var result = await SendAsync(query);

        result.ChatRooms.Should().NotBeNullOrEmpty().And.HaveCount(1);
    }

    [Test]
    public async Task ShouldReturnMessages()
    {
        await RunAsDefaultUserAsync();
        var quantity = new Random().Next(1, ChatRoom.MESSAGES_LIMIT);
        for (int i = 0; i < quantity; i++)
        {
            await SendAsync(new SendChatMessageCommand
            {
                ChatRoomCode = ChatRoom.DEFAULT_CODE,
                Message = $"Message {i + 1}"
            });
        }

        var query = new GetChatRoomsQuery();
        var result = await SendAsync(query);
        result.ChatRooms.Should().NotBeNullOrEmpty().And.HaveCountGreaterThanOrEqualTo(1);

        var messages = result.ChatRooms.First().Messages;
        messages.Should().NotBeNull().And.HaveCount(quantity);
    }

    [Test]
    public async Task ShouldReturnLimitedOfMessages()
    {
        await RunAsDefaultUserAsync();

      
        var aboveLimit = ChatRoom.MESSAGES_LIMIT + 10;
        for (int i = 0; i < aboveLimit; i++)
        {
            await SendAsync(new SendChatMessageCommand
            {
                ChatRoomCode = ChatRoom.DEFAULT_CODE,
                Message = $"Message {i + 1}"
            });
        }

        var query = new GetChatRoomsQuery();
        var result = await SendAsync(query);
        result.ChatRooms.Should().NotBeNullOrEmpty().And.HaveCountGreaterThanOrEqualTo(1);

        var messages = result.ChatRooms.First().Messages;
        messages.Should().NotBeNull().And.HaveCount(ChatRoom.MESSAGES_LIMIT);
    }
}

