import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef, MatDialogConfig } from '@angular/material';
import { Store, Action } from '@ngrx/store';
import { Actions, Effect } from '@ngrx/effects';
import { RouterReducerState, ROUTER_NAVIGATION } from '@ngrx/router-store';
import { Observable } from 'rxjs/Observable';

import { State, AppAction, GeneratorBulkLayoutDialogState } from '../../../../../_shared';
import {
  LayoutActionTypes,
  LayoutOpenDialogBeginAction,
  LayoutOpenDialogEndAction,
  LayoutCloseDialogBeginAction,
  LayoutCloseDialogEndAction
} from './layout.actions';

import { WizardDialogComponent } from '../../components/wizard-dialog/wizard-dialog.component';
import { SelectConstantsDialogComponent } from '../../components/select-constants-dialog/select-constants-dialog.component';

@Injectable()
export class LayoutEffects {

  constructor(
    private actions$: Actions<AppAction>,
    private store: Store<State>,
    private matDialog: MatDialog
  ) { }

  @Effect({ dispatch: false }) onOpenDialogBegin_subscribeToDialogEvents$ = this.actions$
    .ofType(LayoutActionTypes.OPEN_DIALOG_BEGIN)
    .do((action: LayoutOpenDialogBeginAction) => {

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
      } else if (action.payload.dialog === 'select-constants') {
        dialogOpenArgs = {
          componentType: SelectConstantsDialogComponent,
          config: {
            width: '550px',
            height: '1px',
            minHeight: '700px'
          }
        };
      } else {
        throw new Error('Unknown dialog name');
      }

      Observable
        .timer(500)
        .first()
        .subscribe(() => {
          const dialogRef = this.matDialog.open(dialogOpenArgs.componentType, dialogOpenArgs.config);

          dialogRef.afterOpen()
            .first()
            .subscribe(() => {
              this.store.dispatch(new LayoutOpenDialogEndAction({ dialog: action.payload.dialog, dialogRef }));
            });

          dialogRef.afterClosed()
            .first()
            .debounceTime(100)
            .subscribe(() => {
              this.store
                .select(s => s.generatorBulk.layout)
                .first()
                .subscribe(layout => {
                  if (layout.dialogState !== GeneratorBulkLayoutDialogState.Closing) {
                    this.store.dispatch(new LayoutCloseDialogBeginAction({ dialog: layout.dialog }));
                  }

                  this.store.dispatch(new LayoutCloseDialogEndAction({ dialog: layout.dialog }));
                });
            });
        });
    });

    // @Effect() onCloseDialogBegin_closeDialogEnd$ = this.actions$
    //   .ofType<LayoutCloseDialogBeginAction>(LayoutActionTypes.CLOSE_DIALOG_BEGIN)
    //   .debounceTime(100) // Give dialog content a chance to unload
    //   .map(action => new LayoutCloseDialogEndAction({ dialog: action.payload.dialog }));
}
