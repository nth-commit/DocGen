import { Action, MetaReducer } from '@ngrx/store';

import { GeneratorBulkDocumentState, GeneratorBulkDocumentRepeatState, Document } from '../../../../../_core';
import { DocumentAction, DocumentActionsTypes, DocumentBeginAction } from './document.actions';

export function generatorBulkDocumentReducer(state: GeneratorBulkDocumentState, action: DocumentAction): GeneratorBulkDocumentState {
  let newState = resolveState(state, action);
  if (newState !== state) {
    newState = extendState(newState);
  }
  return newState;
}

export function resolveState(state: GeneratorBulkDocumentState, action: DocumentAction): GeneratorBulkDocumentState {
  switch (action.type) {
    case DocumentActionsTypes.BEGIN: {
      return Object.assign({}, state, <GeneratorBulkDocumentState>{
        template: action.payload
      });
    }
    case DocumentActionsTypes.UPDATE_DOCUMENT: {
      const draftDocument: Document = {
        id: action.payload.id,
        values: action.payload.values,
        constants: Object.assign({}, state.constants)
      };

      let { draftDocuments, completedDocuments } = state;

      const existingDraftIndex = state.draftDocuments.findIndex(d => d.id === draftDocument.id);
      const existingCompletedIndex = state.completedDocuments.findIndex(d => d.id === draftDocument.id);

      if (existingDraftIndex === -1 && existingCompletedIndex === -1) {
        draftDocuments = [...draftDocuments, draftDocument];
      } else if (existingDraftIndex === -1) {
        completedDocuments = completedDocuments.filter(d => d.id !== draftDocument.id);
        draftDocuments = [...draftDocuments, draftDocument];
      } else {
        draftDocuments = draftDocuments.replace(existingDraftIndex, draftDocument);
      }

      return Object.assign({}, state, <GeneratorBulkDocumentState>{
        draftDocuments,
        completedDocuments
      });
    }
    case DocumentActionsTypes.PUBLISH_DOCUMENT: {
      const { id, repeat, clearConstants } = action.payload;

      const document = state.draftDocuments.find(d => d.id === id);
      document.creationTime = new Date();

      return Object.assign({}, state, <GeneratorBulkDocumentState>{
        draftDocuments: state.draftDocuments.filter(d => d.id !== id),
        completedDocuments: [...state.completedDocuments, document],
        repeating: repeat,
        constants: clearConstants ? null : state.constants,
        lastCompletedDocument: document,
        lastConstants: state.constants
      });
    }
    case DocumentActionsTypes.UPDATE_CONSTANTS: {
      return Object.assign({}, state, <GeneratorBulkDocumentState>{
        constants: action.payload
      });
    }
    default: {
      return state || {
        completedDocuments: [],
        draftDocuments: []
      };
    }
  }
}

export function extendState(state: GeneratorBulkDocumentState): GeneratorBulkDocumentState {
  if (state) {
    state.documentsById = Map.fromArray(
      [...state.draftDocuments, ...state.completedDocuments],
      d => d.id,
      d => d
    );
  }
  return state;
}
