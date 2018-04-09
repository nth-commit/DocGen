import { Action } from '@ngrx/store';

import { Template, InputValueCollection } from '../../../../../core';

import { AppAction } from '../../../../../_shared';

export enum WizardActionsTypes {
    BEGIN = '[Wizard2] Begin',
    UPDATE_VALUES = '[Wizard2] Update Values',
    DISCARD = '[Wizard2] Discard',
    NEXT = '[Wizard2] Next Step',
    PREVIOUS = '[Wizard2] Previous Step'
}

export class WizardBeginAction implements AppAction {
    readonly type = WizardActionsTypes.BEGIN;
    constructor(public reducerId: string, public payload: { template: Template }) { }
}

export class WizardUpdateValuesAction implements AppAction {
    readonly type = WizardActionsTypes.UPDATE_VALUES;
    constructor(public reducerId: string, public payload: InputValueCollection) { }
}

export class WizardDiscardAction implements AppAction {
    readonly type = WizardActionsTypes.DISCARD;
    constructor(public reducerId: string) { }
}

export class WizardNextStepAction implements AppAction {
    readonly type = WizardActionsTypes.NEXT;
    constructor(public reducerId: string) { }
}

export class WizardPreviousStepAction implements AppAction {
    readonly type = WizardActionsTypes.PREVIOUS;
    constructor(public reducerId: string) { }
}

export type WizardAction =
    WizardBeginAction |
    WizardUpdateValuesAction |
    WizardDiscardAction |
    WizardNextStepAction |
    WizardPreviousStepAction;
