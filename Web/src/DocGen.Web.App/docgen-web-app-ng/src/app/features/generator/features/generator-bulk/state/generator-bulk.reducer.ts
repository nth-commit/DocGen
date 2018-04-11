import { combineReducers, Action } from '@ngrx/store';

import { GeneratorBulkState } from '../../../../_shared';

import { createGeneratorWizardReducer } from '../../_shared';
import { generatorBulkLayoutReducer } from './layout';
import { generatorBulkDocumentReducer } from './document';
import { REDUCER_ID } from './constants';

export const generatorBulkReducer = combineReducers<GeneratorBulkState, Action>(
  {
    wizard: createGeneratorWizardReducer(REDUCER_ID),
    layout: generatorBulkLayoutReducer,
    documents: generatorBulkDocumentReducer
  }
);
