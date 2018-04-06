import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StoreModule, combineReducers, Action } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { EffectsModule } from '@ngrx/effects';

import { environment } from '../../../../../../environments/environment';

import { generatorWizardReducer, GeneratorWizardState } from '../../_shared';
import { generatorBulkDocumentsReducer, GeneratorBulkDocumentsState } from './documents';

export interface GeneratorBulkState {
  wizard: GeneratorWizardState;
  documents: GeneratorBulkDocumentsState;
}

export const generatorBulkReducer = combineReducers<GeneratorBulkState, Action>(
  {
    wizard: generatorWizardReducer,
    documents: generatorBulkDocumentsReducer
  }
);

@NgModule({
  imports: [
    CommonModule,

    StoreModule.forFeature('generatorBulk', generatorBulkReducer)
  ]
})
export class GeneratorBulkStateModule { }
