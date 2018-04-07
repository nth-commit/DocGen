import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatDialogModule } from '@angular/material';

import { GeneratorCoreModule } from '../_core';
import { GeneratorBulkRoutingModule } from './generator-bulk-routing.module';
import { GeneratorBulkStateModule } from './state';

import { GeneratorBulkComponent } from './generator-bulk.component';
import { DocumentsTableComponent } from './components/documents-table/documents-table.component';
import { WizardDialogComponent } from './components/wizard-dialog/wizard-dialog.component';

@NgModule({
  imports: [
    CommonModule,

    MatTableModule,
    MatDialogModule,

    GeneratorCoreModule,
    GeneratorBulkStateModule,
    GeneratorBulkRoutingModule
  ],
  declarations: [
    GeneratorBulkComponent,
    DocumentsTableComponent,
    WizardDialogComponent
  ],
  entryComponents: [
    WizardDialogComponent
  ]
})
export class GeneratorBulkModule { }
