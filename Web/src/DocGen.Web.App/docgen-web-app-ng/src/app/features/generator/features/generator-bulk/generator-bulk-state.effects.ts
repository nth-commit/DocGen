import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Actions, Effect } from '@ngrx/effects';
import { Observable } from 'rxjs/Observable';

import { State, AppAction } from '../../../_shared';
import { WizardActionsTypes, WizardBeginAction, WizardResetAction } from '../_shared';
import { DocumentActionsTypes, DocumentBeginAction, DocumentUpdateDraftAction } from './state/document';
import { LayoutActionTypes, LayoutOpenDialogBeginAction, LayoutCloseDialogBeginAction } from './state/layout';
import { REDUCER_ID } from './state/constants';

import { WizardDialogComponent } from './components/wizard-dialog/wizard-dialog.component';
import { withLatestFrom } from 'rxjs/operator/withLatestFrom';

@Injectable()
export class GeneratorBulkStateEffects {

  constructor(
    private actions$: Actions<AppAction>,
    private store: Store<State>
  ) { }

  @Effect() onDocumentBegin_wizardBegin$ = this.actions$
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

  @Effect() onWizardBegin_layoutOpenDialogBegin$ = this.actions$
    .ofType(DocumentActionsTypes.BEGIN)
    .withLatestFrom(this.store)
    .map(() => new LayoutOpenDialogBeginAction({
      dialog: 'wizard'
    }));

  @Effect() onWizardUpdateValues_documentUpdateDraft$ = this.actions$
    .ofType(WizardActionsTypes.BEGIN, WizardActionsTypes.UPDATE_VALUES)
    .filter(a => a.reducerId === REDUCER_ID)
    .debounceTime(500)
    .withLatestFrom(this.store)
    .map(([action, state]) => new DocumentUpdateDraftAction(state.generatorBulk.wizard));

  @Effect() onDocumentPublishDraft_layoutCloseDialogBegin$ = this.actions$
    .ofType(DocumentActionsTypes.PUBLISH_DRAFT)
    .map(() => new LayoutCloseDialogBeginAction());

  @Effect() onLayoutCloseDialogEnd_wizardReset$ = this.actions$
    .ofType(LayoutActionTypes.CLOSE_DIALOG_END)
    .map(() => new WizardResetAction(REDUCER_ID));
}
