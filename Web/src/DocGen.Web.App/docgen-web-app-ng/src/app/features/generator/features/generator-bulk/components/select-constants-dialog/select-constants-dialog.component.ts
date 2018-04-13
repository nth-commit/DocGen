import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';

import { InputValueCollection, TemplateStep } from '../../../../../core';
import { State } from '../../../../../_shared';
import { DocumentUpdateConstantsAction, DocumentUpdateConstantsCancelAction } from '../../state/document';

@Component({
  templateUrl: './select-constants-dialog.component.html',
  styleUrls: ['./select-constants-dialog.component.scss']
})
export class SelectConstantsDialogComponent implements OnInit {

  steps$ = this.store.select(s => s.generatorBulk.documents.template.steps);
  values$ = this.store.select(s => s.generatorBulk.documents.lastCompletedDocument.values);

  selectedInputs$ = this.store
    .select(s => s.generatorBulk.documents.lastConstants)
    .filter(lastConstants => !!lastConstants)
    .map(lastConstants => Object.keys(lastConstants));

  selectedValues: InputValueCollection;

  constructor(
    private store: Store<State>
  ) { }

  ngOnInit() {
    this.selectedInputs$.first().subscribe(selectedInputs => this.updateSelectedInputs(selectedInputs));
  }

  updateSelectedInputs(selectedInputs: string[]) {
    this.values$
      .first()
      .subscribe(values => {
        const selectedValues: InputValueCollection = {};
        selectedInputs.forEach(inputId => {
          selectedValues[inputId] = values[inputId];
        });
        this.selectedValues = selectedValues;
      });
  }

  save() {
    this.store.dispatch(new DocumentUpdateConstantsAction(this.selectedValues));
  }

  cancel() {
    this.store.dispatch(new DocumentUpdateConstantsCancelAction());
  }
}
