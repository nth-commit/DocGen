using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core.Signing
{
    public class SigningRequestTableEntity : TableEntity
    {
        public string DataJson { get; set; }

        public int DataJsonVersion { get; set; } = 1;
    }
}
