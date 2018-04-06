import { Action, MetaReducer } from '@ngrx/store';

import { AppAction, EventState } from '../../../_shared';

export function coreEventReducer(state: EventState, action: AppAction): EventState {
  return {
    actionType: action.type,
    reducerId: action.reducerId
  };
}
