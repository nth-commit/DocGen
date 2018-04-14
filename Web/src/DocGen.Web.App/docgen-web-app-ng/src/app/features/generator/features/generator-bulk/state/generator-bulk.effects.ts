import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Actions, Effect } from '@ngrx/effects';
import { Observable } from 'rxjs/Observable';

import { State, AppAction, GeneratorBulkDocumentRepeatState } from '../../../../_core';
import { WizardActionsTypes, WizardBeginAction, WizardResetAction } from '../../_core';
import { DocumentActionsTypes, DocumentBeginAction, DocumentUpdateDocumentAction, DocumentUpdateConstantsBeginAction } from './document';
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
    .debounceTime(500)
    .withLatestFrom(this.store, (action, state) => state.generatorBulk.documents)
    .filter(documents => !documents.draftDocuments.length && !documents.draftDocuments.length)
    .map(documents => new WizardBeginAction(REDUCER_ID, {
      template: documents.template,
      presets: documents.constants
    }));

  @Effect() onWizardBegin_dispatchLayoutOpenDialogBegin$ = this.actions$
    .ofType(WizardActionsTypes.BEGIN, WizardActionsTypes.RESUME)
    .map(() => new LayoutOpenDialogBeginAction({
      dialog: 'wizard'
    }));

  @Effect() onWizardUpdateValues_dispatchDocumentUpdateDraft$ = this.actions$
    .ofType(WizardActionsTypes.UPDATE_VALUES)
    .filter(a => a.reducerId === REDUCER_ID)
    .debounceTime(500)
    .withLatestFrom(this.store, (action, state) => state.generatorBulk.wizard)
    .map(wizard => new DocumentUpdateDocumentAction(wizard));

  @Effect() onDocumentPublishDraft_dispatchLayoutCloseDialogBegin$ = this.actions$
    .ofType(DocumentActionsTypes.PUBLISH_DOCUMENT)
    .map(() => new LayoutCloseDialogBeginAction({
      dialog: 'wizard'
    }));

  @Effect() onLayoutCloseDialogEnd_dispatchWizardReset$ = this.actions$
    .ofType<LayoutDialogAction>(LayoutActionTypes.CLOSE_DIALOG_END)
    .filter(a => a.payload.dialog === 'wizard')
    .map(() => new WizardResetAction(REDUCER_ID));

  @Effect() onLayoutCloseDialogEnd_dispatchUpdateConstantsBegin$ = this.actions$
    .ofType<LayoutDialogAction>(LayoutActionTypes.CLOSE_DIALOG_END)
    .filter(action => action.payload.dialog === 'wizard')
    .withLatestFrom(this.store, (action, state) => state.generatorBulk.documents)
    .filter(documents => documents.repeating)
    .map(documents => documents.constants ?
      new WizardBeginAction(REDUCER_ID, {
        template: documents.template,
        presets: documents.constants
      }) :
      new DocumentUpdateConstantsBeginAction());

  @Effect() onDocumentUpdateConstantsBegin_dispatchLayoutOpenDialogBegin$ = this.actions$
    .ofType(DocumentActionsTypes.UPDATE_CONSTANTS_BEGIN)
    .map(() => new LayoutOpenDialogBeginAction({ dialog: 'select-constants' }));

  @Effect() onDocumentUpdateConstants_dispatchLayoutCloseDialogBegin$ = this.actions$
    .ofType(DocumentActionsTypes.UPDATE_CONSTANTS, DocumentActionsTypes.UPDATE_CONSTANTS_CANCEL)
    .withLatestFrom(this.store, (action, state) => state.generatorBulk.layout)
    .filter(layout => layout.dialog === 'select-constants')
    .map(() => new LayoutCloseDialogBeginAction({ dialog: 'select-constants' }));

  @Effect() onLayoutCloseDialogEnd_dispatchWizardBegin$ = this.actions$
    .ofType<LayoutDialogAction>(LayoutActionTypes.CLOSE_DIALOG_END)
    .filter(a => a.payload.dialog === 'select-constants')
    .withLatestFrom(this.store, (action, state) => state.generatorBulk.documents)
    .filter(documents => !!documents.constants) // TODO: Improve this. Basically checking if the last dialog was dismissed or closed.
    .map(documents => new WizardBeginAction(REDUCER_ID, {
      template: documents.template,
      presets: documents.constants
    }));
}
