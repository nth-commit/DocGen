import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule, MatDialogModule, MatCheckboxModule } from '@angular/material';

import { GeneratorCoreModule } from '../_core';
import { GeneratorBulkRoutingModule } from './generator-bulk-routing.module';
import { GeneratorBulkStateModule } from './state';

import { GeneratorBulkComponent } from './generator-bulk.component';
import { DocumentsTableComponent } from './components/documents-table/documents-table.component';
import { WizardDialogComponent } from './components/wizard-dialog/wizard-dialog.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,

    MatTableModule,
    MatDialogModule,
    MatCheckboxModule,

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
