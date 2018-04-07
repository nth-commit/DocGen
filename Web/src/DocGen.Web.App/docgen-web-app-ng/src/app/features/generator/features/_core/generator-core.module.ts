import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import {
  MatInputModule,
  MatRadioModule,
  MatCheckboxModule,
  MatButtonModule,
  MatCardModule,
  MatSnackBarModule,
  MatProgressBarModule
} from '@angular/material';

import { GeneratorCoreRoutingModule } from './generator-core-routing.module';

import { WizardStepComponent } from './components/wizard-step/wizard-step.component';
import { WizardStepInputComponent } from './components/wizard-step-input/wizard-step-input.component';
import { WizardStepNavigationComponent } from './components/wizard-step-navigation/wizard-step-navigation.component';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,

    MatInputModule,
    MatRadioModule,
    MatCheckboxModule,
    MatButtonModule,
    MatCardModule,
    MatSnackBarModule,
    MatProgressBarModule,

    GeneratorCoreRoutingModule
  ],
  declarations: [
    WizardStepComponent,
    WizardStepInputComponent,
    WizardStepNavigationComponent
  ],
  exports: [
    WizardStepComponent,
    WizardStepInputComponent,
    WizardStepNavigationComponent
  ]
})
export class GeneratorCoreModule { }
