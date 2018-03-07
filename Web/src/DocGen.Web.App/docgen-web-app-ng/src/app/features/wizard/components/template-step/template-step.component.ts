import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';

import { TemplateStep } from '../../../core';

@Component({
  selector: 'app-wizard-template-step',
  templateUrl: './template-step.component.html',
  styleUrls: ['./template-step.component.scss']
})
export class TemplateStepComponent implements OnInit {
  @Input() templateStep: TemplateStep;
  @Input() value: { [key: string]: string | boolean } = {};
  @Output() valueChanges = new EventEmitter<{ [key: string]: string | boolean }>();

  private inputValueSubjectsByKey: { [key: string]: BehaviorSubject<string | boolean> } = {};

  constructor() { }

  ngOnInit() {
    this.templateStep.inputs.forEach(i => {
      this.inputValueSubjectsByKey[i.key] = new BehaviorSubject(this.value[i.key]);
    });

    const inputValueObservables = this.templateStep.inputs.map(i =>
      this.inputValueSubjectsByKey[i.key].asObservable().map(v =>
        ({ key: i.key, value: v })));

    Observable
      .combineLatest(inputValueObservables)
      .subscribe(kvps => {
        let isValid = true;
        kvps.forEach(kvp => {
          this.value[kvp.key] = kvp.value;
          isValid = isValid && !!kvp.value;
        });

        if (isValid) {
          this.valueChanges.emit(this.value);
        } else {
          this.valueChanges.emit(null);
        }
      });
  }

  updateInputValue(key: string, value: string | boolean) {
    this.inputValueSubjectsByKey[key].next(value);
  }
}
