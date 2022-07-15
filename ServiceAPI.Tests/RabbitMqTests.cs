using Microsoft.Extensions.Configuration;
using Moq;
using System.Text.Json;
using Xunit;

namespace ServiceAPI.Tests;

public class RabbitMqTests
{
    [Fact]
    public void Send_And_Received_Will_Process()
    {
        var service = new Mock<IRabbitMqService>();
        var wrapper = new MessageBusWrapper(service.Object);

        var id = wrapper.Send(DataTest.PostMessageData());
        Thread.Sleep(100);
        var received = wrapper.Received(id);

        Assert.Equal("Processed", received);
    }

    [Fact]
    public void ProcessingMessage()
    {
        Parallel.Invoke(
            () => ProcessingMessage_One_Will_Return_Success(),
            () => ProcessingMessage_One_Will_Return_Success(),
            () => ProcessingMessage_One_Will_Return_Success());
    }

    public void ProcessingMessage_One_Will_Return_Success()
    {
        var service = new Mock<IRabbitMqService>();
        var wrapper = new MessageBusWrapper(service.Object);

        var id = wrapper.Send(DataTest.PostMessageData());
        Thread.Sleep(100);
        var received = wrapper.Received(id);

        Assert.Equal("Processed", received);
    }

    [Fact]
    public void ErrorProccessingMessage_Will_Return_Error()
    {
        var service = new Mock<IRabbitMqService>();
        var wrapper = new MessageBusWrapper(service.Object);
        var post = DataTest.ExpiredData();

        var id = wrapper.Send(post);
        Thread.Sleep(120);
        var exception = wrapper.ExceptionHandler(post, "Error");
        var received = wrapper.Received(id);

        Assert.Equal("Error", exception);
    }

    [Fact]
    public void SendMessage_Will_Return_Expired_Message()
    {
        var scheduleConfig = new ScheduleConfig<RabbitMqBackgroundService>
        {
            TimeZone = TimeZoneInfo.Local,
            CronExpression = @"*/1 * * * *"
        };

        var service = new RabbitMqBackgroundService(scheduleConfig);

        var res = service.CheckDataBase();

        Assert.Equal("Просрочено", res);
    }
}