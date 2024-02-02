using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage_console_app
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=containerazure231;AccountKey=OUm3Xf3OfcQPAOlouhCgwYiQLDO5J+DRv5rGBUbI34/kjzNcOZyDh5BS3rkgez3xGj6KyDAsLLmW+AStV+L9FQ==;EndpointSuffix=core.windows.net";

            string containerName = "containerazure231";

            string filePath = @"C:\Users\faisal.haroon\Desktop\doc2.pdf";

            string blobName = "doc2.pdf";

            // chunk size of 4MB
            int chunkSize = 4 * 1024 * 1024;

            UploadManager uploadManager = new UploadManager(connectionString, containerName, blobName);

            uploadManager.UploadFileInChunks(filePath, chunkSize).GetAwaiter().GetResult();

        }
    }
}
