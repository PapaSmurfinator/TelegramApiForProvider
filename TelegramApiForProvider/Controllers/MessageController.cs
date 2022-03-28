using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramApiForProvider.DbService;
using TelegramApiForProvider.Extended;
using TelegramApiForProvider.Models;
using TelegramApiForProvider.Service;

namespace TelegramApiForProvider.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly OrderContext db;

        private readonly ITelegramBotService _telegramBotService;
        private readonly ISendService _sendService;

        public MessageController(OrderContext context, ITelegramBotService telegramBotService, ISendService sendService)
        {
            db = context;
            _telegramBotService = telegramBotService;
            _sendService = sendService;
        }

        [HttpPost]
        public async Task UpdateAsync([FromBody] Update update)
        {
            if (update.Message != null)
            {
                if (update.Message.Text == "/start")
                {
                    _telegramBotService.SendMessage(update.Message.From.Id, "Введите номер телефона");
                }
                Models.User user = new Models.User
                {
                    Name = update.Message.From.Username,
                    PhoneNumber = update.Message.Text,
                    ChatId = update.Message.From.Id,
                };
                var response = await _sendService.ConfirmPassword(user.PhoneNumber);
                if (response != null)
                {
                    if (!db.Users.Any(x => x.PhoneNumber == user.PhoneNumber))
                    {
                        user.PartnerId = response.PartnerId;
                        db.Users.Add(user);
                        await db.SaveChangesAsync();
                        _telegramBotService.SendMessage(update.Message.From.Id, "Вход выполнен, ждите заказов");
                    }
                    _telegramBotService.SendMessage(update.Message.From.Id, "Вы уже вошли");

                }
                _telegramBotService.SendMessage(update.Message.From.Id, "Ошибка при входе");


            }
            await CallbackHandlingAsync(update.CallbackQuery);
        }

        private async Task CallbackHandlingAsync(CallbackQuery callbackQuery)
        {
            if (callbackQuery != null)
            {
                long chatId = callbackQuery.From.Id;
                List<ExtendedOrder> extendedOrders = db.ExtendedOrders.ToList();

                foreach (var item in extendedOrders)
                {
                    if (item.IsAccept == null)
                    {
                        if (callbackQuery.Data == $"{item.OrderNumber} Принят")
                        {
                            item.IsAccept = true;
                            await _telegramBotService.SendMessage(chatId, $"Заказ номер {item.OrderNumber} принят на обработку.",item.MessageId);
                            _telegramBotService.EditMessage(chatId, (int)item.MessageId);
                            RequestData requestData = new RequestData
                            {
                                OrderId = item.Id,
                                StatusId = (int)OrderStatus.Accept
                            };
                            db.SaveChanges();
                            await _sendService.SendStatus(requestData);
                        }
                        if (callbackQuery.Data == $"{item.OrderNumber} Отклонён")
                        {
                            item.IsAccept = false;
                            await _telegramBotService.SendMessage(chatId, $"Заказ номер {item.OrderNumber} отклонён.", item.MessageId);
                            _telegramBotService.EditMessage(chatId, (int)item.MessageId);
                            RequestData requestData = new RequestData
                            {
                                OrderId = item.Id,
                                StatusId = (int)OrderStatus.Cancel
                            };
                            db.SaveChanges();
                            await _sendService.SendStatus(requestData);
                        }
                    }
                }
            }
        }
    }
}
