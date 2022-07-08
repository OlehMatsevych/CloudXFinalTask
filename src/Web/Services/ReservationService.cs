using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.Azure.ServiceBus;
using Microsoft.eShopWeb.Web.ViewModels;
using Newtonsoft.Json;

namespace Microsoft.eShopWeb.Web.Services
{
    public class ReservationService
    {
        const string ServiceBusConnectionString = "Endpoint=sb://reservationdetails.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9XVm1FOG/U8b2GsGj3Sl/zwDgvXsndm13hZ5FdrT2as=";
        const string QueueName = "reservations";
        private IQueueClient queueClient;
        public ReservationService()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

        }

        public async Task SendToServiceBusAsync(CatalogItemViewModel model)
        {
            BinaryFormatter bf = new BinaryFormatter();
            var message = new Message();
            message.Body = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(model));


            await queueClient.SendAsync(message);
        }
    }
}
