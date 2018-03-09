import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DocumentViewerRoutingModule } from './document-viewer-routing.module';
import { DocumentViewerComponent } from './pages/document-viewer/document-viewer.component';
import { TextDocumentComponent } from './components/text-document/text-document.component';

@NgModule({
  imports: [
    CommonModule,
    DocumentViewerRoutingModule
  ],
  declarations: [DocumentViewerComponent, TextDocumentComponent]
})
export class DocumentViewerModule { }
