import { NgModule, Injectable } from '@angular/core';
import { Routes, RouterModule, UrlMatchResult } from '@angular/router';

import { WizardPageComponent } from './pages/wizard-page/wizard-page.component';
import { WizardPageResolve } from './pages/wizard-page/wizard-page.resolve';

const routes: Routes = [
  {
    component: WizardPageComponent,
    matcher: function (url) {
      return url.length === 2 && url[0].path === 'create' && url[1].path && url[1].path !== 'bulk' ?
        { consumed: url, posParams: { templateId: url[1] } } :
        null;
    },
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
