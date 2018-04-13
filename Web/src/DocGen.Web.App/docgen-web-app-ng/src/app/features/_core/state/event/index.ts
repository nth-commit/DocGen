import { Action, MetaReducer } from '@ngrx/store';

import { AppAction, EventState } from '../../../_core';

export function coreEventReducer(state: EventState, action: AppAction): EventState {
  return {
    actionType: action.type,
    reducerId: action.reducerId
  };
}
