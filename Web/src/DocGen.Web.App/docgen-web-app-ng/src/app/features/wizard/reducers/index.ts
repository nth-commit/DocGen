import {
  Action,
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer
} from '@ngrx/store';

import { environment } from '../../../../environments/environment';

import {
  Template, TemplateStep, TemplateStepConditionType, TemplateUtility,
  InputValue, InputValueCollection, InputValueCollectionUtility
} from '../../core';

export enum WizardActionTypes {
  REFRESH = '[Wizard] Refresh',
  BEGIN = '[Wizard] Begin',
  UPDATE_VALUES = '[Wizard] Update values',
  NEXT_STEP = '[Wizard] Next step',
  PREVIOUS_STEP = '[Wizard] Previous step',
  COMPLETE = '[Wizard] Complete',
  CLEAR_VALUES = '[Wizard] Clear'
}

export class Refresh implements Action {
  readonly type: string = WizardActionTypes.REFRESH;
  constructor (public payload: WizardState) { }
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

export class ClearValues implements Action {
  readonly type: string = WizardActionTypes.CLEAR_VALUES;
  constructor () { }
}


export type WizardAction = Begin | UpdateValues| NextStep | PreviousStep | Complete;

export interface State {
  wizard: WizardState;
}

export interface WizardState {
  template: Template;
  stepIndexHistory: number[];
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
  completed: boolean;
  empty: boolean;
}

const DEFAULT_WIZARD_STATE = <WizardState>{
  stepIndexHistory: []
};

export const reducerBase: ActionReducer<WizardState> = (state, action: WizardAction) => {
  switch (action.type) {
    case WizardActionTypes.REFRESH: {
      return action.payload;
    }
    case WizardActionTypes.BEGIN: {
      return Object.assign({}, state, <WizardState>{
        template: action.payload,
        currentStepIndex: 0,
        values: InputValueCollectionUtility.fromTemplate(<any>action.payload)
      });
    }
    case WizardActionTypes.UPDATE_VALUES: {
      const values = Object.assign({}, state.values, action.payload);
      return Object.assign(state, <WizardState>{
        values,

        valid: state.template.steps.every(s => {
          if (s.conditionType === TemplateStepConditionType.EqualsPreviousInputValue) {
            const expectedPreviousInputValue = s.conditionTypeData.PreviousInputValue;
            const previousInputId = s.conditionTypeData.PreviousInputId;

            if (state.values[previousInputId] !== expectedPreviousInputValue) {
              // Skip this step
              return true;
            }
          }

          return s.inputs.every(i => {
            const inputId = TemplateUtility.getTemplateStepInputId(s, i);
            return state.values[inputId] !== undefined && state.values[inputId] !== null;
          });
        })
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
        currentStepIndex: state.nextStepIndex,
        stepIndexHistory: [...state.stepIndexHistory, state.currentStepIndex]
      });
    }
    case WizardActionTypes.PREVIOUS_STEP: {
      if (!state.hasPreviousStep) {
        throw new Error('Template does not have a previous step');
      }

      return Object.assign(state, <WizardState>{
        currentStepIndex: state.stepIndexHistory[state.stepIndexHistory.length - 1],
        stepIndexHistory: state.stepIndexHistory.slice(0, state.stepIndexHistory.length - 1)
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
    case WizardActionTypes.CLEAR_VALUES: {
      return Object.assign(
        {},
        <WizardState>{
          template: state.template,
          currentStepIndex: 0,
          values: InputValueCollectionUtility.fromTemplate(state.template)
        },
        DEFAULT_WIZARD_STATE);
    }
    default: {
      return state || DEFAULT_WIZARD_STATE;
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
      const inputId = TemplateUtility.getTemplateStepInputId(state.currentStep, i);
      state.currentValues[inputId] = state.values[inputId];
    });

    state.nextStepIndex = state.template.steps
      .findIndex((s, i) => i > state.currentStepIndex &&
        (!s.conditionType || state.values[s.conditionTypeData.PreviousInputId] === s.conditionTypeData.PreviousInputValue));

    state.hasNextStep = state.nextStepIndex > -1;

    state.nextStep = state.hasNextStep ? state.template.steps[state.nextStepIndex] : null;

    state.currentStepValid = state.currentStep.inputs
      .map(i => TemplateUtility.getTemplateStepInputId(state.currentStep, i))
      .every(inputId => state.values[inputId] !== undefined && state.values[inputId] !== null);

    state.empty = Object.keys(state.values).every(id => state.values[id] === undefined || state.values[id] === null);
  }

  return state;
};


export const metaReducers: MetaReducer<WizardState>[] = !environment.production ? [] : [];
