import { Action, MetaReducer } from '@ngrx/store';

import { GeneratorBulkDocumentState, Document } from './generator-bulk-documents.state';
import { DocumentAction, DocumentActionsTypes, DocumentBeginAction } from './generator-bulk-documents.actions';

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
    default: {
      return state || {
        completedDocuments: [],
        draftDocuments: []
      };
    }
  }
}
