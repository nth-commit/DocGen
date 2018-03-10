import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TemplateService } from './services/templates/template.service';

@NgModule({
  imports: [
    CommonModule
  ],
  providers: [
    TemplateService
  ],
  declarations: [],
  exports: [
  ]
})
export class CoreModule { }
