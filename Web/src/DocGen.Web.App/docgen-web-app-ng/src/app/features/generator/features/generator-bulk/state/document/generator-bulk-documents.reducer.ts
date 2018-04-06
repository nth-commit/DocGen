import { Action, MetaReducer } from '@ngrx/store';

import { GeneratorBulkDocumentState } from './generator-bulk-documents.state';
import { DocumentActions, DocumentActionsTypes, Begin } from './generator-bulk-documents.actions';

export function generatorBulkDocumentReducer(state: GeneratorBulkDocumentState, action: DocumentActions): GeneratorBulkDocumentState {
    switch (action.type) {
        case DocumentActionsTypes.BEGIN: {
            return Object.assign({}, state, <GeneratorBulkDocumentState>{
                template: action.paylod
            });
        }
        case DocumentActionsTypes.BEGIN_DRAFT: {
            return state;
        }
        default: {
            return {
                completedDocuments: [],
                draftDocuments: []
            };
        }
    }
}
