import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StoreModule, combineReducers, Action } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { EffectsModule } from '@ngrx/effects';

import { environment } from '../../../../environments/environment';
import { CoreState } from '../../_shared';

import { coreEventReducer } from './event';

export const REDUCER_ID = 'core';

export const coreReducer = combineReducers<CoreState, Action>({
  event: coreEventReducer
});

@NgModule({
  imports: [
    CommonModule,
    StoreModule.forFeature(REDUCER_ID, coreReducer),
    !environment.production ? StoreDevtoolsModule.instrument() : [],
  ],
  declarations: []
})
export class CoreStateModule { }
