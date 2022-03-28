using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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
        private readonly IOrderService _orderService;

        public SendingController(OrderContext context, ITelegramBotService telegramBotService, IOrderService orderService)
        {
            db = context;
            _telegramBotService = telegramBotService;
            _orderService = orderService;
        }

        private Message sentMessage = null;
        private ExtendedOrder extendedOrder;


        [HttpPost]
        public async Task ReceiveAndSend(OrderParameter orderParameter)
        {
            extendedOrder = new ExtendedOrder
            {
                Id = orderParameter.Id,
                OrderNumber = orderParameter.OrderNumber,
                PartnerName = orderParameter.PartnerName,
                PartnerId = orderParameter.PartnerId,
                CreateDatetime = orderParameter.CreateDatetime
            };

            
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

            Models.User user = db.Users.FirstOrDefault(x => x.PartnerId == orderParameter.PartnerId);

            if (IsOrderAccept(extendedOrder))
            {
                sentMessage = await _telegramBotService.SendMessage(user.ChatId, orderText, inlineKeyboard, ParseMode.Html);
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
