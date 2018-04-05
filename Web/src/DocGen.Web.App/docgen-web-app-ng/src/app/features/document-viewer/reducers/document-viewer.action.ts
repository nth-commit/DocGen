import { Action } from '@ngrx/store';

import { Template } from '../../core';

export enum DocumentViewerActionTypes {
    INIT = '[Document Viewer] Initialize',
    BEGIN_SIGN = '[Document Viewer] Begin signing',
    CANCEL_SIGN = '[Document Viewer] Cancel signing',
    COMPLETE_SIGN_START = '[Document Viewer] Start completing signing',
    COMPLETE_SIGN_END = '[Document Viewer] End completing signing',
    COMPLETE_SIGN_ERROR = '[Document Viewer] Error completing signing'
}

export interface InitPayload {
    template: Template;
}

export class Init implements Action {
    readonly type: string = DocumentViewerActionTypes.INIT;
    constructor(public payload) { }
}

export class BeginSign implements Action {
    readonly type: string = DocumentViewerActionTypes.BEGIN_SIGN;
}

export type DocumentViewerAction = Init | BeginSign;
