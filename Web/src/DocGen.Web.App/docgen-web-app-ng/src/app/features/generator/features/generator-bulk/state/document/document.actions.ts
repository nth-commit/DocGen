import { Action } from '@ngrx/store';

import { Template, InputValueCollection } from '../../../../../_core';
import { GeneratorWizardState } from '../../../../../_core';

export enum DocumentActionsTypes {
    BEGIN = '[Generator Bulk Document] Begin',
    UPDATE_DOCUMENT = '[Generator Bulk Document] Update Document',
    PUBLISH_DOCUMENT = '[Generator Bulk Document] Publish Document',
    EDIT_DOCUMENT = '[Generator Bulk Document] Edit Document',
    UPDATE_CONSTANTS = '[Generator Bulk Document] Update Constants',
    UPDATE_CONSTANTS_BEGIN = '[Generator Bulk Document] Update Constants Begin',
    UPDATE_CONSTANTS_CANCEL = '[Generator Bulk Document] Update Constants Cancel',
    DELETE_DOCUMENT = '[Generator Bulk Document] Delete Document',
    CREATE_FROM_DOCUMENT = '[Generator Bulk Document] Create From Document'
}

export class DocumentBeginAction implements Action {
    readonly type = DocumentActionsTypes.BEGIN;
    constructor(public payload: Template) { }
}

export class DocumentUpdateDocumentAction implements Action {
    readonly type = DocumentActionsTypes.UPDATE_DOCUMENT;
    constructor(public payload: GeneratorWizardState) { }
}

export interface DocumentPublishDocumentPayload {
    id: string;
    repeat: boolean;
    clearConstants: boolean;
}

export class DocumentPublishDocumentAction implements Action {
    readonly type = DocumentActionsTypes.PUBLISH_DOCUMENT;
    constructor(public payload: DocumentPublishDocumentPayload) { }
}

export class DocumentUpdateConstantsAction implements Action {
    readonly type = DocumentActionsTypes.UPDATE_CONSTANTS;
    constructor(public payload: InputValueCollection) { }
}

export class DocumentUpdateConstantsBeginAction implements Action {
    readonly type = DocumentActionsTypes.UPDATE_CONSTANTS_BEGIN;
    constructor() { }
}

export class DocumentUpdateConstantsCancelAction implements Action {
    readonly type = DocumentActionsTypes.UPDATE_CONSTANTS_CANCEL;
    constructor() { }
}

export class DocumentDeleteDocumentAction implements Action {
    readonly type = DocumentActionsTypes.DELETE_DOCUMENT;
    constructor(public payload: { id: string }) { }
}

export class DocumentCreateFromAction implements Action {
    readonly type = DocumentActionsTypes.CREATE_FROM_DOCUMENT;
    constructor(public payload: { id: string }) { }
}

export type DocumentAction =
    DocumentBeginAction |
    DocumentUpdateDocumentAction |
    DocumentPublishDocumentAction |
    DocumentUpdateConstantsAction |
    DocumentUpdateConstantsBeginAction |
    DocumentUpdateConstantsCancelAction |
    DocumentDeleteDocumentAction |
    DocumentCreateFromAction;
