import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

import { DocumentRenderingService } from '../../services/document-rendering/document-rendering.service';
import { SerializableDocument } from '../../../_core';

@Component({
  selector: 'app-document-viewer-pdf-document',
  templateUrl: './pdf-document.component.html',
  styleUrls: ['./pdf-document.component.scss']
})
export class PdfDocumentComponent implements OnInit {
  @Input() serializableDocument: SerializableDocument;

  pdfBlobUri: SafeResourceUrl;

  constructor(
    private documentRenderingService: DocumentRenderingService,
    private domSanitizer: DomSanitizer,
  ) { }

  ngOnInit() {
    this.documentRenderingService.render(this.serializableDocument, 'pdf').then(result => {
      this.pdfBlobUri = this.domSanitizer.bypassSecurityTrustResourceUrl(result);
    });
  }
}
