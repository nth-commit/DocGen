import { Injectable } from '@angular/core';
import { Router, RouterEvent, NavigationEnd, RoutesRecognized } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { Store, Action } from '@ngrx/store';
import { Actions, Effect } from '@ngrx/effects';

import { Observable } from 'rxjs/Observable';
import { timer } from 'rxjs/observable/timer';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/withLatestFrom';

import { State, WizardActionTypes, Complete, ClearValues } from './reducers';
import { RouteChangeService } from '../_core';

@Injectable()
export class WizardEffects {

  constructor(
    private actions$: Actions,
    private store: Store<State>,
    private router: Router,
    private snackBar: MatSnackBar,
    private routeChangeService: RouteChangeService) { }

  @Effect({ dispatch: false }) changes$ = this.actions$
    .ofType(WizardActionTypes.BEGIN, WizardActionTypes.UPDATE_VALUES, WizardActionTypes.NEXT_STEP, WizardActionTypes.NEXT_STEP)
    .withLatestFrom(this.store)
    .do(([action, state]) => {
      localStorage.setItem(this.getKey(state, 'wizard'), JSON.stringify(state.wizard));
    });

  @Effect({ dispatch: false }) refresh$ = this.actions$
    .ofType(WizardActionTypes.REFRESH)
    .do(action => {
      this.router.events
        .debounceTime(1000)
        .first()
        .withLatestFrom(this.store)
        .subscribe(([event, state]) => {
          if (!state.wizard.empty && (!this.routeChangeService.previousUrl || this.routeChangeService.previousUrl === '/') ) {
            const snackBarRef = this.snackBar.open(
              'We loaded the document you were working on before!',
              'Reset',
              { duration: 10000 });

            this.router.events
              .first()
              .subscribe(ev => {
                snackBarRef.dismiss();
              });

            snackBarRef
              .onAction()
              .first()
              .subscribe(() => {
                this.store.dispatch(new ClearValues());
              });
          }
        });
    });

  @Effect({ dispatch: false }) complete$ = this.actions$
    .ofType(WizardActionTypes.COMPLETE)
    .withLatestFrom(this.store)
    .do(([action, state]) => {
      const { template } = state.wizard;

      const valuesKey = this.getKey(state, `${template.version}:values`);
      localStorage.setItem(valuesKey, JSON.stringify(state.wizard.values)); // Used for document generation module

      const correlationIdKey = this.getKey(state, `${template.version}:correlationId`);
      localStorage.setItem(correlationIdKey, state.wizard.correlationId);

      this.router.navigateByUrl(`/${template.id}/preview?version=${template.version}`);
    });

  @Effect({ dispatch: false }) clear$ = this.actions$
    .ofType(WizardActionTypes.CLEAR_VALUES)
    .withLatestFrom(this.store)
    .do(([action, state]) => {
      localStorage.removeItem(this.getKey(state, 'wizard'));
    });

    private getKey(state: State, suffix: string) {
      return `drafts:${state.wizard.template.id}:${suffix}`;
    }
}
