import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DocumentViewerPageComponent } from './pages/document-viewer/document-viewer-page.component';

import { TextDocumentResolve } from './pages/document-viewer/text-document.resolve';
import { TemplateResolve } from './pages/document-viewer/template.resolve';

const routes: Routes = [
  {
    component: DocumentViewerPageComponent,
    path: ':templateId/preview',
    resolve: {
      textDocument: TextDocumentResolve,
      template: TemplateResolve
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    TextDocumentResolve,
    TemplateResolve
  ]
})
export class DocumentViewerRoutingModule { }
