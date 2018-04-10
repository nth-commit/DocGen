import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StoreModule, combineReducers, Action, compose } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { EffectsModule } from '@ngrx/effects';

import { environment } from '../../../../../../environments/environment';
import { CoreModule } from '../../../../_core';
import { resetMetaReducer, GeneratorWizardState, GeneratorBulkState } from '../../../../_shared';

import { createGeneratorWizardReducer } from '../../_shared';
import { generatorBulkDocumentReducer } from './document';

import { GeneratorBulkStateEffects } from './generator-bulk-state.effects';
import { REDUCER_ID } from './constants';

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
    EffectsModule.forFeature([GeneratorBulkStateEffects]),
    !environment.production ? StoreDevtoolsModule.instrument() : [],

    CoreModule
  ]
})
export class GeneratorBulkStateModule { }
