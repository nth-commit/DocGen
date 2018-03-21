using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core.Templates
{
    public class TemplateTableEntity : TableEntity
    {
        public string DataJson { get; set; }

        public int DataJsonVersion { get; set; } = 1;
    }
}
