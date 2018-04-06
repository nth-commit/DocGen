import { Inject, Injectable } from '@angular/core';
import { Action, ScannedActionsSubject } from '@ngrx/store';
import { Actions } from '@ngrx/effects';

import { Observable } from 'rxjs/Observable';
import { Operator } from 'rxjs/Operator';
import { OperatorFunction } from 'rxjs/interfaces';
import { filter } from 'rxjs/operators';

import { AppAction } from '../../_shared';

@Injectable()
export class AppActions<V = AppAction> extends Observable<V> {
  constructor(@Inject(ScannedActionsSubject) source?: Observable<V>) {
    super();

    if (source) {
      this.source = source;
    }
  }

  ofReducerId<V2 extends V = V>(reducerId: string): AppActions<V2> {
    return ofReducerId<any>(reducerId)(this as AppActions<any>) as AppActions<V2>;
  }
}

export function ofReducerId<T extends AppAction>(reducerId: string): OperatorFunction<AppAction, T> {
  return filter((action: AppAction): action is T => action.reducerId === reducerId);
}

