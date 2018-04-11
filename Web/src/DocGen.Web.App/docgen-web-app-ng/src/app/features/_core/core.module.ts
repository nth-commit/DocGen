import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { StoreModule, combineReducers, Action } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { StoreRouterConnectingModule, routerReducer } from '@ngrx/router-store';

import { environment } from '../../../environments/environment';

import { CoreState } from '../_shared';
import { coreEventReducer } from './state/event';

export const coreReducer = combineReducers<CoreState, Action>({
  event: coreEventReducer,
  router: routerReducer
});

@NgModule({
  imports: [
    CommonModule,
    StoreModule.forFeature('core', coreReducer),
    RouterModule,
    StoreRouterConnectingModule.forRoot({ stateKey: 'router' }),
    !environment.production ? StoreDevtoolsModule.instrument() : [],
  ],
  declarations: []
})
export class CoreModule { }
