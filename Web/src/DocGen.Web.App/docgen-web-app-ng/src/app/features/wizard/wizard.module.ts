import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { StoreModule } from '@ngrx/store';
import { reducer, metaReducers } from './reducers';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from '../../../environments/environment';
import { EffectsModule } from '@ngrx/effects';
import { WizardEffects } from './wizard.effects';

import { WizardRoutingModule } from './wizard-routing.module';
import { WizardPageComponent } from './pages/wizard-page/wizard-page.component';

@NgModule({
  imports: [
    CommonModule,
    HttpModule,
    WizardRoutingModule,
    StoreModule.forFeature('wizard', reducer, { metaReducers }),
    !environment.production ? StoreDevtoolsModule.instrument() : [],
    EffectsModule.forFeature([WizardEffects])
  ],
  declarations: [WizardPageComponent]
})
export class WizardModule { }
