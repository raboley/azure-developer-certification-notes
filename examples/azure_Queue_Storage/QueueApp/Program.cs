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

            CloudQueue queue = GetQueue();
            bool createdQueue = await queue.CreateIfNotExistsAsync();
            if (createdQueue)
            {
                Console.WriteLine("The queue of news articles was created!");
            }

            var message = new CloudQueueMessage(newsMessage);
            await queue.AddMessageAsync(message);

        }

        static CloudQueue GetQueue()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient client = account.CreateCloudQueueClient();
            return client.GetQueueReference("newsqueue");
        }

        static async Task<string> ReceiveArticleAsync()
        {
         CloudQueue queue = GetQueue();
         bool exists = await queue.ExistsAsync();
         if (exists)
         {
             CloudQueueMessage retrievedArticle = await queue.GetMessageAsync();
             if (retrievedArticle != null)
             {
                 string newsMessage = retrievedArticle.AsString;
                 await queue.DeleteMessageAsync(retrievedArticle);
                 return newsMessage;
             }

         }

         return "<queue empty or not created>";

        }

        static async Task Main(string[] args)
        {
            
            if (args.Length > 0)
            {
                string newsMessage = string.Join(" ", args);
                await SendArticleAsync(newsMessage);
                Console.WriteLine($"Sent: {newsMessage}");
            } 
            else 
            {
                string newsMessage = await ReceiveArticleAsync();
                Console.WriteLine($"Received: {newsMessage}");
            }
        }
    }
}
