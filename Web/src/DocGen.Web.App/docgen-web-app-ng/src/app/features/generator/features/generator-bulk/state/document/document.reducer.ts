import { Action, MetaReducer } from '@ngrx/store';

import { GeneratorBulkDocumentState, GeneratorBulkDocumentRepeatState, Document } from '../../../../../_shared';
import { DocumentAction, DocumentActionsTypes, DocumentBeginAction } from './document.actions';

export function generatorBulkDocumentReducer(state: GeneratorBulkDocumentState, action: DocumentAction): GeneratorBulkDocumentState {
  switch (action.type) {
    case DocumentActionsTypes.BEGIN: {
      return Object.assign({}, state, <GeneratorBulkDocumentState>{
        template: action.payload
      });
    }
    case DocumentActionsTypes.UPDATE_DRAFT: {
      const draftDocument: Document = {
        id: action.payload.id,
        values: action.payload.values
      };

      const existingDraftIndex = state.draftDocuments.findIndex(d => d.id === draftDocument.id);

      return Object.assign({}, state, <GeneratorBulkDocumentState>{
        draftDocuments: existingDraftIndex === -1 ?
          [...state.draftDocuments, draftDocument] :
          state.draftDocuments.replace(existingDraftIndex, draftDocument)
      });
    }
    case DocumentActionsTypes.PUBLISH_DRAFT: {
      const { id, repeat, clearConstants } = action.payload;

      const document = state.draftDocuments.find(d => d.id === id);

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
    case DocumentActionsTypes.UPDATE_CONSTANTS_CANCEL: {
      return Object.assign({}, state, <GeneratorBulkDocumentState>{
        constants: null
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
