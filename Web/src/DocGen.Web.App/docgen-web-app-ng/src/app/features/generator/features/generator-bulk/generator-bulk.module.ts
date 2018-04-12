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
import { REDUCER_ID, generatorBulkReducer, GeneratorBulkEffects, LayoutEffects } from './state';

import { GeneratorBulkComponent } from './generator-bulk.component';
import { DocumentsTableComponent } from './components/documents-table/documents-table.component';
import { WizardDialogComponent } from './components/wizard-dialog/wizard-dialog.component';
import { DocumentValueSelectionTableComponent } from './components/document-value-selection-table/document-value-selection-table.component';

import { DocumentValueSelectorDialogComponent } from './services/document-value-selector/document-value-selector-dialog.component';
import { DocumentValueSelectorService } from './services/document-value-selector/document-value-selector.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,

    MatTableModule,
    MatDialogModule,
    MatCheckboxModule,

    StoreModule.forFeature(REDUCER_ID, generatorBulkReducer),
    EffectsModule.forFeature([GeneratorBulkEffects, LayoutEffects]),
    !environment.production ? StoreDevtoolsModule.instrument() : [],

    CoreModule,
    GeneratorCoreModule,
    GeneratorBulkRoutingModule
  ],
  declarations: [
    GeneratorBulkComponent,
    DocumentsTableComponent,
    WizardDialogComponent,
    DocumentValueSelectionTableComponent,
    DocumentValueSelectorDialogComponent
  ],
  entryComponents: [
    WizardDialogComponent,
    DocumentValueSelectorDialogComponent
  ],
  providers: [
    DocumentValueSelectorService
  ]
})
export class GeneratorBulkModule { }
