using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramApiForProvider.Service
{
    public interface ITelegramBotService
    {
        void SendMessage(long chatId, string text);
        Task<Message> SendMessage(long chatId, string text, IReplyMarkup replyMarkup,ParseMode parse);
        void EditMessage(long chatId, int messageId);

        Task<Message> SendMessage(long chatId, string text, int? reply_to_message_id);
    }
}
