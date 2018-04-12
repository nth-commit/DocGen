import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule, MatDialogModule, MatCheckboxModule } from '@angular/material';

import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { EffectsModule } from '@ngrx/effects';

import { environment } from '../../../../../environments/environment';
import { CoreModule } from '../../../_core';
import { GeneratorCoreModule } from '../_core';
import { GeneratorBulkRoutingModule } from './generator-bulk-routing.module';
import { REDUCER_ID, generatorBulkReducer, GeneratorBulkEffects, LayoutEffects, DocumentEffects } from './state';

import { GeneratorBulkComponent } from './generator-bulk.component';
import { DocumentsTableComponent } from './components/documents-table/documents-table.component';
import { WizardDialogComponent } from './components/wizard-dialog/wizard-dialog.component';
import { SelectConstantsDialogComponent } from './components/select-constants-dialog/select-constants-dialog.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,

    MatTableModule,
    MatDialogModule,
    MatCheckboxModule,

    StoreModule.forFeature(REDUCER_ID, generatorBulkReducer),
    EffectsModule.forFeature([GeneratorBulkEffects, LayoutEffects, DocumentEffects]),
    !environment.production ? StoreDevtoolsModule.instrument() : [],

    CoreModule,
    GeneratorCoreModule,
    GeneratorBulkRoutingModule
  ],
  declarations: [
    GeneratorBulkComponent,
    DocumentsTableComponent,
    WizardDialogComponent,
    SelectConstantsDialogComponent
  ],
  entryComponents: [
    WizardDialogComponent,
    SelectConstantsDialogComponent
  ],
  providers: [
  ]
})
export class GeneratorBulkModule { }
