import { Component, OnInit, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';

import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Subscription } from 'rxjs/Subscription';
import 'rxjs/add/observable/combineLatest';

import { TemplateStep, TemplateStepInput, InputValueCollection, InputValue, InputValueCollectionUtility } from '../../../core';

@Component({
  selector: 'app-wizard-template-step',
  templateUrl: './template-step.component.html',
  styleUrls: ['./template-step.component.scss']
})
export class TemplateStepComponent implements OnInit, OnChanges {
  @Input() templateStep: TemplateStep;
  @Input() value: InputValueCollection = {};
  @Output() valueChanges = new EventEmitter<InputValueCollection>();

  private inputValueSubjectsById: { [key: string]: BehaviorSubject<InputValue> };
  private inputValueObservables: Observable<{ id: string, value: InputValue }>[];
  private inputValuesByKeySub: Subscription;

  private valueArray: InputValue[];

  constructor() { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    const valueChange = changes.value;
    if (!InputValueCollectionUtility.isEqual(valueChange.currentValue, valueChange.previousValue)) {
      this.valueArray = [];
      this.templateStep.inputs.forEach((input, i) => {
        this.valueArray[i] = this.value[input.id];
      });
    }
  }

  onInputValueChanges(input: TemplateStepInput, index: number, value: InputValue) {
    this.valueArray[index] = value;
    this.value[input.id] = value;

    setTimeout(() => {
      this.valueChanges.emit(this.value);
    }, 50);
  }
}
