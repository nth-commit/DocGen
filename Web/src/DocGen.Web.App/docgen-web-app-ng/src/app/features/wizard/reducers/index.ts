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
  Template, TemplateSigningType, TemplateStep, TemplateStepInput, TemplateStepConditionType,
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
  templateValid: boolean;
  templateStepsValid: boolean[];
  templateStepInputsValid: boolean[][];
  completed: boolean;
  empty: boolean;
}

export const TEMPLATE_STEP_IS_DOCUMENT_SIGNED: TemplateStep = {
  id: 'document_signed',
  name: 'Use electronic signature?',
  description: 'Should this document be electronically signed?',
  conditions: [],
  inputs: [
    <TemplateStepInput>{
      key: null,
      id: 'document_signed',
      type: 3
    }
  ],
  parentReference: null
};

export const reducerBase: ActionReducer<WizardState> = (state, action: WizardAction) => {
  switch (action.type) {
    case WizardActionTypes.REFRESH: {
      return action.payload;
    }
    case WizardActionTypes.BEGIN: {
      const template = <Template>action.payload;

      // Push in the step to allow decision on whether document is signed
      let signDocumentStepIndex = -1;
      if (template.signingType === TemplateSigningType.Optional) {
        const firstDependentStepIndex = template.steps.findIndex(s =>
          s.conditions.some(c => c.type === TemplateStepConditionType.IsDocumentSigned));

        signDocumentStepIndex = firstDependentStepIndex || template.steps.length;

        template.steps = template.steps.insert(signDocumentStepIndex, TEMPLATE_STEP_IS_DOCUMENT_SIGNED);
        template.steps.forEach(s => {
          const documentSignedConditionIndex = s.conditions.findIndex(c => c.type === TemplateStepConditionType.IsDocumentSigned);

          // Replace the IsDocumentSigned conditions with EqualsPreviousInputValue
          if (documentSignedConditionIndex > -1) {
            s.conditions.splice(documentSignedConditionIndex, 1);
            s.conditions.push({
              type: TemplateStepConditionType.EqualsPreviousInputValue,
              typeData: {
                PreviousInputId: TEMPLATE_STEP_IS_DOCUMENT_SIGNED.id,
                PreviousInputValue: true
              }
            });
          }
        });
      }

      // Mark all inputs as invalid initially
      const templateStepInputsValid = [];
      template.steps.forEach(s => {
        const result = [];
        s.inputs.forEach(i => result.push(false));
        templateStepInputsValid.push(result);
      });

      return Object.assign({}, state, <WizardState>{
        template,
        currentStepIndex: 0,
        values: InputValueCollectionUtility.fromTemplate(template),
        templateStepInputsValid,
        stepIndexHistory: []
      });
    }
    case WizardActionTypes.UPDATE_VALUES: {
      const values: InputValueCollection = Object.assign({}, state.values, action.payload);

      const templateStepInputsValid: boolean[][] = [];
      state.template.steps.forEach((step, stepIndex) => {
        const result: boolean[] = [];

        const skipValidation = step.conditions.some(c => {
          if (c.type === TemplateStepConditionType.EqualsPreviousInputValue) {
            // Skip if values do not match.
            const expectedPreviousInputValue = c.typeData.PreviousInputValue;
            const previousInputId = c.typeData.PreviousInputId;

            return state.values[previousInputId] !== expectedPreviousInputValue;
          }
          return false;
        });

        step.inputs.forEach((input, inputIndex) => {
          result[inputIndex] = skipValidation || (values[input.id] !== undefined && values[input.id] !== null);
        });

        templateStepInputsValid[stepIndex] = result;
      });

      return Object.assign(state, <WizardState>{
        values,
        templateStepInputsValid
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
      if (!state.templateValid) {
        throw new Error('Template values are incomplete');
      }

      return Object.assign(state, <WizardState>{
        completed: true
      });
    }
    case WizardActionTypes.CLEAR_VALUES: {
      const template = state.template;

      const templateStepInputsValid = [];
      template.steps.forEach(s => {
        const result = [];
        s.inputs.forEach(i => result.push(false));
        templateStepInputsValid.push(result);
      });

      return Object.assign({}, state, <WizardState>{
        template,
        currentStepIndex: 0,
        values: InputValueCollectionUtility.fromTemplate(template),
        templateStepInputsValid,
        stepIndexHistory: []
      });
    }
    default: {
      return {};
    }
  }
};

export const reducer: ActionReducer<WizardState> = (state, action: WizardAction) => {
  state = reducerBase(state, action);

  if (state.template) {
    state.templateStepsValid = state.templateStepInputsValid.map(inputsValid => inputsValid.every(x => x));
    state.templateValid = state.templateStepsValid.every(x => x);

    state.hasPreviousStep = state.currentStepIndex > 0;

    state.currentStep = state.template.steps[state.currentStepIndex];

    state.currentValues = {};
    state.currentStep.inputs.forEach(i => {
      state.currentValues[i.id] = state.values[i.id];
    });

    state.nextStepIndex = state.template.steps
      .findIndex((s, i) => {
        if (i <= state.currentStepIndex) {
          return false;
        }

        const allConditionsMet = s.conditions.every(c => {
          if (c.type === TemplateStepConditionType.EqualsPreviousInputValue) {
            return state.values[c.typeData.PreviousInputId] === c.typeData.PreviousInputValue;
          }

          return false;
        });

        return allConditionsMet;
      });

    state.hasNextStep = state.nextStepIndex > -1;

    state.nextStep = state.hasNextStep ? state.template.steps[state.nextStepIndex] : null;

    state.currentStepValid = state.currentStep.inputs
      .every(i => state.values[i.id] !== undefined && state.values[i.id] !== null);

    state.empty = Object.keys(state.values).every(id => state.values[id] === undefined || state.values[id] === null);
  }

  return state;
};


export const metaReducers: MetaReducer<WizardState>[] = !environment.production ? [] : [];
