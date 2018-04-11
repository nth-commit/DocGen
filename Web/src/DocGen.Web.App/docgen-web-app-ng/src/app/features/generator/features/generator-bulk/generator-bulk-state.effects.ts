import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef, MatDialogConfig } from '@angular/material';
import { Store, Action } from '@ngrx/store';
import { Actions, Effect } from '@ngrx/effects';
import { RouterReducerState, ROUTER_NAVIGATION } from '@ngrx/router-store';
import { Observable } from 'rxjs/Observable';

import { State, AppAction, GeneratorBulkLayoutDialogState } from '../../../_shared';
import { WizardActionsTypes, WizardBeginAction, WizardResetAction } from '../_shared';
import { DocumentActionsTypes, DocumentBeginAction, DocumentUpdateDraftAction } from './state/document';
import {
  LayoutActionTypes,
  LayoutOpenDialogBeginAction,
  LayoutOpenDialogEndAction,
  LayoutCloseDialogBeginAction,
  LayoutCloseDialogEndAction
} from './state/layout';
import { REDUCER_ID } from './state/constants';

import { WizardDialogComponent } from './components/wizard-dialog/wizard-dialog.component';
import { withLatestFrom } from 'rxjs/operator/withLatestFrom';

@Injectable()
export class GeneratorBulkStateEffects {

  constructor(
    private actions$: Actions<AppAction>,
    private store: Store<State>,
    private matDialog: MatDialog
  ) { }

  @Effect() onDocumentBegin_triggerWizardBegin$ = this.actions$
    .ofType(DocumentActionsTypes.BEGIN)
    .withLatestFrom(this.store)
    .switchMap(([action, state]) => {
      const { documents } = state.generatorBulk;

      if (!documents.draftDocuments.length && !documents.draftDocuments.length) {
        return Observable.of(new WizardBeginAction(REDUCER_ID, {
          template: documents.template,
          presets: documents.constants
        }));
      }

      return Observable.empty();
    });

  @Effect() onWizardBegin_triggerLayoutOpenDialogBegin$ = this.actions$
    .ofType(DocumentActionsTypes.BEGIN)
    .withLatestFrom(this.store)
    .map(() => new LayoutOpenDialogBeginAction({
      dialog: 'wizard'
    }));

  @Effect() onLayoutOpenDialogBegin_triggerLayoutOpenDialogEnd$ = this.actions$
    .ofType(LayoutActionTypes.OPEN_DIALOG_BEGIN)
    .switchMap((action: LayoutOpenDialogBeginAction) => {

      let dialogOpenArgs: {
        componentType: any,
        config: MatDialogConfig
      } = null;

      if (action.payload.dialog === 'wizard') {
        dialogOpenArgs = {
          componentType: WizardDialogComponent,
          config: {
            width: '550px',
            height: '1px',
            minHeight: '700px'
          }
        };
      } else {
        throw new Error('Unknown dialog name');
      }

      return Observable
        .timer(500)
        .switchMap(() => {
          const dialogRef = this.matDialog.open(dialogOpenArgs.componentType, dialogOpenArgs.config);

          dialogRef.afterClosed()
            .switchMap(() => this.store.select(s => s.generatorBulk.layout))
            .first()
            .subscribe(layout => {
              if (layout.dialogState !== GeneratorBulkLayoutDialogState.Closing) {
                this.store.dispatch(new LayoutCloseDialogBeginAction());
              }
            });

          return dialogRef.afterOpen().map(() => new LayoutOpenDialogEndAction({ dialogRef }));
        });
    });

  @Effect() onWizardUpdateValues_triggerDocumentUpdateDraft$ = this.actions$
    .ofType(WizardActionsTypes.BEGIN, WizardActionsTypes.UPDATE_VALUES)
    .filter(a => a.reducerId === REDUCER_ID)
    .debounceTime(500)
    .withLatestFrom(this.store)
    .map(([action, state]) => new DocumentUpdateDraftAction(state.generatorBulk.wizard));

  @Effect() onDocumentPublishDraft_triggerLayoutCloseDialogBegin$ = this.actions$
    .ofType(DocumentActionsTypes.PUBLISH_DRAFT)
    .map(() => new LayoutCloseDialogBeginAction());

  @Effect() onLayouCloseDialogEnd_triggerWizardReset = this.actions$
    .ofType(LayoutActionTypes.CLOSE_DIALOG_END)
    .map(() => new WizardResetAction(REDUCER_ID));
}
