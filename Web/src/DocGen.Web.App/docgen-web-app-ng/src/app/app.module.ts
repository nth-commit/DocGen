import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { StoreModule } from '@ngrx/store';
import { reducers, metaReducers } from './reducers';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from '../environments/environment';
import { EffectsModule } from '@ngrx/effects';
import { AppEffects } from './app.effects';

import { DashboardModule } from './features/dashboard';
import { WizardModule } from './features/wizard';
import { DocumentViewerModule } from './features/document-viewer';
import { GeneratorModule } from './features/generator';

import { NotFoundPageComponent } from './pages/not-found-page/not-found-page.component';

@NgModule({
  declarations: [
    AppComponent,
    NotFoundPageComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot([
      // {
      //   path: '**',
      //   component: NotFoundPageComponent
      // }
    ]),

    StoreModule.forRoot(reducers, { metaReducers }),
    !environment.production ? StoreDevtoolsModule.instrument() : [],
    EffectsModule.forRoot([AppEffects]),

    DashboardModule,
    WizardModule,
    DocumentViewerModule,
    GeneratorModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
