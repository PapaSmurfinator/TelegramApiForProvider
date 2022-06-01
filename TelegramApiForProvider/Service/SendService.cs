using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TelegramApiForProvider.Models;
using TelegramApiForProvider.Contract;

namespace TelegramApiForProvider.Service
{
    public class SendService : ISendService
    {
        public async Task<CheckPartnerResponseModel> ConfirmPassword(string phoneNumber)
        {
            var url = "https://api.cronmarket.ru/api/adminpanel/v1/Tbot/CheckPartnerPhone?" + $"partnerPhone={phoneNumber}";
            using var client = new HttpClient();

            var response = await client.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<CheckPartnerResponseModel>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task SendStatus(RequestData requestData)
        {
            var json = JsonConvert.SerializeObject(requestData, Formatting.Indented,
                                    new JsonSerializerSettings()
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                    });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "https://api.cronmarket.ru/api/adminpanel/v1/Tbot/SetOrderStatus";
            using var client = new HttpClient();

            var response = await client.PutAsync(url, data);
        }
    }
}
