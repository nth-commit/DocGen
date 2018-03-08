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

import { Utility } from '../utility';
import { InputValueCollection, InputValue, createInputValueCollection } from '../models';


export enum WizardActionTypes {
  BEGIN = '[Wizard] Begin',
  UPDATE_VALUES = '[Wizard] Update values',
  NEXT_STEP = '[Wizard] Next step',
  PREVIOUS_STEP = '[Wizard] Previous step',
  COMPLETE = '[Wizard] Complete',
}

export class Begin implements Action {
  readonly type: string = WizardActionTypes.BEGIN;
  constructor (public payload: Template) { }
}

export class UpdateValues implements Action {
  readonly type: string = WizardActionTypes.UPDATE_VALUES;
  constructor(public payload: InputValueCollection) { }
}

export class NextStep implements Action {
  readonly type: string = WizardActionTypes.NEXT_STEP;
  constructor (public payload?: any) { }
}

export class PreviousStep implements Action {
  readonly type: string = WizardActionTypes.PREVIOUS_STEP;
  constructor (public payload?: any) { }
}

export class Complete implements Action {
  readonly type: string = WizardActionTypes.COMPLETE;
  constructor (public payload?: any) { }
}

export type WizardAction = Begin | UpdateValues; // | NextStep | PreviousStep | Complete;

export interface State {
  wizard: WizardState;
}

export interface WizardState {
  template: Template;
  hasPreviousStep: boolean;
  currentStep: TemplateStep;
  currentStepIndex: number;
  currentValues: InputValueCollection;
  nextStep?: TemplateStep;
  nextStepIndex: number;
  hasNextStep: boolean;
  values: InputValueCollection;
  currentStepValid: boolean;
  valid: boolean;
  completed: true;
}

export const reducerBase: ActionReducer<WizardState> = (state, action: WizardAction) => {
  switch (action.type) {
    case WizardActionTypes.BEGIN: {
      return Object.assign({}, state, <WizardState>{
        template: action.payload,
        currentStepIndex: 0,
        values: createInputValueCollection(<any>action.payload)
      });
    }
    case WizardActionTypes.UPDATE_VALUES: {
      return Object.assign(state, <WizardState>{
        values: Object.assign({}, state.values, action.payload)
      });
    }
    case WizardActionTypes.NEXT_STEP: {
      if (!state.hasNextStep) {
        throw new Error('Template does not have a next step');
      }

      if (!state.currentStepValid) {
        throw new Error('Cannot navigate to the next step if the current step is invalid');
      }

      return Object.assign(state, <WizardState>{
        currentStepIndex: state.currentStepIndex + 1
      });
    }
    case WizardActionTypes.PREVIOUS_STEP: {
      if (!state.hasPreviousStep) {
        throw new Error('Template does not have a previous step');
      }
      return Object.assign(state, <WizardState>{
        currentStepIndex: state.currentStepIndex - 1
      });
    }
    case WizardActionTypes.COMPLETE: {
      if (!state.valid) {
        throw new Error('Template values are incomplete');
      }
      return Object.assign(state, <WizardState>{
        completed: true
      });
    }
    default: {
      return state || <WizardState>{};
    }
  }
};

export const reducer: ActionReducer<WizardState> = (state, action: WizardAction) => {
  state = reducerBase(state, action);

  if (state.template) {
    state.hasPreviousStep = state.currentStepIndex > 0;

    state.currentStep = state.template.steps[state.currentStepIndex];

    state.currentValues = {};
    state.currentStep.inputs.forEach(i => {
      const inputId = Utility.getTemplateStepInputId(state.currentStep, i);
      state.currentValues[inputId] = state.values[inputId];
    });

    state.nextStepIndex = state.template.steps
      .findIndex((s, i) => i > state.currentStepIndex &&
        (!s.conditionType || state.values[s.conditionTypeData.previousInputId] === s.conditionTypeData.previousInputValue));

    state.hasNextStep = state.nextStepIndex > -1;

    state.nextStep = state.hasNextStep ? state.template.steps[state.nextStepIndex] : null;

    state.currentStepValid = state.currentStep.inputs
      .map(i => Utility.getTemplateStepInputId(state.currentStep, i))
      .every(reference => !!state.values[reference]);

    state.valid = false; // TODO
  }

  return state;
};


export const metaReducers: MetaReducer<WizardState>[] = !environment.production ? [] : [];
