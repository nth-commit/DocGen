import { Component, OnInit, OnChanges, Input } from '@angular/core';

import { TemplateStep, TemplateUtility, InputValueCollection, InputValueCollectionUtility } from '../../../../../core';

@Component({
  selector: 'app-generator-document-values-table',
  templateUrl: './document-values-table.component.html',
  styleUrls: ['./document-values-table.component.scss']
})
export class DocumentValuesTableComponent implements OnInit, OnChanges {
  @Input() steps: TemplateStep[];
  @Input() values: InputValueCollection;

  inputs: { name: string, value: string }[];

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
      .filter(k => this.values[k])
      .map(k => {
        const inputName = inputNameById.get(k);
        return {
          name: inputName,
          value: InputValueCollectionUtility.getInputString(this.values[k])
        };
      });
    }
  }
}
