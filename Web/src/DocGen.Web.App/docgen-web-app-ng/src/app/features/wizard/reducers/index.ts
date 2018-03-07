import {
  Action,
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer
} from '@ngrx/store';

import { environment } from '../../../../environments/environment';

import { Template, TemplateStep, TemplateStepConditionType } from '../../core';
import { InputValueCollection, InputValue } from '../models';


export enum WizardActionTypes {
  BEGIN = '[Wizard] Begin',
  UPDATE_VALUES = '[Wizard] Update values'
}

export class Begin implements Action {
  readonly type: string = WizardActionTypes.BEGIN;
  constructor (public payload: Template) { }
}

export class UpdateValues implements Action {
  readonly type: string = WizardActionTypes.UPDATE_VALUES;
  constructor(public payload: InputValueCollection) { }
}

export type WizardAction = Begin;

export interface WizardState {
  template: Template;
  values: InputValueCollection;
  currentStepIndex: number;
  nextStepIndex: number;
}

export const reducer: ActionReducer<WizardState> = (state, { type, payload }: WizardAction) => {
  if (type === WizardActionTypes.BEGIN) {
    const initialValues: InputValueCollection = {};
    payload.steps.forEach(s => {
      s.inputs.forEach(i => {
        initialValues[`${s.id}.${i.key}`] = null;
      });
    });

    state = Object.assign({}, state, <WizardState>{
      template: payload,
      currentStepIndex: 0,
      values: initialValues
    });
  } else if (type === WizardActionTypes.UPDATE_VALUES) {
    state.values = Object.assign({}, state.values, payload);
  } else {
    state = <WizardState>{};
  }

  if (state.template) {
    state.nextStepIndex = state.template.steps
      .slice(state.currentStepIndex + 1, state.template.steps.length)
      .findIndex(s => !s.conditionType || state.values[s.conditionTypeData.previousInputId] === s.conditionTypeData.previousInputValue);
  }

  return state;
};


export const metaReducers: MetaReducer<WizardState>[] = !environment.production ? [] : [];
