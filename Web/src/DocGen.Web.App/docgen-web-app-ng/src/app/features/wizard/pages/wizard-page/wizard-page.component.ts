import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';

import { Observable } from 'rxjs';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/first';

import { Template } from '../../../core';

import { InputValueCollection } from '../../models';
import { State, Begin, UpdateValues, NextStep, PreviousStep, Complete } from '../../reducers';

@Component({
  selector: 'app-wizard-page',
  templateUrl: './wizard-page.component.html',
  styleUrls: ['./wizard-page.component.scss']
})
export class WizardPageComponent implements OnInit {

  template$ = this.store.select(s => s.wizard.template);
  templateStep$ = this.store.select(s => s.wizard.currentStep);
  hasNextStep$ = this.store.select(s => s.wizard.hasNextStep);
  hasPreviousStep$ = this.store.select(s => s.wizard.hasPreviousStep);
  currentStepValid$ = this.store.select(s => s.wizard.currentStepValid);
  currentValues$ = this.store.select(s => s.wizard.currentValues);

  constructor(
    private route: ActivatedRoute,
    private store: Store<State>
  ) { }

  ngOnInit() {
  }

  updateStepValues(values: InputValueCollection) {
    this.store.dispatch(new UpdateValues(values));
  }

  onNextStepClick() {
    this.currentStepValid$
      .first()
      .subscribe(currentStepValid => {
        if (currentStepValid) {
          this.store.dispatch(new NextStep());
        } else {
          // TODO: Trigger validation?
        }
    });
  }

  onPreviousStepClick() {
    this.store.dispatch(new PreviousStep());
  }

  onCompleteClick() {
    this.store.dispatch(new Complete());
  }
}
