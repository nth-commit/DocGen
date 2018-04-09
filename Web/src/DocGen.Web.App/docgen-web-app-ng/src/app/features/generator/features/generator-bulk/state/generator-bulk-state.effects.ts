import { Injectable } from '@angular/core';
import { Store, Action } from '@ngrx/store';
import { Actions, Effect } from '@ngrx/effects';

import { State, AppAction } from '../../../../_shared';
import { WizardActionsTypes, WizardBeginAction } from '../../_shared';
import { DocumentActionsTypes, DocumentBeginAction, DocumentUpdateDraftAction } from './document';
import { REDUCER_ID } from './constants';

import { WizardDialogComponent } from '../components/wizard-dialog/wizard-dialog.component';

@Injectable()
export class GeneratorBulkStateEffects {

  constructor(
    private actions$: Actions<AppAction>,
    private store: Store<State>
  ) { }

  @Effect() updateDraftDocument$ = this.actions$
    .ofType(WizardActionsTypes.BEGIN, WizardActionsTypes.UPDATE_VALUES)
    .filter(a => a.reducerId === REDUCER_ID)
    .debounceTime(500)
    .withLatestFrom(this.store)
    .map(([action, state]) => new DocumentUpdateDraftAction(state.generatorBulk.wizard));

  // @Effect() resetWizardWhenPublished = this.actions$
  //   .ofType(DocumentActionsTypes.PUBLISH_DRAFT)
  //   .map(action => )
}
