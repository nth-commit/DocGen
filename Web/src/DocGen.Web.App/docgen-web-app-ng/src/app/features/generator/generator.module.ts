import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatCardModule, MatDialogModule } from '@angular/material';

import { CoreModule } from '../_core';
import { GeneratorBulkModule } from './features/generator-bulk';

import { GeneratorRoutingModule } from './generator-routing.module';
import { GeneratorComponent } from './generator.component';

@NgModule({
  imports: [
    CommonModule,

    MatCardModule,
    MatButtonModule,
    MatDialogModule,

    GeneratorRoutingModule,
    GeneratorBulkModule
  ],
  declarations: [GeneratorComponent]
})
export class GeneratorModule { }
