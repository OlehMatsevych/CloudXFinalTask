using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Http;
using System.Text;

namespace OrderItemsReserver
{
    public class Function1
    {
        private BlobServiceClient serviceClient;
        private BlobContainerClient containerClient;
        int maxRetriesCount = 3;
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=reservationsmatsevych;AccountKey=7OgwYnFgDWInYL6j/BcGRmZ3UTsZsn/69ASLiYDCouN5uUZbvSoix1wt1NAdpXa25hs+w95c/i/p+AStDq1+UA==;EndpointSuffix=core.windows.net";
        string containerName = "reservations";

        [FunctionName("Function1")]
        public void Run([ServiceBusTrigger("reservations", Connection = "Endpoint=sb://reservationdetails.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9XVm1FOG/U8b2GsGj3Sl/zwDgvXsndm13hZ5FdrT2as=")]string myQueueItem, ILogger log)
        {
            try
            {

                var blobClientOptions = new BlobClientOptions();
                blobClientOptions.Retry.MaxRetries = maxRetriesCount;

                serviceClient =  new BlobServiceClient(connectionString, blobClientOptions);

                containerClient = serviceClient.GetBlobContainerClient(containerName);

                BlobClient blob = containerClient.GetBlobClient("reservations");
            

                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(myQueueItem)))
                {
                    blob.Upload(ms);
                }
            }
            catch (System.Exception)
            {
                HttpClient httpClient = new HttpClient();
                var response = httpClient.PostAsync("https://matsevych.azurewebsites.net/api/ErrorHandling/triggers/manual/invoke?api-version=2022-05-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=qljcmwq123_RzZKLwucfWx1nT2Hd2XzOm75q9GmcqMQ", new StringContent(myQueueItem, Encoding.UTF8, "application/json"));
            }
        }
    }
}
