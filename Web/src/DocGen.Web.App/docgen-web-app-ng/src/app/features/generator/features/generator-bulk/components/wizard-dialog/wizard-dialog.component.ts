import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import { InputValueCollection, TemplateStep } from '../../../../../core';
import { State } from '../../../../../_shared';
import {
  GeneratorWizardState,
  WizardUpdateValuesAction,
  WizardNextStepAction,
  WizardPreviousStepAction,
  WizardCompleteStepAction
 } from '../../../_shared';
import { REDUCER_ID } from '../../state';

@Component({
  selector: 'app-generator-wizard-dialog',
  templateUrl: './wizard-dialog.component.html',
  styleUrls: ['./wizard-dialog.component.scss']
})
export class WizardDialogComponent implements OnInit {

  step$ = this.selectFromWizard(w => w.step);
  steps$ = this.selectFromWizard(w => w.steps);
  hasPreviousStep$ = this.selectFromWizard(w => w.hasPreviousStep);
  hasNextStep$ = this.selectFromWizard(w => w.hasNextStep);
  stepValid$ = this.selectFromWizard(w => w.stepValid);
  stepValues$ = this.selectFromWizard(w => w.stepValues);
  progress$ = this.selectFromWizard(w => w.progress);
  completed$ = this.selectFromWizard(w => w.completed);

  // hasNextStep$ = Observable.combineLatest(this.selectFromWizard(w => w.hasNextStep), this.completed$)
  //   .map(([hasNextStep, completed]) => hasNextStep || !completed);

  constructor(
    private store: Store<State>
  ) { }

  ngOnInit() {
  }

  updateStepValues(values: InputValueCollection) {
    this.store.dispatch(new WizardUpdateValuesAction(REDUCER_ID, values));
  }

  onNextStepClick() {
    Observable.combineLatest(this.stepValid$, this.hasNextStep$)
      .first()
      .subscribe(([stepValid, hasNextStep]) => {
        if (stepValid) {
          if (hasNextStep) {
            this.store.dispatch(new WizardNextStepAction(REDUCER_ID));
          } else {
            this.store.dispatch(new WizardCompleteStepAction(REDUCER_ID));
          }
        }
      });
  }

  onPreviousStepClick() {
    this.store.dispatch(new WizardPreviousStepAction(REDUCER_ID));
  }

  onCompleteClick() {
    this.store.dispatch(new WizardCompleteStepAction(REDUCER_ID));
  }

  private selectFromWizard<T>(func: (wizard: GeneratorWizardState) => T): Observable<T> {
    return this.store.select(s => func(s.generatorBulk.wizard));
  }

}
