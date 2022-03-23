using System.Threading.Tasks;
using TelegramApiForProvider.Models;
using TelegramApiForProvider.Contract;

namespace TelegramApiForProvider.Service
{
    public interface ISendService
    {
        Task SendStatus(RequestData requestData);
        Task<CheckPartnerResponseModel> ConfirmPassword(string phoneNumber);
    }
}
