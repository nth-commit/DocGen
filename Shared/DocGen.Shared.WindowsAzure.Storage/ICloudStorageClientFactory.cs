using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.WindowsAzure.Storage
{
    public interface ICloudStorageClientFactory
    {
        CloudBlobClient CreateBlobClient();

        CloudTableClient CreateTableClient();
    }
}
