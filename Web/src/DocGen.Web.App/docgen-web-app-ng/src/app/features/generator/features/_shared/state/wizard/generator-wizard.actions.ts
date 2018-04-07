import { Action } from '@ngrx/store';

import { Template } from '../../../../../core';

import { AppAction } from '../../../../../_shared';

export enum WizardActionsTypes {
    BEGIN = '[Wizard2] Begin',
    DISCARD = '[Wizard2] Discard'
}

export class WizardBeginAction implements AppAction {
    readonly type = WizardActionsTypes.BEGIN;
    constructor(public reducerId: string, public payload: { template: Template }) { }
}

export class WizardDiscardAction implements AppAction {
    readonly type = WizardActionsTypes.DISCARD;
    constructor(public reducerId: string) { }
}

export type WizardAction = WizardBeginAction | WizardDiscardAction;
