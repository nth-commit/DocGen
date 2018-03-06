import {
  Action,
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer
} from '@ngrx/store';
import { environment } from '../../../../environments/environment';

export enum WizardActionTypes {

}

export interface WizardState {

}

export const reducer: ActionReducer<WizardState> = (state, { type }: Action) => {

  return state;
};


export const metaReducers: MetaReducer<WizardState>[] = !environment.production ? [] : [];
