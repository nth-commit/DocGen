import { Component, OnInit, AfterViewInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Store } from '@ngrx/store';
import { Actions } from '@ngrx/effects';

import { Observable } from 'rxjs/Observable';

import { State, CoreState } from '../../../_shared';
import { WizardActionsTypes } from '../_shared';

import { REDUCER_ID, GeneratorBulkState, BeginDraft } from './state';
import { WizardDialogComponent } from './components/wizard-dialog/wizard-dialog.component';

@Component({
  selector: 'app-generator-bulk',
  templateUrl: './generator-bulk.component.html',
  styleUrls: ['./generator-bulk.component.scss']
})
export class GeneratorBulkComponent implements OnInit {

  template$ = this.store.select(s => s.generatorBulk.documents.template);
  completedDocuments$ = this.store.select(s => s.generatorBulk.documents.completedDocuments);
  draftDocuments$ = this.store.select(s => s.generatorBulk.documents.draftDocuments);

  constructor(
    private store: Store<State>,
    private actions$: Actions,
    private matDialog: MatDialog
  ) { }

  ngOnInit() {
    Observable.combineLatest(this.completedDocuments$, this.draftDocuments$)
      .first()
      .subscribe(([completedDocuments, draftDocuments]) => {
        if (!completedDocuments.length && !draftDocuments.length) {
          this.store.dispatch(new BeginDraft());
        }
      });

    this.store
      .select(s => s.core.event)
      .filter(e =>
        e.actionType === WizardActionsTypes.BEGIN &&
        e.reducerId === REDUCER_ID)
      .debounceTime(200)
      .subscribe(e => {
        this.matDialog.open(WizardDialogComponent);
      });
  }
}
