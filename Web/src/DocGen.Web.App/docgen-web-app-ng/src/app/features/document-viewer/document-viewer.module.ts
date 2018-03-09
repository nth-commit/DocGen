import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DocumentViewerRoutingModule } from './document-viewer-routing.module';
import { DocumentViewerComponent } from './pages/document-viewer/document-viewer.component';

@NgModule({
  imports: [
    CommonModule,
    DocumentViewerRoutingModule
  ],
  declarations: [DocumentViewerComponent]
})
export class DocumentViewerModule { }
