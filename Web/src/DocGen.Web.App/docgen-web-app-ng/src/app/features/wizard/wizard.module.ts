import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  MatInputModule, MatRadioModule, MatCheckboxModule,
  MatButtonModule,
  MatCardModule,
  MatSnackBarModule,
  MatProgressBarModule
} from '@angular/material';
import { StoreModule } from '@ngrx/store';
import { reducer, metaReducers } from './reducers';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from '../../../environments/environment';
import { EffectsModule } from '@ngrx/effects';
import { WizardEffects } from './wizard.effects';
import { CoreModule } from '../core';

import { WizardRoutingModule } from './wizard-routing.module';

import { WizardPageComponent } from './pages/wizard-page/wizard-page.component';
import { TemplateStepInputComponent } from './components/template-step-input/template-step-input.component';
import { TemplateStepComponent } from './components/template-step/template-step.component';
import { TemplateStepNavigationComponent } from './components/template-step-navigation/template-step-navigation.component';

@NgModule({
  imports: [
    CommonModule,
    HttpModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatInputModule, MatRadioModule, MatCheckboxModule, MatButtonModule, MatCardModule, MatSnackBarModule, MatProgressBarModule,
    WizardRoutingModule,
    StoreModule.forFeature('wizard', reducer, { metaReducers }),
    !environment.production ? StoreDevtoolsModule.instrument() : [],
    EffectsModule.forFeature([WizardEffects]),
    CoreModule
  ],
  declarations: [WizardPageComponent, TemplateStepInputComponent, TemplateStepComponent, TemplateStepNavigationComponent],
  providers: []
})
export class WizardModule { }
