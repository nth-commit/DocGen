import { Component, OnInit, AfterViewInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';
import { Store } from '@ngrx/store';
import { Actions } from '@ngrx/effects';

import { Observable } from 'rxjs/Observable';

import { State } from '../../../_shared';
import { WizardActionsTypes, WizardBeginAction } from '../_shared';

import { REDUCER_ID, DocumentActionsTypes, DocumentPublishDraftAction } from './state';
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

  private wizardDialogRef: MatDialogRef<WizardDialogComponent>;

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
          setTimeout(() => {
            this.openWizardDialog();
          }, 500);
        }
      });

    this.actions$
      .ofType(DocumentActionsTypes.PUBLISH_DRAFT)
      .subscribe((action: DocumentPublishDraftAction) => {
        if (action.payload.repeat) {
          this.wizardDialogRef.afterClosed().subscribe(() => {
            // TODO: Open field selector for wizard
            this.openWizardDialog();
          });
        }
        this.wizardDialogRef.close();
      });
  }

  private openWizardDialog() {
    if (this.wizardDialogRef) {
      throw new Error('Wizard dialog ref is already open');
    }

    this.store
      .select(s => s.generatorBulk.documents.template)
      .first()
      .subscribe(template => {
        this.store.dispatch(new WizardBeginAction(REDUCER_ID, { template }));

        this.wizardDialogRef = this.matDialog.open(WizardDialogComponent, {
          width: '550px',
          height: '1px',
          minHeight: '700px'
        });

        this.wizardDialogRef.afterClosed().first().subscribe(() => {
          this.wizardDialogRef = null;
        });
      });
  }
}
