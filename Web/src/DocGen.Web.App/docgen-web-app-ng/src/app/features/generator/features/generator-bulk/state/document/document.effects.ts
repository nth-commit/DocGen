import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Actions, Effect } from '@ngrx/effects';
import { Observable } from 'rxjs/Observable';

import { State, AppAction } from '../../../../../_core';
import { DocumentActionsTypes, DocumentPublishDraftAction, DocumentUpdateConstantsBeginAction } from './document.actions';

let id = 0;

@Injectable()
export class DocumentEffects {

  constructor(
    private actions$: Actions<AppAction>,
    private store: Store<State>
  ) { }

  @Effect({ dispatch: false })
  x = this.actions$.do(a => {
    id++;
    (<any>a).id = id;
    console.log('Action: ' + id, a);
  });

}
