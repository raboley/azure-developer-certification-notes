using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;


namespace QueueApp
{
    class Program
    {
        private const string connectionString = "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=rabqueue;AccountKey=4FW1XFTTGK9kRN8uF2CVIf58vEDcQ+4z3jBIyDRJNQ612aRqH7Z8ml/Dxa7vW3HtcWKpi7Jpo0R1CgbdZB74bg==";

        static async Task SendArticleAsync(string newsMessage)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient client = account.CreateCloudQueueClient();
            CloudQueue queue = client.GetQueueReference("newsqueue");
            bool createdQueue = await queue.CreateIfNotExistsAsync();
            if (createdQueue)
            {
                Console.WriteLine("The queue of news articles was created!");
            }

            var message = new CloudQueueMessage(newsMessage);
            await queue.AddMessageAsync(message);

        }

        static void Main(string[] args)
        {
            
            if (args.Length > 0)
            {
                string newsMessage = string.Join(" ", args);
                SendArticleAsync(newsMessage).Wait();
                Console.WriteLine($"Sent: {newsMessage}");
            }
        }
    }
}
