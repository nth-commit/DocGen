import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatTableModule } from '@angular/material';

import { GeneratorBulkRoutingModule } from './generator-bulk-routing.module';
import { GeneratorBulkStateModule } from './state';

import { GeneratorBulkComponent } from './generator-bulk.component';
import { DocumentsTableComponent } from './components/documents-table/documents-table.component';

@NgModule({
  imports: [
    CommonModule,

    MatTableModule,

    GeneratorBulkRoutingModule,
    GeneratorBulkStateModule
  ],
  declarations: [GeneratorBulkComponent, DocumentsTableComponent]
})
export class GeneratorBulkModule { }
