import { Action, ActionReducer } from '@ngrx/store';

import { GeneratorWizardState } from './generator-wizard.state';
import { WizardActionsTypes, WizardAction, WizardBeginAction } from './generator-wizard.actions';

export function createGeneratorWizardReducer(id: string): ActionReducer<GeneratorWizardState, WizardAction> {
  function resolveState(state: GeneratorWizardState, action: WizardAction): GeneratorWizardState {

    return {};
  }

  function extendState(state: GeneratorWizardState): GeneratorWizardState {
    return state;
  }

  return function generatorWizardReducer(state: GeneratorWizardState, action: WizardAction) {
    if (action.reducerId !== id) {
      return;
    }

    return extendState(resolveState(state, action));
  };
}
