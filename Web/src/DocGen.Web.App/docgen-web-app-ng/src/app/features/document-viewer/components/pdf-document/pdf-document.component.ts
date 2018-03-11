import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

import { PdfDocumentRendererService } from '../../services/renderers';
import { SerializableDocument } from '../../../core';

@Component({
  selector: 'app-document-viewer-pdf-document',
  templateUrl: './pdf-document.component.html',
  styleUrls: ['./pdf-document.component.scss']
})
export class PdfDocumentComponent implements OnInit {
  @Input() serializableDocument: SerializableDocument;

  pdfBlobUri: SafeResourceUrl;

  constructor(
    private pdfDocumentRenderer: PdfDocumentRendererService,
    private domSanitizer: DomSanitizer,
  ) { }

  ngOnInit() {
    this.pdfDocumentRenderer.render(this.serializableDocument).then(result => {
      this.pdfBlobUri = this.domSanitizer.bypassSecurityTrustResourceUrl(result);
    });
  }
}
