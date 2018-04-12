import { Action, ActionReducer } from '@ngrx/store';

import {
  Template, TemplateStep, TemplateStepInput, TemplateStepConditionType,
  InputValueCollection, InputValueCollectionUtility
} from '../../../../../core';

import { GeneratorWizardState } from '../../../../../_shared';
import { WizardActionsTypes, WizardAction, WizardBeginAction, WizardResetAction } from './generator-wizard.actions';

export function createGeneratorWizardReducer(id: string): ActionReducer<GeneratorWizardState, WizardAction> {

  function generatorWizardReducer(state: GeneratorWizardState, action: WizardAction) {
    if (action.reducerId !== id) {
      return state || <GeneratorWizardState>{};
    }
    return extendState(resolveState(state, action));
  }

  function resolveState(state: GeneratorWizardState, action: WizardAction): GeneratorWizardState {
    switch (action.type) {
      case WizardActionsTypes.BEGIN: {
        if (state.id) {
          throw new Error('Wizard has already began');
        }

        const { template, presets, showPresetInputs } = action.payload;
        // TODO: Validate presets!

        // Ignore steps after signing for now. Previous wizard reducer has example of how to handle them. But is it necessary? Should we
        // worry about signing info at a later time?
        const fistStepIndexWithSignedCondition = template.steps.findIndex(s =>
          s.conditions.some(c => c.type === TemplateStepConditionType.IsDocumentSigned));

        const allSteps = template.steps.slice(0, fistStepIndexWithSignedCondition);

        const steps = allSteps
          .map(s => Object.assign({}, s, <TemplateStep>{
            inputs: s.inputs.filter(i => !presets || showPresetInputs || !(i.id in presets))
          }))
          .filter(s => s.inputs.length);

        const stepInputsValid = steps.map(s => s.inputs.map(i => false));

        const values = Object.assign(InputValueCollectionUtility.fromSteps(steps), presets || {});

        return Object.assign({}, state, <GeneratorWizardState>{
          id: new Date().getTime().toString(),

          steps,
          allSteps,
          stepIndex: 0,
          stepIndexHistory: [],

          stepInputsValid,
          values,
          presets
        });
      }
      case WizardActionsTypes.UPDATE_VALUES: {
        const values: InputValueCollection = Object.assign({}, state.values, action.payload);

        const stepInputsValid: boolean[][] = [];
        state.steps.forEach((step, stepIndex) => {
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

          stepInputsValid[stepIndex] = result;
        });

        return Object.assign({}, state, <GeneratorWizardState>{
          values,
          stepInputsValid
        });
      }
      case WizardActionsTypes.NEXT: {
        if (!state.stepValid) {
          throw new Error('Cannot navigate to the next step if the current step is invalid');
        }

        if (state.hasNextStep) {
          return Object.assign({}, state, <GeneratorWizardState>{
            stepIndex: state.nextStepIndex,
            stepIndexHistory: [...state.stepIndexHistory, state.stepIndex]
          });
        } else {
          return Object.assign({}, state, <GeneratorWizardState>{
            completed: true
          });
        }
      }
      case WizardActionsTypes.PREVIOUS: {
        if (state.completed) {
          return Object.assign({}, state, <GeneratorWizardState>{
            completed: false
          });
        } else if (state.hasPreviousStep) {
          return Object.assign({}, state, <GeneratorWizardState>{
            stepIndex: state.stepIndexHistory[state.stepIndexHistory.length - 1],
            stepIndexHistory: state.stepIndexHistory.slice(0, state.stepIndexHistory.length - 1)
          });
        } else {
          throw new Error('No previous step found');
        }
      }
      case WizardActionsTypes.RESET: {
        return <GeneratorWizardState>{};
      }
      default: {
        return state || <GeneratorWizardState>{};
      }
    }
  }

  function extendState(state: GeneratorWizardState): GeneratorWizardState {
    if (state.id) {
      state.stepsValid = state.stepInputsValid.map(inputsValid => inputsValid.every(x => x));
      state.stepValid = state.stepsValid[state.stepIndex];
      state.valid = state.stepsValid.every(x => x);

      const { values } = state;
      const step = state.steps[state.stepIndex];
      if (!state.step || state.step.id !== step.id) {
        // Don't break object equality, else it will trigger change detection and re-render step.
        state.step = Object.assign({}, step, <TemplateStep>{
          name: InputValueCollectionUtility.getString(step.name, values),
          description: InputValueCollectionUtility.getString(step.description, values),
          inputs: step.inputs.map(i => Object.assign({}, i, <TemplateStepInput>{
            name: i.name ? InputValueCollectionUtility.getString(i.name, values) : i.name,
            description: i.description ? InputValueCollectionUtility.getString(i.description, values) : i.description,
            hint: i.hint ? InputValueCollectionUtility.getString(i.hint, values) : i.hint
          }))
        });
      }

      state.stepValues = {};
      state.step.inputs.forEach(i => {
        state.stepValues[i.id] = state.values[i.id];
      });

      state.nextStepIndex = state.steps.findIndex((s, i) => {
        if (i <= state.stepIndex) {
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

      state.hasNextStep = !state.completed && state.nextStepIndex > -1;
      state.hasPreviousStep = state.stepIndex > 0;

      state.empty = Object.keys(state.values).every(stepId =>
        state.values[stepId] === undefined ||
        state.values[stepId] === null);

      state.progress = (state.stepIndex / (state.steps.length - 1)) * 100;
    }

    return state;
  }

  return generatorWizardReducer;
}
