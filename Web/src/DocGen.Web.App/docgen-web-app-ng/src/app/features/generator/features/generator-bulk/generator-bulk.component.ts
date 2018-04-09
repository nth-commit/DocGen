import { Component, OnInit, AfterViewInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Store } from '@ngrx/store';
import { Actions } from '@ngrx/effects';

import { Observable } from 'rxjs/Observable';

import { State, CoreState } from '../../../_shared';
import { WizardActionsTypes, WizardBeginAction } from '../_shared';

import { REDUCER_ID, GeneratorBulkState } from './state';
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

  x$ = this.store.select(s => s.generatorBulk.documents);

  constructor(
    private store: Store<State>,
    private actions$: Actions,
    private matDialog: MatDialog
  ) { }

  ngOnInit() {
    this.store.select(s => s.generatorBulk.documents)
      .first()
      .subscribe(documentState => {
        if (!documentState.draftDocuments.length && !documentState.draftDocuments.length) {
          this.store.dispatch(new WizardBeginAction(REDUCER_ID, {
            template: documentState.template
          }));

          setTimeout(() => {
            this.matDialog.open(WizardDialogComponent, {
              width: '550px',
              height: '1px',
              minHeight: '700px'
            });
          }, 500);
        }
      });
  }
}
