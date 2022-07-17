using FinancialChat.Domain.Common;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.UnitTests.Entities;

public class BaseEntityTest
{
    [Test]
    public void ShouldAddEvents()
    {
        BaseEntity entity = new TestEntity();
        entity.DomainEvents.Should().BeNullOrEmpty();

        BaseEvent event1 = new TestEvent();
        entity.AddDomainEvent(event1);
        entity.DomainEvents.Should().NotBeNullOrEmpty().And.HaveCount(1);
    }

    [Test]
    public void ShouldRemoveEvents()
    {
        BaseEntity entity = new TestEntity();
        entity.DomainEvents.Should().BeEmpty();

        BaseEvent event1 = new TestEvent();
        entity.AddDomainEvent(event1);
        entity.RemoveDomainEvent(new TestEvent());
        entity.DomainEvents.Should().NotBeNullOrEmpty().And.HaveCount(1);

        entity.RemoveDomainEvent(event1);
        entity.DomainEvents.Should().BeNullOrEmpty();
    }


    [Test]
    public void ShouldClearEvents()
    {
        BaseEntity entity = new TestEntity();
        entity.DomainEvents.Should().BeEmpty();

        int min = 3, max = 10;
        TestContext.WriteLine($"Generate x events between: {min} - {max}");
        var eventsCount = new Random().Next(min, max);
        TestContext.WriteLine($"Random quantity: {eventsCount}");

        for (int i = 0; i < eventsCount; i++)
        {
            entity.AddDomainEvent(new TestEvent());
        }
        entity.DomainEvents.Should().NotBeNullOrEmpty()
            .And.HaveCountGreaterThanOrEqualTo(min)
            .And.HaveCountLessThanOrEqualTo(max);

        entity.ClearDomainEvents();
        entity.DomainEvents.Should().BeNullOrEmpty();
    }
}

