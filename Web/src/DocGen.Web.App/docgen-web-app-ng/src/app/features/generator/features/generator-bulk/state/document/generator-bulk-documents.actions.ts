import { Action } from '@ngrx/store';

import { Template } from '../../../../../core';
import { GeneratorWizardState } from '../../../../../_shared';

export enum DocumentActionsTypes {
    BEGIN = '[Generator Bulk Document] Begin',
    UPDATE_DRAFT = '[Generator Bulk Document] Update Draft',
    PUBLISH_DRAFT = '[Generator Bulk Document] Publish Draft'
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
}

export class DocumentPublishDraftAction implements Action {
    readonly type = DocumentActionsTypes.PUBLISH_DRAFT;
    constructor(public payload: DocumentPublishDraftPayload) { }
}

export type DocumentAction = DocumentBeginAction | DocumentUpdateDraftAction | DocumentPublishDraftAction;
