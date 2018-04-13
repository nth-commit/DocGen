import { Component, OnInit, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { MatCheckboxChange } from '@angular/material';

import { InputValueCollection, TemplateStep, InputValueCollectionUtility, TemplateUtility } from '../../../../../_core';
import { ENGINE_METHOD_DIGESTS } from 'constants';

@Component({
  selector: 'app-generator-bulk-document-value-select-table',
  templateUrl: './document-value-select-table.component.html',
  styleUrls: ['./document-value-select-table.component.scss']
})
export class DocumentValueSelectTableComponent implements OnInit, OnChanges {
  @Input() steps: TemplateStep[];
  @Input() values: InputValueCollection;
  @Input() selectedInputs: string[];
  @Output() selectedInputsChanged = new EventEmitter<string[]>();

  displayedColumns = ['selected', 'name', 'value'];
  inputs: { id: string, name: string, value: string }[];
  inputSelectedById: { [inputName: string]: boolean };

  constructor() { }

  ngOnInit() {
  }

  ngOnChanges() {
    if (!this.steps || !this.values) {
      this.inputs = [];
    } else {

    const inputNameById = this.steps
      .selectMany(s => s.inputs.map(i => ({
        name: TemplateUtility.getInputName(s, i),
        id: i.id
      })))
      .toMap(
        x => x.id,
        x => x.name
      );

    this.inputs = Object.keys(this.values)
      .filter(k => {
        const value = this.values[k];
        return value !== null && value !== undefined;
      })
      .map(k => {
        const inputName = inputNameById.get(k);
        return {
          id: k,
          name: inputName,
          value: InputValueCollectionUtility.getInputString(this.values[k])
        };
      })
      .filter(x => x.name);
    }

    this.selectedInputs = this.selectedInputs || [];

    this.inputSelectedById = {};
    this.inputs.forEach(i => {
      this.inputSelectedById[i.id] = this.selectedInputs.indexOf(i.id) > -1;
    });
  }

  onChange(inputId: string, change: MatCheckboxChange) {
    this.inputSelectedById[inputId] = change.checked;

    this.selectedInputs = this.inputs
      .filter(i => this.inputSelectedById[i.id])
      .map(i => i.id);

    this.selectedInputsChanged.emit(this.selectedInputs);
  }

}
