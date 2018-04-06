import { Action } from '@ngrx/store';

import { Template } from '../../../../../core';

export enum DocumentsActionsTypes {
    BEGIN = '[Generator Bulk Document] Begin'
}

export class Begin implements Action {
    readonly type = DocumentsActionsTypes.BEGIN;
    constructor(public paylod: Template) { }
}

export type DocumentsActions = Begin;
