import { Component, OnInit, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';

import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Subscription } from 'rxjs/Subscription';
import 'rxjs/add/observable/combineLatest';

import { TemplateStep, TemplateStepInput, InputValueCollection, InputValue, InputValueCollectionUtility } from '../../../../../_core';

@Component({
  selector: 'app-generator-wizard-step',
  templateUrl: './wizard-step.component.html',
  styleUrls: ['./wizard-step.component.scss']
})
export class WizardStepComponent implements OnInit, OnChanges {
  @Input() step: TemplateStep;
  @Input() values: InputValueCollection = {};
  @Input() allValues: InputValueCollection;
  @Output() valueChanges = new EventEmitter<InputValueCollection>();

  valueArray: InputValue[];

  private inputValueSubjectsById: { [key: string]: BehaviorSubject<InputValue> };
  private inputValueObservables: Observable<{ id: string, value: InputValue }>[];
  private inputValuesByKeySub: Subscription;

  constructor() { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    const valuesChange = changes.values;
    if (!InputValueCollectionUtility.isEqual(valuesChange.currentValue, valuesChange.previousValue)) {
      this.valueArray = [];
      this.step.inputs.forEach((input, i) => {
        this.valueArray[i] = this.values[input.id];
      });
    }
  }

  onInputValueChanges(input: TemplateStepInput, index: number, value: InputValue) {
    this.valueArray[index] = value;
    this.values[input.id] = value;

    setTimeout(() => {
      this.valueChanges.emit(this.values);
    }, 50);
  }
}
