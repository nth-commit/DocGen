import { MatDialogRef } from '@angular/material';
import { Action } from '@ngrx/store';

export enum LayoutActionTypes {
  OPEN_DIALOG_BEGIN = '[Generator Bulk Layout] Open Dialog Begin',
  OPEN_DIALOG_END = '[Generator Bulk Layout] Open Dialog End',
  CLOSE_DIALOG_BEGIN = '[Generator Bulk Layout] Close Dialog Begin',
  CLOSE_DIALOG_END = '[Generator Bulk Layout] Close Dialog End',
  CANCEL_DIALOG_BEGIN = '[Generator Bulk Layout] Cancel Dialog Begin',
  CANCEL_DIALOG_END = '[Generator Bulk Layout] Cancel Dialog End'
}

export interface LayoutDialogActionPayload {
  dialog: string;
}

export class LayoutOpenDialogBeginAction {
  readonly type = LayoutActionTypes.OPEN_DIALOG_BEGIN;
  constructor(public payload: LayoutDialogActionPayload) { }
}

export interface LayoutOpenDialogEndActionPayload extends LayoutDialogActionPayload {
  dialogRef: MatDialogRef<any>;
}

export class LayoutOpenDialogEndAction {
  readonly type = LayoutActionTypes.OPEN_DIALOG_END;
  constructor(public payload: LayoutOpenDialogEndActionPayload) { }
}

export class LayoutCloseDialogBeginAction {
  readonly type = LayoutActionTypes.CLOSE_DIALOG_BEGIN;
  constructor(public payload: LayoutDialogActionPayload) { }
}

export class LayoutCloseDialogEndAction {
  readonly type = LayoutActionTypes.CLOSE_DIALOG_END;
  constructor(public payload: LayoutDialogActionPayload) { }
}

export class LayoutCancelDialogBeginAction {
  readonly type = LayoutActionTypes.CANCEL_DIALOG_BEGIN;
  constructor(public payload: LayoutDialogActionPayload) { }
}

export class LayoutCancelDialogEndAction {
  readonly type = LayoutActionTypes.CANCEL_DIALOG_END;
  constructor(public payload: LayoutDialogActionPayload) { }
}

export type LayoutDialogAction =
  LayoutOpenDialogBeginAction |
  LayoutOpenDialogEndAction |
  LayoutCloseDialogBeginAction |
  LayoutCloseDialogEndAction |
  LayoutCancelDialogBeginAction |
  LayoutCancelDialogEndAction;

export type LayoutAction =
  LayoutDialogAction;
