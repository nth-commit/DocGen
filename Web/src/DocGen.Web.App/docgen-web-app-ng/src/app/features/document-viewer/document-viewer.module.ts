import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatToolbarModule, MatButtonModule } from '@angular/material';

import { DocumentViewerRoutingModule } from './document-viewer-routing.module';

import { DocumentRenderingService } from './services/document-rendering/document-rendering.service';

import { DocumentViewerPageComponent } from './pages/document-viewer/document-viewer-page.component';
import { TextDocumentComponent } from './components/text-document/text-document.component';
import { PdfDocumentComponent } from './components/pdf-document/pdf-document.component';

@NgModule({
  imports: [
    CommonModule,
    MatToolbarModule, MatButtonModule,
    DocumentViewerRoutingModule
  ],
  providers: [
    DocumentRenderingService
  ],
  declarations: [
    DocumentViewerPageComponent,
    TextDocumentComponent,
    PdfDocumentComponent
  ]
})
export class DocumentViewerModule { }
