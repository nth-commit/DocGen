import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatToolbarModule } from '@angular/material';

import { DocumentViewerRoutingModule } from './document-viewer-routing.module';
import { DocumentViewerPageComponent } from './pages/document-viewer/document-viewer-page.component';
import { TextDocumentComponent } from './components/text-document/text-document.component';

@NgModule({
  imports: [
    CommonModule,
    MatToolbarModule,
    DocumentViewerRoutingModule
  ],
  declarations: [DocumentViewerPageComponent, TextDocumentComponent]
})
export class DocumentViewerModule { }
