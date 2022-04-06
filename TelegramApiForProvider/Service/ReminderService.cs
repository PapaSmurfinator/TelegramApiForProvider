using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TelegramApiForProvider.DbService;

namespace TelegramApiForProvider.Service
{
    public class ReminderService : IInvocable
    {
        private readonly OrderContext db;

        private readonly ITelegramBotService _telegramBotService;

        public ReminderService(OrderContext context, ITelegramBotService telegramBotService)
        {
            db = context;
            _telegramBotService = telegramBotService;
        }

        public async Task Invoke()
        {
            DateTime endDay = DateTime.Now.Date;
            var orderMessage = db.OrderMessages.Include(x => x.Order).Where(x=>x.IsAccept==null).ToList();
            foreach (var item in orderMessage)
            {
                if (item.IsAccept==null && item.Order.CreateDatetime.Date== endDay)
                {
                    await _telegramBotService.SendMessage(item.ChatId, $"❗️Вы все еще не приняли заказ.❗️\n\n" +
                                                                   $"Заказ номер: {item.Order.OrderNumber} от {item.Order.CreateDatetime}", item.MessageId);
                }
            }
        }
    }
}