import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DocumentViewerPageComponent } from './pages/document-viewer/document-viewer-page.component';

import { DocumentViewerPageResolve } from './pages/document-viewer/document-viewer-page.resolve';

const routes: Routes = [
  {
    component: DocumentViewerPageComponent,
    path: ':templateId/preview',
    resolve: {
      textDocument: DocumentViewerPageResolve
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    DocumentViewerPageResolve
  ]
})
export class DocumentViewerRoutingModule { }
