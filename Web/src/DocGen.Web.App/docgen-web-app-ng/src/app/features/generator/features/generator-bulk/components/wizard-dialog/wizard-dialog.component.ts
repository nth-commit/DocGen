import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import { InputValueCollection, TemplateStep } from '../../../../../_core';
import { State, GeneratorWizardState } from '../../../../../_core';
import { WizardUpdateValuesAction, WizardNextAction, WizardPreviousAction } from '../../../_core';
import { REDUCER_ID } from '../../state/constants';
import { DocumentPublishDocumentAction } from '../../state/document';

@Component({
  templateUrl: './wizard-dialog.component.html',
  styleUrls: ['./wizard-dialog.component.scss']
})
export class WizardDialogComponent implements OnInit {

  step$ = this.selectFromWizard(w => w.step);
  steps$ = this.selectFromWizard(w => w.steps);
  allSteps$ = this.selectFromWizard(w => w.allSteps);
  hasPreviousStep$ = this.selectFromWizard(w => w.hasPreviousStep);
  hasNextStep$ = this.selectFromWizard(w => w.hasNextStep);
  stepValid$ = this.selectFromWizard(w => w.stepValid);
  stepValues$ = this.selectFromWizard(w => w.stepValues);
  progress$ = this.selectFromWizard(w => w.progress);
  completed$ = this.selectFromWizard(w => w.completed);
  values$ = this.selectFromWizard(w => w.values);
  hasConstants$ = this.selectFromWizard(w => !!w.presets);

  repeatCreation = true;
  clearConstants = false;

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
          this.store.dispatch(new WizardNextAction(REDUCER_ID));
        }
      });
  }

  onPreviousStepClick() {
    this.completed$
      .first()
      .subscribe(completed => {
        this.store.dispatch(new WizardPreviousAction(REDUCER_ID));
      });
  }

  onCompleteClick() {
    this.store
      .select(s => s.generatorBulk.wizard.id)
      .first()
      .subscribe(id => {
        this.store.dispatch(new DocumentPublishDocumentAction({
          id: id,
          repeat: this.repeatCreation,
          clearConstants: this.clearConstants
        }));
      });
  }

  private selectFromWizard<T>(func: (wizard: GeneratorWizardState) => T): Observable<T> {
    return this.store.select(s => func(s.generatorBulk.wizard));
  }

}
