using DocGen.Templating.Rendering.Builders.V1;
using DocGen.Templating.Rendering.Instructions.V1;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering.Pdf.V1
{
    public class PdfDocumentBuilderV1 : IDocumentBuilderV1<PdfDocument>, IDisposable
    {
        private Document _pdfDocument = null;
        private MemoryStream _memoryStream = null;
        private PdfWriter _pdfWriter = null;
        private bool _isComplete = false;

        public PdfDocument Result
        {
            get
            {
                if (!_isComplete)
                {
                    throw new Exception("Rendering is not complete");
                }

                return new PdfDocument()
                {
                    Contents = _memoryStream.ToArray()
                };
            }
        }

        public int MarkupVersion => 1;

        public Task BeginWriteDocumentAsync(DocumentInstructionContextV1 context)
        {
            _pdfDocument = new Document(PageSize.A4);
            _memoryStream = new MemoryStream();
            _pdfWriter = PdfWriter.GetInstance(_pdfDocument, _memoryStream);
            _pdfDocument.Open();
            return Task.CompletedTask;
        }

        public Task EndWriteDocumentAsync(DocumentInstructionContextV1 context)
        {
            _pdfDocument.Close();
            _isComplete = true;
            return Task.CompletedTask;
        }

        public Task BeginWritePageAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task EndWritePageAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task EndWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginWriteListAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task EndWriteListAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginWriteListItemAsync(int index, DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task EndWriteListItemAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginConditionalAsync(string expression, DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task EndCondititionalAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task WriteTextAsync(string text, string reference, DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _memoryStream.Dispose();
        }
    }
}
