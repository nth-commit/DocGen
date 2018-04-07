import { Action } from '@ngrx/store';

import { Template, InputValueCollection } from '../../../../../core';

import { AppAction } from '../../../../../_shared';

export enum WizardActionsTypes {
    BEGIN = '[Wizard2] Begin',
    UPDATE_VALUES = '[Wizard2] Update Values',
    DISCARD = '[Wizard2] Discard',
    NEXT_STEP = '[Wizard2] Next Step',
    PREVIOUS_STEP = '[Wizard2] Previous Step',
    COMPLETE = '[Wizard2] Complete'
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
    readonly type = WizardActionsTypes.NEXT_STEP;
    constructor(public reducerId: string) { }
}

export class WizardPreviousStepAction implements AppAction {
    readonly type = WizardActionsTypes.PREVIOUS_STEP;
    constructor(public reducerId: string) { }
}

export class WizardCompleteStepAction implements AppAction {
    readonly type = WizardActionsTypes.COMPLETE;
    constructor(public reducerId: string) { }
}

export type WizardAction =
    WizardBeginAction |
    WizardUpdateValuesAction |
    WizardDiscardAction |
    WizardNextStepAction |
    WizardPreviousStepAction |
    WizardCompleteStepAction;
