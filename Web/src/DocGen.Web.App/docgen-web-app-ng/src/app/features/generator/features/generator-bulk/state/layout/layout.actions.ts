import { MatDialogRef } from '@angular/material';
import { Action } from '@ngrx/store';

export enum LayoutActionTypes {
  OPEN_DIALOG_BEGIN = '[Generator Bulk Layout] Open Dialog Begin',
  OPEN_DIALOG_END = '[Generator Bulk Layout] Open Dialog End',
  CLOSE_DIALOG_BEGIN = '[Generator Bulk Layout] Close Dialog Begin',
  CLOSE_DIALOG_END = '[Generator Bulk Layout] Close Dialog End'
}

export class LayoutOpenDialogBeginAction {
  readonly type = LayoutActionTypes.OPEN_DIALOG_BEGIN;
  constructor(public payload: { dialog: 'wizard' }) { }
}

export class LayoutOpenDialogEndAction {
  readonly type = LayoutActionTypes.OPEN_DIALOG_END;
  constructor(public payload: { dialogRef: MatDialogRef<any> }) { }
}

export class LayoutCloseDialogBeginAction {
  readonly type = LayoutActionTypes.CLOSE_DIALOG_BEGIN;
  constructor() { }
}

export class LayoutCloseDialogEndAction {
  readonly type = LayoutActionTypes.CLOSE_DIALOG_END;
  constructor() { }
}

export type LayoutAction =
  LayoutOpenDialogBeginAction |
  LayoutOpenDialogEndAction |
  LayoutCloseDialogBeginAction |
  LayoutCloseDialogEndAction;
