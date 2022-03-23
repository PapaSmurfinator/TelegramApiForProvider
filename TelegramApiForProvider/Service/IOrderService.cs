using TelegramApiForProvider.Models;

namespace TelegramApiForProvider.Service
{
    public interface IOrderService
    {
        public string CreateDescriptionForPartner(OrderParameter orderParameter);
        public string CreateDescriptionForCron(OrderParameter orderParameter);

    }
}
