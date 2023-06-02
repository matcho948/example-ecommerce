using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using Backend.ImageUploadModule;
using Azure.Storage.Blobs;
using System.Configuration;

namespace Backend.Implementations;

public class ImageUploadService : IImageStorageService
{
    private readonly IConfiguration _configuration;

    public ImageUploadService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> UploadImageAsync(Guid imageId, Stream imageStream)
    {
        BlobContainerClient blobContainerClient = new BlobContainerClient(_configuration.GetConnectionString("StorageAccount"), "webappcontainer");
        BlobClient blob = blobContainerClient.GetBlobClient(imageId.ToString() + ".jpg");
        await blob.UploadAsync(imageStream, true);
        imageStream.Dispose();
        return new Uri(blob.Uri.ToString()).PathAndQuery;
    }
}