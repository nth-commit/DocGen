import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TemplateService } from './services/templates/template.service';
import { LocalStorageDocumentService } from './services/documents/local-storage-document.service';
import { RouteChangeService } from './services/route-change/route-change.service';

@NgModule({
  imports: [
    CommonModule
  ],
  providers: [
    TemplateService,
    LocalStorageDocumentService,
    RouteChangeService
  ],
  declarations: [],
  exports: [
  ]
})
export class CoreModule { }
