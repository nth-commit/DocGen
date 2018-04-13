import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule, MatButtonModule, MatFormFieldModule, MatInputModule } from '@angular/material';

import { StoreModule, combineReducers, Action } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { StoreRouterConnectingModule, routerReducer } from '@ngrx/router-store';

import { environment } from '../../../environments/environment';

import { CoreState } from '../_core';
import { coreEventReducer } from './state/event';

import { TemplateService } from './services/templates/template.service';
import { LocalStorageDocumentService } from './services/documents/local-storage-document.service';
import { RouteChangeService } from './services/route-change/route-change.service';

import { TemplateSelectDialogComponent } from './components';

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
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule
  ],
  providers: [
    TemplateService,
    LocalStorageDocumentService,
    RouteChangeService
  ],
  declarations: [
    TemplateSelectDialogComponent
  ],
  entryComponents: [
    TemplateSelectDialogComponent
  ],
  exports: [
    TemplateSelectDialogComponent
  ]
})
export class CoreModule { }
