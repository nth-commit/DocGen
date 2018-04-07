import { Component, OnInit, OnChanges, SimpleChanges, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { TemplateStepInput, TemplateStepInputType, InputValue } from '../../../../../core';

@Component({
  selector: 'app-generator-wizard-step-input',
  templateUrl: './wizard-step-input.component.html',
  styleUrls: ['./wizard-step-input.component.scss']
})
export class WizardStepInputComponent implements OnInit, OnChanges {
  @Input() stepInput: TemplateStepInput;
  @Input() value: InputValue;
  @Output() valueChanges = new EventEmitter<InputValue>();

  TemplateStepInputType = TemplateStepInputType;
  form: FormGroup;

  constructor(
    private fb: FormBuilder
  ) { }

  ngOnChanges(changes: SimpleChanges) {
    if (this.form && changes.value && changes.value.currentValue !== changes.value.previousValue) {
      const controls = this.form.controls;
      const controlKey = Object.keys(controls)[0];
      controls[controlKey].setValue(changes.value.currentValue);
    }
  }

  ngOnInit() {
    const { fb, stepInput } = this;

    if (stepInput.type === TemplateStepInputType.Text) {
      this.form = fb.group({
        text: fb.control(this.value)
      });

      this.form.valueChanges.subscribe(v => {
        if (v.text) {
          this.valueChanges.emit(v.text);
        } else {
          this.valueChanges.emit(null);
        }
      });
    } else if (stepInput.type === TemplateStepInputType.Radio) {
      this.form = fb.group({
        radio: fb.control(this.value)
      });

      this.form.valueChanges.subscribe(v => {
        if (v.radio) {
          this.valueChanges.emit(v.radio);
        } else {
          this.valueChanges.emit(null);
        }
      });
    } else if (stepInput.type === TemplateStepInputType.Checkbox) {
      const yesControl = fb.control(this.value === true);
      const noControl = fb.control(this.value === false);

      this.form = fb.group({
        yes: yesControl,
        no: noControl
      });

      yesControl.valueChanges.subscribe(v => {
        if (v === true && noControl.value === true) {
          noControl.setValue(false);
        }
      });

      noControl.valueChanges.subscribe(v => {
        if (v === true && yesControl.value === true) {
          yesControl.setValue(false);
        }
      });

      this.form.valueChanges.subscribe(v => {
        if (v.yes === true || v.no === true) {
          this.valueChanges.emit(v.yes);
        } else {
          this.valueChanges.emit(null);
        }
      });
    } else {
      throw new Error('Unknown TemplateStepInputType');
    }
  }
}
