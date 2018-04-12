import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Actions, Effect } from '@ngrx/effects';
import { Observable } from 'rxjs/Observable';

import { State, AppAction, GeneratorBulkDocumentRepeatState } from '../../../../_shared';
import { WizardActionsTypes, WizardBeginAction, WizardResetAction } from '../../_shared';
import { DocumentActionsTypes, DocumentBeginAction, DocumentUpdateDraftAction, DocumentUpdateConstantsBeginAction } from './document';
import { LayoutActionTypes, LayoutDialogAction, LayoutOpenDialogBeginAction, LayoutCloseDialogBeginAction } from './layout';
import { REDUCER_ID } from './constants';

@Injectable()
export class GeneratorBulkEffects {

  constructor(
    private actions$: Actions<AppAction>,
    private store: Store<State>
  ) { }

  @Effect() onDocumentBegin_dispatchWizardBegin$ = this.actions$
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

  @Effect() onWizardBegin_dispatchLayoutOpenDialogBegin$ = this.actions$
    .ofType(WizardActionsTypes.BEGIN)
    .map(() => new LayoutOpenDialogBeginAction({
      dialog: 'wizard'
    }));

  @Effect() onWizardUpdateValues_dispatchDocumentUpdateDraft$ = this.actions$
    .ofType(WizardActionsTypes.UPDATE_VALUES)
    .filter(a => a.reducerId === REDUCER_ID)
    .debounceTime(500)
    .withLatestFrom(this.store)
    .map(([action, state]) => new DocumentUpdateDraftAction(state.generatorBulk.wizard));

  @Effect() onDocumentPublishDraft_dispatchLayoutCloseDialogBegin$ = this.actions$
    .ofType(DocumentActionsTypes.PUBLISH_DRAFT)
    .map(() => new LayoutCloseDialogBeginAction({
      dialog: 'wizard'
    }));

  @Effect() onLayoutCloseDialogEnd_dispatchWizardReset$ = this.actions$
    .ofType<LayoutDialogAction>(LayoutActionTypes.CLOSE_DIALOG_END)
    .filter(a => a.payload.dialog === 'wizard')
    .map(() => new WizardResetAction(REDUCER_ID));

  @Effect() onLayoutCloseDialogEnd_dispatchUpdateConstantsBegin$ = this.actions$
    .ofType<LayoutDialogAction>(LayoutActionTypes.CLOSE_DIALOG_END)
    .withLatestFrom(this.store)
    .filter(([action, store]) => action.payload.dialog === 'wizard' && store.generatorBulk.documents.repeating)
    .map(([action, store]) => store.generatorBulk.documents.constants ?
      new WizardBeginAction(REDUCER_ID, {
        template: store.generatorBulk.documents.template,
        presets: store.generatorBulk.documents.constants
      }) :
      new DocumentUpdateConstantsBeginAction());

  @Effect() onDocumentUpdateConstantsBegin_dispatchLayoutOpenDialogBegin$ = this.actions$
    .ofType(DocumentActionsTypes.UPDATE_CONSTANTS_BEGIN)
    .map(() => new LayoutOpenDialogBeginAction({ dialog: 'select-constants' }));

  @Effect() onDocumentUpdateConstants_dispatchLayoutCloseDialogBegin$ = this.actions$
    .ofType(DocumentActionsTypes.UPDATE_CONSTANTS)
    .withLatestFrom(this.store)
    .filter(([action, state]) => state.generatorBulk.layout.dialog === 'select-constants')
    .map(() => new LayoutCloseDialogBeginAction({ dialog: 'select-constants' }));

  @Effect() onLayoutCloseDialogEnd_dispatchWizardBegin$ = this.actions$
    .ofType<LayoutDialogAction>(LayoutActionTypes.CLOSE_DIALOG_END)
    .filter(a => a.payload.dialog === 'select-constants')
    .withLatestFrom(this.store)
    .switchMap(([action, state]) => {
      const { documents } = state.generatorBulk;

      if (documents.constants) {
        // TODO: Improve this. Basically checking if the last dialog was dismissed or closed.
        return Observable.of(new WizardBeginAction(REDUCER_ID, {
          template: documents.template,
          presets: documents.constants
        }));
      }

      return Observable.empty();
    });
}
