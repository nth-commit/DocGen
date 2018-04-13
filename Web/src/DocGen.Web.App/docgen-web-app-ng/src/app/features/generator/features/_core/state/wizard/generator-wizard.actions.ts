import { Action } from '@ngrx/store';

import { Template, InputValueCollection } from '../../../../../_core';

import { AppAction } from '../../../../../_core';

export enum WizardActionsTypes {
    BEGIN = '[Generator Wizard] Begin',
    RESUME = '[Generator Wizard] Resume',
    UPDATE_VALUES = '[Generator Wizard] Update Values',
    RESET = '[Generator Wizard] Reset',
    NEXT = '[Generator Wizard] Next Step',
    PREVIOUS = '[Generator Wizard] Previous Step'
}

export interface WizardBeginActionPayload {
    template: Template;
    presets?: InputValueCollection;
    showPresetInputs?: boolean;
}

export class WizardBeginAction implements AppAction {
    readonly type = WizardActionsTypes.BEGIN;
    constructor(
        public reducerId: string,
        public payload: WizardBeginActionPayload
    ) { }
}

export interface WizardResumeActionPayload extends WizardBeginActionPayload {
    id: string;
    values: InputValueCollection;
}

export class WizardResumeAction implements AppAction {
    readonly type = WizardActionsTypes.RESUME;
    constructor(
        public reducerId: string,
        public payload: WizardResumeActionPayload
    ) { }
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
    WizardResumeAction |
    WizardUpdateValuesAction |
    WizardResetAction |
    WizardNextAction |
    WizardPreviousAction;
