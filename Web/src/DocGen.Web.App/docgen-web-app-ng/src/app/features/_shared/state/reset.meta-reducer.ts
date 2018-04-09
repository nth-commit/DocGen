import { ActionReducer, Action } from '@ngrx/store';

export const RESET = '[Meta] Reset';

export function resetMetaReducer<T>(reducer: ActionReducer<T>): ActionReducer<T> {
  return function resetReducer(state: T, action) {
    if (action.type === RESET) {
      state = undefined;
    }
    return reducer(state, action);
  };
}
