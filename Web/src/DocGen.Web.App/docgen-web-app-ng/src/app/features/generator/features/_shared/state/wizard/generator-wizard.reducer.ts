import { Action, MetaReducer } from '@ngrx/store';

import { GeneratorWizardState } from './generator-wizard.state';

export function generatorWizardReducer(state: GeneratorWizardState, action: Action): GeneratorWizardState {
    console.log('Reducer: generatorWizardReducer');
    return {};
}
