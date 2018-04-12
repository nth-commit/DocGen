import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef, MatDialogConfig } from '@angular/material';
import { Store, Action } from '@ngrx/store';
import { Actions, Effect } from '@ngrx/effects';
import { RouterReducerState, ROUTER_NAVIGATION } from '@ngrx/router-store';
import { Observable } from 'rxjs/Observable';

import { State, AppAction, GeneratorBulkLayoutDialogState } from '../../../../../_shared';
import { LayoutActionTypes, LayoutOpenDialogBeginAction, LayoutOpenDialogEndAction, LayoutCloseDialogBeginAction } from './layout.actions';
import { WizardDialogComponent } from '../../components/wizard-dialog/wizard-dialog.component';

@Injectable()
export class LayoutEffects {

  constructor(
    private actions$: Actions<AppAction>,
    private store: Store<State>,
    private matDialog: MatDialog
  ) { }

  @Effect() onOpenDialogBegin_openDialogEnd$ = this.actions$
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
}
