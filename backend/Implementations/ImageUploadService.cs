using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using Backend.ImageUploadModule;
using Azure.Storage.Blobs;

namespace Backend.Implementations;

public class ImageUploadService : IImageStorageService
{
    public async Task<string> UploadImageAsync(Guid imageId, Stream imageStream)
    {
        BlobContainerClient blobContainerClient = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=appstorageaccount144481;AccountKey=Lpl4xjwM240PWZFpGaLWXh7CzTHbYoGLi0Z4JoVyo5Mg97C1oinpMNUaWj8tX5C8/Acsxv7eGrkN+AStji6HRw==;EndpointSuffix=core.windows.net", "webappcontainer");
        BlobClient blob = blobContainerClient.GetBlobClient(imageId.ToString() + ".jpg");
        await blob.UploadAsync(imageStream, true);
        imageStream.Dispose();
        return new Uri(blob.Uri.ToString());
    }
}