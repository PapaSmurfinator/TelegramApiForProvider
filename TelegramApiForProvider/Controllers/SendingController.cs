using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramApiForProvider.DbService;
using TelegramApiForProvider.Extended;
using TelegramApiForProvider.Models;
using TelegramApiForProvider.Service;

namespace TelegramApiForProvider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendingController : ControllerBase
    {
        private readonly OrderContext db;

        private readonly ITelegramBotService _telegramBotService;

        public SendingController(OrderContext context, ITelegramBotService telegramBotService)
        {
            db = context;
            _telegramBotService = telegramBotService;
        }

        Message sentMessage = null;
        ExtendedOrder extendedOrder;
        OrderParameter _orderParameter;
        

        [HttpPost]
        public async Task ReceiveAndSend(OrderParameter orderParameter)
        {
            _orderParameter = new OrderParameter
            {
                Id = orderParameter.Id,
                CustomerName = orderParameter.CustomerName,
                PartnerName = orderParameter.PartnerName,
                PartnerId = orderParameter.PartnerId,
                Amount = orderParameter.Amount,
                DeliveryCost = orderParameter.DeliveryCost,
                Discount = orderParameter.Discount,
                PhoneNumber = orderParameter.PhoneNumber,
                OrderNumber = orderParameter.OrderNumber,
                Comment = orderParameter.Comment,
                CreateDatetime = orderParameter.CreateDatetime,
                DeliveryAddress = orderParameter.DeliveryAddress,
                DeliverAtTime = orderParameter.DeliverAtTime,
                OrderContent = orderParameter.OrderContent,
                CutleryQuantity = orderParameter.CutleryQuantity,
                HasAdminPanel = orderParameter.HasAdminPanel,
                TotalAmount = orderParameter.TotalAmount,
                PaymentMethod = orderParameter.PaymentMethod,
                DeliveryLocation = orderParameter.DeliveryLocation,
                DeliveryType = orderParameter.DeliveryType,
                Products = orderParameter.Products
            };
            extendedOrder = new ExtendedOrder
            {
                Id = orderParameter.Id,
                OrderNumber = orderParameter.OrderNumber,
                PartnerName = orderParameter.PartnerName,
                PartnerId = orderParameter.PartnerId,
                CreateDatetime = orderParameter.CreateDatetime
            };
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
                // first row
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Принять", callbackData: $"{_orderParameter.OrderNumber} Принят"),
                    InlineKeyboardButton.WithCallbackData(text: "Отклонить", callbackData: $"{_orderParameter.OrderNumber} Отклонён"),
                });
            string orderText=null;
            OrderService orderService=new OrderService();
            if (_orderParameter.DeliveryType.Id==(int)DeliveryName.CronMarket)
            {
                orderText = orderService.CreateDescriptionForCron(_orderParameter);
            }
            else if (_orderParameter.DeliveryType.Id == (int)DeliveryName.Marketplace)
            {
                 orderText = orderService.CreateDescriptionForPartner(_orderParameter);
            }    


            Models.User user = db.Users.FirstOrDefault(x => x.PartnerId == _orderParameter.PartnerId);

            if (IsOrderAccept(extendedOrder))
            {
                sentMessage = await _telegramBotService.SendMessage(user.ChatId, orderText, inlineKeyboard);
                extendedOrder.MessageId = sentMessage.MessageId;
                db.ExtendedOrders.Add(extendedOrder);
                await db.SaveChangesAsync();
            }
        }

        bool IsOrderAccept(ExtendedOrder extendedOrder)
        {
            if (extendedOrder.MessageId == null)
            {
                return true;
            }
            return false;
        }
    }
}
