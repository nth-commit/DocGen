import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DocumentViewerRoutingModule } from './document-viewer-routing.module';
import { DocumentViewerPageComponent } from './pages/document-viewer/document-viewer-page.component';
import { TextDocumentComponent } from './components/text-document/text-document.component';

@NgModule({
  imports: [
    CommonModule,
    DocumentViewerRoutingModule
  ],
  declarations: [DocumentViewerPageComponent, TextDocumentComponent]
})
export class DocumentViewerModule { }
