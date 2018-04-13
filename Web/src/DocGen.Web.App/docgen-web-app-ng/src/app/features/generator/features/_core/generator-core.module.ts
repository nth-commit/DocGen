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

import { WizardStepComponent } from './components/wizard-step/wizard-step.component';
import { WizardStepInputComponent } from './components/wizard-step-input/wizard-step-input.component';
import { WizardStepNavigationComponent } from './components/wizard-step-navigation/wizard-step-navigation.component';
import { DocumentValuesTableComponent } from './components/document-values-table/document-values-table.component';

import { GeneratorCoreRoutingModule } from './generator-core-routing.module';
import { TemplatedTextPipe } from './pipes/templated-text.pipe';

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

    GeneratorCoreRoutingModule
  ],
  declarations: [
    WizardStepComponent,
    WizardStepInputComponent,
    WizardStepNavigationComponent,
    DocumentValuesTableComponent,
    TemplatedTextPipe
  ],
  exports: [
    WizardStepComponent,
    WizardStepInputComponent,
    WizardStepNavigationComponent,
    DocumentValuesTableComponent,
    TemplatedTextPipe
  ]
})
export class GeneratorCoreModule { }
