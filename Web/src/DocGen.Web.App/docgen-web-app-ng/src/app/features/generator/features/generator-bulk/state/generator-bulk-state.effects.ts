import { Injectable } from '@angular/core';
import { Store, Action } from '@ngrx/store';
import { Actions, Effect } from '@ngrx/effects';
import { MatDialog } from '@angular/material';

import { State, AppAction } from '../../../../_shared';
import { WizardActionsTypes, WizardBeginAction } from '../../_shared';
import { DocumentActionsTypes } from './document';
import { REDUCER_ID } from './constants';

import { WizardDialogComponent } from '../components/wizard-dialog/wizard-dialog.component';

@Injectable()
export class GeneratorBulkStateEffects {

  constructor(
    private actions$: Actions<AppAction>,
    private store: Store<State>,
    private matDialog: MatDialog
  ) { }

  @Effect() beginWizard$ = this.actions$
    .ofType(DocumentActionsTypes.BEGIN_DRAFT)
    .withLatestFrom(this.store)
    .map(([action, state]) => new WizardBeginAction(REDUCER_ID, {
      template: state.generatorBulk.documents.template
    }));
}
