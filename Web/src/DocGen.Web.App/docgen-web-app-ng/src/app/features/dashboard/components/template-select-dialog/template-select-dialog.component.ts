import { Component, OnInit, Input, Output, AfterViewInit, ViewChild, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { MatAutocompleteTrigger } from '@angular/material';

import { Template } from '../../../core';

@Component({
  selector: 'app-template-select-dialog',
  templateUrl: './template-select-dialog.component.html',
  styleUrls: ['./template-select-dialog.component.scss']
})
export class TemplateSelectDialogComponent implements OnInit, AfterViewInit {
  @Input() templates: Template[];
  @Output() templateSelected = new EventEmitter<Template>();

  @ViewChild(MatAutocompleteTrigger)
  templateNameAutocompleteTrigger: MatAutocompleteTrigger;

  form: FormGroup;
  templateNameControl: FormControl;
  options: Observable<string[]>;

  private templatesByName: Map<string, Template>;

  constructor(
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.templatesByName = Map.fromArray(this.templates, t => t.name, t => t);

    this.templateNameControl = this.fb.control('');
    this.form = this.fb.group({ templateName: this.templateNameControl });

    this.options = this.templateNameControl.valueChanges.map((n1: string) => {
      const result = Array.from(this.templatesByName.keys()).filter(n2 => {
        const index = n2.toLowerCase().indexOf(n1.toLowerCase());
        return index > -1;
      });
      return result;
    });
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.templateNameControl.setValue('');
      this.templateNameAutocompleteTrigger.openPanel();
    }, 500);
  }
}
