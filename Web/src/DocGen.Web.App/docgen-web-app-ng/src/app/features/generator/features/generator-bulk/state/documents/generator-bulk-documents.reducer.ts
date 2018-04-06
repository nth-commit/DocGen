import { Action, MetaReducer } from '@ngrx/store';

import { GeneratorBulkDocumentsState } from './generator-bulk-documents.state';
import { DocumentsActions, DocumentsActionsTypes, Begin } from './generator-bulk-documents.actions';

export function generatorBulkDocumentsReducer(state: GeneratorBulkDocumentsState, action: DocumentsActions): GeneratorBulkDocumentsState {
    switch (action.type) {
        case DocumentsActionsTypes.BEGIN: {
            return Object.assign({}, state, <GeneratorBulkDocumentsState>{
                template: action.paylod
            });
        }
        default: {
            return {
                savedDocuments: []
            };
        }
    }
}
