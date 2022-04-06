using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramApiForProvider.DbService;
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
        private readonly IOrderService _orderService;
        private readonly ISendService _sendService;

        public SendingController(OrderContext context, ITelegramBotService telegramBotService, IOrderService orderService, ISendService sendService)
        {
            db = context;
            _telegramBotService = telegramBotService;
            _orderService = orderService;
            _sendService = sendService;
        }

        private Message sentMessage = null;
        private Order order;
        private OrderMessage orderMessage;


        [HttpPost]
        public async Task ReceiveAndSend(OrderParameter orderParameter)
        {
            var user = db.Users.Where(x => x.PartnerId == orderParameter.PartnerId).FirstOrDefault();
            var response = _sendService.ConfirmPassword(user.PhoneNumber).Result;
            if (response != null)
            {
                var users = db.Users.Where(x => x.PartnerId == orderParameter.PartnerId).ToList();
                order = new Order
                {
                    Id = orderParameter.Id,
                    OrderNumber = orderParameter.OrderNumber,
                    PartnerName = orderParameter.PartnerName,
                    PartnerId = orderParameter.PartnerId,
                    CreateDatetime = orderParameter.CreateDatetime
                };
                foreach (var item in users)
                {
                    orderMessage = new OrderMessage
                    {
                        Id = Guid.NewGuid(),
                        IsAccept = null,
                        MessageId = null,
                        ChatId = item.ChatId,
                        OrderId = orderParameter.Id
                    };
                    db.OrderMessages.Add(orderMessage);
                }
                db.Orders.Add(order);
                db.SaveChanges();

                InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
                    new InlineKeyboardButton[]
                    {
                    InlineKeyboardButton.WithCallbackData(text: "Принять ✅", callbackData: $"{orderParameter.OrderNumber} Принят"),
                    InlineKeyboardButton.WithCallbackData(text: "Отклонить ❌", callbackData: $"{orderParameter.OrderNumber} Отклонён"),
                    });

                string orderText = null;
                if (orderParameter.DeliveryType.Id == (int)DeliveryName.CronMarket)
                {
                    orderText = _orderService.CreateDescriptionForCron(orderParameter);
                }
                if (orderParameter.DeliveryType.Id == (int)DeliveryName.Marketplace)
                {
                    orderText = _orderService.CreateDescriptionForPartner(orderParameter);
                }

                foreach (var item in users)
                {

                    var orderrr = db.OrderMessages.Where(x => x.OrderId == orderParameter.Id).FirstOrDefault(x => x.MessageId == null);
                    if (IsOrderAccept(orderrr))
                    {
                        sentMessage = await _telegramBotService.SendMessage(orderrr.ChatId, orderText, inlineKeyboard, ParseMode.Html);
                        orderrr.MessageId = sentMessage.MessageId;
                        db.OrderMessages.Update(orderrr);
                        await db.SaveChangesAsync();
                    }

                }
            }

            bool IsOrderAccept(OrderMessage orderMessage)
            {
                if (orderMessage.MessageId == null)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
