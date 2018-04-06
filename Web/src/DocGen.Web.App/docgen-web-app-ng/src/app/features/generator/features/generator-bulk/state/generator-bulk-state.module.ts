import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StoreModule, combineReducers, Action } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { EffectsModule } from '@ngrx/effects';

import { environment } from '../../../../../../environments/environment';

import { createGeneratorWizardReducer, GeneratorWizardState } from '../../_shared';
import { generatorBulkDocumentReducer, GeneratorBulkDocumentState } from './document';

import { GeneratorBulkEffects } from './generator-bulk.effects';
import { REDUCER_ID } from './constants';

export interface GeneratorBulkState {
  wizard: GeneratorWizardState;
  documents: GeneratorBulkDocumentState;
}

export const generatorBulkReducer = combineReducers<GeneratorBulkState, Action>(
  {
    wizard: createGeneratorWizardReducer(REDUCER_ID),
    documents: generatorBulkDocumentReducer
  }
);

@NgModule({
  imports: [
    CommonModule,

    StoreModule.forFeature(REDUCER_ID, generatorBulkReducer),
    EffectsModule.forFeature([GeneratorBulkEffects]),
    !environment.production ? StoreDevtoolsModule.instrument() : [],
  ]
})
export class GeneratorBulkStateModule { }
