import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatTableModule } from '@angular/material';

import { GeneratorBulkRoutingModule } from './generator-bulk-routing.module';
import { GeneratorBulkStateModule } from './state';

import { GeneratorBulkComponent } from './generator-bulk.component';
import { ContractsTableComponent } from './components/contracts-table/contracts-table.component';

@NgModule({
  imports: [
    CommonModule,

    MatTableModule,

    GeneratorBulkRoutingModule,
    GeneratorBulkStateModule
  ],
  declarations: [GeneratorBulkComponent, ContractsTableComponent]
})
export class GeneratorBulkModule { }
