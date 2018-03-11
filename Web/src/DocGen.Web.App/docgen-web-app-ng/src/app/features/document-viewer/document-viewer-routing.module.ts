import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DocumentViewerPageComponent } from './pages/document-viewer/document-viewer-page.component';

import { DocumentResolve } from './pages/document-viewer/document.resolve';
import { TemplateResolve } from './pages/document-viewer/template.resolve';

const routes: Routes = [
  {
    component: DocumentViewerPageComponent,
    path: ':templateId/preview',
    resolve: {
      document: DocumentResolve,
      template: TemplateResolve
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    DocumentResolve,
    TemplateResolve
  ]
})
export class DocumentViewerRoutingModule { }
