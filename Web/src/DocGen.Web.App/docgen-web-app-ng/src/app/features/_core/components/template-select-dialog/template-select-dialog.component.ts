import { Component, OnInit, Input, Output, AfterViewInit, ViewChild, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { MatAutocompleteTrigger, MatDialogRef } from '@angular/material';

import { Template } from '../../../_core';

@Component({
  selector: 'app-core-template-select-dialog',
  templateUrl: './template-select-dialog.component.html',
  styleUrls: ['./template-select-dialog.component.scss']
})
export class TemplateSelectDialogComponent implements OnInit, AfterViewInit {
  @Input() templates: Template[];
  @Output() templateSelected = new EventEmitter<Template>();
  @Output() templateSelectionCancelled = new EventEmitter();

  @ViewChild(MatAutocompleteTrigger)
  templateNameAutocompleteTrigger: MatAutocompleteTrigger;

  form: FormGroup;
  templateNameControl: FormControl;
  options: Observable<string[]>;

  private templatesByName: Map<string, Template>;
  private isSelected = false;
  private isCancelled = false;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<TemplateSelectDialogComponent>
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

    this.dialogRef.beforeClose().subscribe(() => {
      if (!this.isCancelled && !this.isSelected) {
        this.onCancelClick();
      }
    });
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.templateNameControl.setValue('');
      this.templateNameAutocompleteTrigger.openPanel();
    }, 500);
  }

  onGoClick() {
    if (!this.isCancelled) {
      const templateName = this.templateNameControl.value;
      if (templateName) {
        this.templateSelected.emit(this.templatesByName.get(templateName));
        this.dialogRef.close();
      }
    }
  }

  onCancelClick() {
    this.isCancelled = true;
    this.templateSelectionCancelled.emit();
    this.dialogRef.close();
  }
}
