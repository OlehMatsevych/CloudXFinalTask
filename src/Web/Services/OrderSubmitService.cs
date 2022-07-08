using System.Text;
using System.Text.Json;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.Web.Pages.Basket;

namespace Microsoft.eShopWeb.Web.Services
{
    public class OrderSubmitService
    {
        private string functionUrl = "https://orderprocessormatsevych.azurewebsites.net/api/Function1?code=AyN25Mv10PzJBQZouLR_ew0xnvAtIHNU5rdwXaJVgBJMAzFusMBRQg==";

        public async Task SendDataAsync(IEnumerable<BasketItemViewModel> items)
        {
            var model = new
            {
                shippingAddress = new Address("123 Main St.", "Kent", "OH", "United States", "44240"),
                items = items,
                finalPrice = items.Sum(x => x.Quantity)
            };
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(model, options);

            using (var client = new HttpClient())
            {

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(functionUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
        }
    }
}
