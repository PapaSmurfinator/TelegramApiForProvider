using Coravel.Invocable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramApiForProvider.DbService;
using TelegramApiForProvider.Extended;

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
            List<ExtendedOrder> extendedOrders = db.ExtendedOrders.ToList();
            foreach (var item in extendedOrders)
            {
                if (item.IsAccept == null && item.CreateDatetime <= DateTime.Today.AddDays(1))
                {
                    var user = db.Users.Where(x => x.PartnerId == item.PartnerId).FirstOrDefault();
                    await _telegramBotService.SendMessage(user.ChatId, $"Вы все еще не приняли заказ.\n\nЗаказ номер: {item.OrderNumber} от {item.CreateDatetime}", item.MessageId);
                    continue;
                }
            }
        }
    }
}
