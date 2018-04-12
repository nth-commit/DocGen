import { Component, OnInit, AfterViewInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';
import { Store } from '@ngrx/store';
import { Actions } from '@ngrx/effects';

import { Observable } from 'rxjs/Observable';

import { State } from '../../../_shared';
import { WizardActionsTypes, WizardBeginAction } from '../_shared';

import { REDUCER_ID, DocumentActionsTypes, DocumentPublishDraftAction, DocumentUpdateConstantsAction } from './state';
import { WizardDialogComponent } from './components/wizard-dialog/wizard-dialog.component';
import { DocumentValueSelectorService } from './services/document-value-selector/document-value-selector.service';

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
    private matDialog: MatDialog,
    private documentValueSelectorService: DocumentValueSelectorService
  ) { }

  ngOnInit() {

    // this.store.select(s => s.generatorBulk.documents)
    //   .first()
    //   .subscribe(documentState => {
    //     if (!documentState.draftDocuments.length && !documentState.draftDocuments.length) {
    //       setTimeout(() => {
    //         this.openWizardDialog();
    //       }, 500);
    //     }
    //   });

    this.actions$
      .ofType(DocumentActionsTypes.PUBLISH_DRAFT)
      .withLatestFrom(this.store)
      .subscribe(([action, state]: [DocumentPublishDraftAction, State]) => {
      // .subscribe((action: DocumentPublishDraftAction) => {
        // if (action.payload.repeat) {
        //   this.wizardDialogRef.afterClosed().subscribe(() => {

        //     const constantsSetPromise = Promise.resolve();
        //     // if (!state.generatorBulk.documents.constants || true) {
        //     if (true) {
        //       // constantsSetPromise = this.documentValueSelectorService.selectValues();
        //     }

        //     // constantsSetPromise.then(() => this.openWizardDialog());
        //   });
        // }

        // this.wizardDialogRef.close();
      });
  }

  private setDocumentUpdateConstants(): Promise<void> {
    // return this.matDialog.open(DocumentValueSelectionDialog).afterClosed().toPromise();
    return Promise.resolve();
  }
}
