import { Injectable } from '@angular/core';
import { Store, Action } from '@ngrx/store';
import { Actions, Effect } from '@ngrx/effects';

import { State, AppAction } from '../../../../_shared';
import { WizardActionsTypes, WizardBeginAction } from '../../_shared';
import { DocumentActionsTypes } from './document';
import { REDUCER_ID } from './constants';

@Injectable()
export class GeneratorBulkEffects {

  constructor(
    private actions$: Actions<AppAction>,
    private store: Store<State>
  ) { }

  @Effect({ dispatch: false }) beginWizard$ = this.actions$
    .ofType(DocumentActionsTypes.BEGIN_DRAFT)
    .filter(a => a.reducerId === REDUCER_ID)
    .withLatestFrom(this.store)
    .do(([event, state]) => {
      this.store.dispatch(new WizardBeginAction(REDUCER_ID, {
        template: state.generatorBulk.documents.template
      }));
    });
}
