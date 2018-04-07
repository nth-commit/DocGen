import { Action, ActionReducer } from '@ngrx/store';

import {
  Template, TemplateStep, TemplateStepInput, TemplateStepConditionType,
  InputValueCollection, InputValueCollectionUtility
} from '../../../../../core';

import { GeneratorWizardState } from './generator-wizard.state';
import { WizardActionsTypes, WizardAction, WizardBeginAction, WizardDiscardAction } from './generator-wizard.actions';

export function createGeneratorWizardReducer(id: string): ActionReducer<GeneratorWizardState, WizardAction> {

  function generatorWizardReducer(state: GeneratorWizardState, action: WizardAction) {
    if (action.reducerId !== id) {
      return state;
    }
    return extendState(resolveState(state, action));
  }

  function resolveState(state: GeneratorWizardState, action: WizardAction): GeneratorWizardState {
    switch (action.type) {
      case WizardActionsTypes.BEGIN: {
        const { template } = action.payload;

        // Ignore steps after signing for now. Previous wizard reducer has example of how to handle them. But is it necessary? Should we
        // worry about signing info at a later time?
        const fistStepIndexWithSignedCondition = template.steps.findIndex(s =>
          s.conditions.some(c => c.type === TemplateStepConditionType.IsDocumentSigned));
        const steps = template.steps.slice(0, fistStepIndexWithSignedCondition);

        const stepInputsValid = steps.map(s => s.inputs.map(i => false));

        const values = InputValueCollectionUtility.fromSteps(steps);

        return Object.assign({}, state, <GeneratorWizardState>{
          id: new Date().getTime().toString(),

          steps,
          stepIndex: 0,
          stepIndexHistory: [],

          stepInputsValid,
          values
        });
      }
      default: {
        return state || {} as GeneratorWizardState;
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
      state.step = Object.assign({}, step, <TemplateStep>{
        name: InputValueCollectionUtility.getString(step.name, values),
        description: InputValueCollectionUtility.getString(step.description, values),
        inputs: step.inputs.map(i => Object.assign({}, i, <TemplateStepInput>{
          name: i.name ? InputValueCollectionUtility.getString(i.name, values) : i.name,
          description: i.description ? InputValueCollectionUtility.getString(i.description, values) : i.description,
          hint: i.hint ? InputValueCollectionUtility.getString(i.hint, values) : i.hint
        }))
      });

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

      state.hasNextStep = true; // state.nextStepIndex > -1; // Always show next step to navigate to custom complete view.
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
