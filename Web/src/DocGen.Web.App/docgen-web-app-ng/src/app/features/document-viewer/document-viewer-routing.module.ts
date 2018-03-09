import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DocumentViewerComponent } from './pages/document-viewer/document-viewer.component';

import { DocumentViewerResolve } from './pages/document-viewer/document-viewer.resolve';

const routes: Routes = [
  {
    component: DocumentViewerComponent,
    path: ':templateId/preview',
    resolve: {
      textDocument: DocumentViewerResolve
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    DocumentViewerResolve
  ]
})
export class DocumentViewerRoutingModule { }
