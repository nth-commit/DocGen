import { Action } from '@ngrx/store';

import { Template, InputValueCollection } from '../../../../../core';

import { AppAction } from '../../../../../_shared';

export enum WizardActionsTypes {
    BEGIN = '[Wizard2] Begin',
    UPDATE_VALUES = '[Wizard2] Update Values',
    RESET = '[Wizard2] Reset',
    NEXT = '[Wizard2] Next Step',
    PREVIOUS = '[Wizard2] Previous Step'
}

export class WizardBeginAction implements AppAction {
    readonly type = WizardActionsTypes.BEGIN;
    constructor(
        public reducerId: string,
        public payload: { template: Template, presets?: InputValueCollection, showPresetInputs?: boolean }) { }
}

export class WizardUpdateValuesAction implements AppAction {
    readonly type = WizardActionsTypes.UPDATE_VALUES;
    constructor(public reducerId: string, public payload: InputValueCollection) { }
}

export class WizardResetAction implements AppAction {
    readonly type = WizardActionsTypes.RESET;
    constructor(public reducerId: string) { }
}

export class WizardNextAction implements AppAction {
    readonly type = WizardActionsTypes.NEXT;
    constructor(public reducerId: string) { }
}

export class WizardPreviousAction implements AppAction {
    readonly type = WizardActionsTypes.PREVIOUS;
    constructor(public reducerId: string) { }
}

export type WizardAction =
    WizardBeginAction |
    WizardUpdateValuesAction |
    WizardResetAction |
    WizardNextAction |
    WizardPreviousAction;
