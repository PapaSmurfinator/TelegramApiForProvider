using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramApiForProvider.DbService;
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
                    _telegramBotService.SendMessage(update.Message.From.Id, "Введите номер телефона в формате 79*********");
                }
                else
                {
                    var phoneNumber = update.Message.Text;
                    var response = _sendService.ConfirmPassword(phoneNumber).Result;
                    if (response != null)
                    {
                        if (!db.Users.Any(x => x.PhoneNumber == phoneNumber && x.ChatId == update.Message.From.Id))
                        {
                            Models.User user = new Models.User
                            {
                                Name = update.Message.From.Username,
                                PhoneNumber = update.Message.Text,
                                ChatId = update.Message.From.Id,
                                PartnerId = response.PartnerId
                            };
                            db.Users.Add(user);
                            await db.SaveChangesAsync();
                            _telegramBotService.SendMessage(update.Message.From.Id, $"Вход выполнен, {response.PartnerName}, ждите заказов");
                        }
                        else
                        {
                            Models.User userUpdate = db.Users.FirstOrDefault(n => n.PhoneNumber == phoneNumber);
                            userUpdate.PartnerId = response.PartnerId;
                            userUpdate.Name = update.Message.From.Username;
                            userUpdate.PhoneNumber = phoneNumber;
                            db.Users.Update(userUpdate);
                            await db.SaveChangesAsync();
                            _telegramBotService.SendMessage(update.Message.From.Id, $"Вход выполнен, {response.PartnerName}, ждите заказов");
                        }
                    }
                    else
                    {
                        _telegramBotService.SendMessage(update.Message.From.Id, "Ошибка при входе");
                    }
                }
            }
            await CallbackHandlingAsync(update.CallbackQuery);
        }

        private async Task CallbackHandlingAsync(CallbackQuery callbackQuery)
        {
            if (callbackQuery != null)
            {
                long chatId = callbackQuery.From.Id;
                List<OrderMessage> orderMessages = db.OrderMessages.Include(x => x.Order).Where(x => x.ChatId == chatId).ToList();

                foreach (var item in orderMessages)
                {
                    if (item.IsAccept == null)
                    {
                        if (callbackQuery.Data == $"{item.Order.OrderNumber} Принят")
                        {
                            string orderNumber = callbackQuery.Data.Replace(" Принят", "");
                            var orderrr = db.OrderMessages.Include(x => x.Order).Where(x => x.Order.OrderNumber == orderNumber);
                            foreach (var order in orderrr)
                            {
                                order.IsAccept = true;
                                await _telegramBotService.SendMessage(order.ChatId, $"Заказ номер {item.Order.OrderNumber} принят на обработку.", order.MessageId);
                                _telegramBotService.EditMessage(order.ChatId, (int)order.MessageId);
                            }
                            RequestData requestData = new RequestData
                            {
                                OrderId = item.Order.Id,
                                StatusId = (int)OrderStatus.Accept
                            };
                            db.SaveChanges();
                            await _sendService.SendStatus(requestData);
                        }
                        if (callbackQuery.Data == $"{item.Order.OrderNumber} Отклонён")
                        {
                            string orderNumber = callbackQuery.Data.Replace(" Отклонён", "");
                            var orderrr = db.OrderMessages.Include(x => x.Order).Where(x => x.Order.OrderNumber == orderNumber);
                            foreach (var order in orderrr)
                            {
                                order.IsAccept = true;
                                await _telegramBotService.SendMessage(order.ChatId, $"Заказ номер {item.Order.OrderNumber} отклонён.", order.MessageId);
                                _telegramBotService.EditMessage(order.ChatId, (int)order.MessageId);
                            }
                            RequestData requestData = new RequestData
                            {
                                OrderId = item.Order.Id,
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
