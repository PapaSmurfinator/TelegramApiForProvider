using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading.Tasks;
using TelegramApiForProvider.Controllers;
using TelegramApiForProvider.DbService;
using TelegramApiForProvider.Extended;
using TelegramApiForProvider.Models;
using TelegramApiForProvider.Service;
using Xunit;

namespace TelegramApiForProvider.Tests
{
    public class Class1
    {
        OrderParameter order = new OrderParameter
        {
            PhoneNumber = "8977545743543",
            Amount = 643,
            OrderNumber = "20"
        };
        OrderContext db;

        [Fact]
        public void IndexViewDataMessage()
        {
            // Arrange
            var mockTelegramBotService = new Mock<ITelegramBotService>();
            mockTelegramBotService.Setup(service => service.SendMessage(123, "Text"));

            var mockOrderContext = new Mock<OrderContext>();
            mockOrderContext.Setup(context => context.SaveChanges());
            var controller = new SendingController(mockOrderContext.Object, mockTelegramBotService.Object);
            //// Act
            ExtendedOrder result = null;// controller.ReceiveAndSend(order);

            //// Assert
            Assert.NotNull(result);
        }
    }
}
