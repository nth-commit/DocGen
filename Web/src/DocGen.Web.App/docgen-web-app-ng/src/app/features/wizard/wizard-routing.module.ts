import { NgModule, Injectable } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { WizardPageComponent } from './pages/wizard-page/wizard-page.component';
import { WizardPageResolve } from './pages/wizard-page/wizard-page.resolve';

const routes: Routes = [
  {
    component: WizardPageComponent,
    path: 'create/:templateId',
    resolve: {
      template: WizardPageResolve
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    WizardPageResolve
  ]
})
export class WizardRoutingModule { }
