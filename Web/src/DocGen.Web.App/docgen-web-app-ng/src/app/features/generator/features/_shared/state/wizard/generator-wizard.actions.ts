import { Action } from '@ngrx/store';

import { Template } from '../../../../../core';

import { AppAction } from '../../../../../_shared';

export enum WizardActionsTypes {
    BEGIN = '[Wizard2] Begin'
}

export abstract class WizardAction implements AppAction {
    abstract type: string;
    constructor(public reducerId: string) { }
}

export interface WizardBeginPayload {
    template: Template;
}

export class WizardBeginAction extends WizardAction {
    readonly type = WizardActionsTypes.BEGIN;
    constructor(reducerId: string, public paylod: WizardBeginPayload) { super(reducerId); }
}
