import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule, MatButtonModule, MatFormFieldModule, MatInputModule } from '@angular/material';

import { TemplateService } from './services/templates/template.service';
import { LocalStorageDocumentService } from './services/documents/local-storage-document.service';
import { RouteChangeService } from './services/route-change/route-change.service';

import { TemplateSelectDialogComponent } from './components';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule
  ],
  providers: [
    TemplateService,
    LocalStorageDocumentService,
    RouteChangeService
  ],
  declarations: [
    TemplateSelectDialogComponent
  ],
  entryComponents: [
    TemplateSelectDialogComponent
  ],
  exports: [
    TemplateSelectDialogComponent
  ]
})
export class CoreModule { }
