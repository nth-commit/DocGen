import { Component, OnInit, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { Observable, BehaviorSubject, Subscription } from 'rxjs';

import { TemplateStep, TemplateStepInput, InputValueCollection, InputValue, TemplateUtility } from '../../../core';

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
  private inputValuesByKeySub: Subscription;

  constructor() { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['templateStep']) {

      if (this.inputValuesByKeySub) {
        this.inputValuesByKeySub.unsubscribe();
      }

      this.inputValueSubjectsById = {};
      this.templateStep.inputs.forEach(i => {
        const inputId = TemplateUtility.getTemplateStepInputId(this.templateStep, i);
        const initialValue = this.value[inputId];
        this.inputValueSubjectsById[inputId] = new BehaviorSubject(initialValue);
      });

      const inputValueObservables = this.templateStep.inputs.map(i => {
        const inputId = this.getInputId(i);
        return this.inputValueSubjectsById[inputId]
          .asObservable()
          .map(v => ({ inputId, value: v }));
      });

      this.inputValuesByKeySub = Observable
        .combineLatest(inputValueObservables)
        .subscribe(kvps => {
          kvps.forEach(kvp => {
            this.value[kvp.inputId] = kvp.value;
          });
          this.valueChanges.emit(this.value);
        });
    }
  }

  getInputValue(input: TemplateStepInput) {
    return this.value[this.getInputId(input)];
  }

  setInputValue(input: TemplateStepInput, value: InputValue) {
    this.inputValueSubjectsById[this.getInputId(input)].next(value);
  }

  private getInputIdByKey(key: string): string {
    return this.getInputId(<TemplateStepInput>{ key });
  }

  private getInputId(input: TemplateStepInput): string {
    return TemplateUtility.getTemplateStepInputId(this.templateStep, input);
  }
}
