import { Action } from '@ngrx/store';

import { Template } from '../../../../../core';

export enum DocumentActionsTypes {
    BEGIN = '[Generator Bulk Document] Begin',
    BEGIN_DRAFT = '[Generator Bulk Document] Begin Draft'
}

export class Begin implements Action {
    readonly type = DocumentActionsTypes.BEGIN;
    constructor(public paylod: Template) { }
}

export class BeginDraft implements Action {
    readonly type = DocumentActionsTypes.BEGIN_DRAFT;
    constructor() { }
}

export type DocumentActions = Begin | BeginDraft;
