using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage;
using Microsoft.Azure.Storage;
using System.IO;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;

namespace storage_console_app
{
    public class UploadManager
    {

        BlobServiceClient blobServiceClient;

        BlobContainerClient containerClient;

        BlockBlobClient blockBlobClient;

        string blockId;

        List<string> blockList = new List<string>();


        public UploadManager(string connectionString, string containerName, string blobName)
        {
            blobServiceClient = new BlobServiceClient(connectionString);

            containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            containerClient.CreateIfNotExists();

            blockBlobClient = containerClient.GetBlockBlobClient(blobName);

        }

        public async Task UploadFileInChunks(string filePath, int chunkSize)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                byte[] buffer = new byte[chunkSize];

                int bytesRead, blockNumber = 0; 

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    using (MemoryStream chunkStream = new MemoryStream(buffer, 0, bytesRead))
                    {
                        blockId = Convert.ToBase64String(BitConverter.GetBytes(blockNumber));

                        await blockBlobClient.StageBlockAsync(blockId, chunkStream);

                        blockNumber++;

                        blockList.Add(blockId);
                    }
                }
             
                await blockBlobClient.CommitBlockListAsync(blockList);

            }

            Console.ReadLine();

        }
       
    }

}
