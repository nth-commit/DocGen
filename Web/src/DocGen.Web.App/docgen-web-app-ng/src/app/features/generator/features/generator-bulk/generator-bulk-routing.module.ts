import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { GeneratorBulkResolveTemplate } from './generator-bulk-resolve.template';

import { GeneratorBulkComponent } from './generator-bulk.component';

const routes: Routes = [
  {
    path: 'create/:templateId/bulk',
    component: GeneratorBulkComponent,
    resolve: {
      template: GeneratorBulkResolveTemplate
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    GeneratorBulkResolveTemplate
  ]
})
export class GeneratorBulkRoutingModule { }
