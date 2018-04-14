import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule, MatDialogModule, MatCheckboxModule, MatButtonModule, MatIconModule, MatMenuModule } from '@angular/material';

import { StoreModule, ActionReducer } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { EffectsModule } from '@ngrx/effects';
import { LocalStorageConfig, localStorageSync } from 'ngrx-store-localstorage';

import { environment } from '../../../../../environments/environment';
import { CoreModule } from '../../../_core';
import { GeneratorCoreModule } from '../_core';
import { GeneratorBulkRoutingModule } from './generator-bulk-routing.module';
import { REDUCER_ID, generatorBulkReducer, GeneratorBulkEffects, LayoutEffects, DocumentEffects } from './state';

import { GeneratorBulkComponent } from './generator-bulk.component';
import { DocumentsTableComponent } from './components/documents-table/documents-table.component';
import { WizardDialogComponent } from './components/wizard-dialog/wizard-dialog.component';
import { SelectConstantsDialogComponent } from './components/select-constants-dialog/select-constants-dialog.component';
import { DocumentValueSelectTableComponent } from './components/document-value-select-table/document-value-select-table.component';

export function localStorageMetaReducer(reducer: ActionReducer<any>): ActionReducer<any> {
  const config: LocalStorageConfig = {
    keys: [
      'documents'
    ],
    rehydrate: true,
    removeOnUndefined: true
  };

  return localStorageSync(config)(reducer);
}

@NgModule({
  imports: [
    CommonModule,
    FormsModule,

    MatTableModule,
    MatDialogModule,
    MatCheckboxModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,

    StoreModule.forFeature(REDUCER_ID, generatorBulkReducer, {
      metaReducers: [
        localStorageMetaReducer
      ]
    }),
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
    SelectConstantsDialogComponent,
    DocumentValueSelectTableComponent
  ],
  entryComponents: [
    WizardDialogComponent,
    SelectConstantsDialogComponent
  ],
  providers: [
  ]
})
export class GeneratorBulkModule { }
