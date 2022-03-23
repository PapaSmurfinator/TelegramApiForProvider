using TelegramApiForProvider.Models;

namespace TelegramApiForProvider.Extended
{
    public class ExtendedOrder : Order
    {
        public bool? IsAccept { get; set; }
        public int? MessageId { get; set; }
    }
}
