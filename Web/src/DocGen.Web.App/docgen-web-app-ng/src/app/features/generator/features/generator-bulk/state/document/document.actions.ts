import { Action } from '@ngrx/store';

import { Template, InputValueCollection } from '../../../../../core';
import { GeneratorWizardState } from '../../../../../_shared';

export enum DocumentActionsTypes {
    BEGIN = '[Generator Bulk Document] Begin',
    UPDATE_DRAFT = '[Generator Bulk Document] Update Draft',
    PUBLISH_DRAFT = '[Generator Bulk Document] Publish Draft',
    UPDATE_CONSTANTS = '[Generator Bulk Document] Update Constants',
    UPDATE_CONSTANTS_BEGIN = '[Generator Bulk Document] Update Constants Begin'
}

export class DocumentBeginAction implements Action {
    readonly type = DocumentActionsTypes.BEGIN;
    constructor(public payload: Template) { }
}

export class DocumentUpdateDraftAction implements Action {
    readonly type = DocumentActionsTypes.UPDATE_DRAFT;
    constructor(public payload: GeneratorWizardState) { }
}

export interface DocumentPublishDraftPayload {
    id: string;
    repeat: boolean;
    clearConstants: boolean;
}

export class DocumentPublishDraftAction implements Action {
    readonly type = DocumentActionsTypes.PUBLISH_DRAFT;
    constructor(public payload: DocumentPublishDraftPayload) { }
}

export class DocumentUpdateConstantsAction implements Action {
    readonly type = DocumentActionsTypes.UPDATE_CONSTANTS;
    constructor(public payload: InputValueCollection) { }
}

export class DocumentUpdateConstantsBeginAction implements Action {
    readonly type = DocumentActionsTypes.UPDATE_CONSTANTS_BEGIN;
    constructor() { }
}

export type DocumentAction =
    DocumentBeginAction |
    DocumentUpdateDraftAction |
    DocumentPublishDraftAction |
    DocumentUpdateConstantsAction |
    DocumentUpdateConstantsBeginAction;
