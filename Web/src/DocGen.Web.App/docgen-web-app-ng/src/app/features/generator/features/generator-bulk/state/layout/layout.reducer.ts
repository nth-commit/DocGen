import { Action, MetaReducer } from '@ngrx/store';

import { GeneratorBulkLayoutState, GeneratorBulkLayoutDialogState } from '../../../../../_shared';
import { LayoutAction, LayoutActionTypes } from './layout.actions';

export function generatorBulkLayoutReducer(state: GeneratorBulkLayoutState, action: LayoutAction): GeneratorBulkLayoutState {
  switch (action.type) {
    case (LayoutActionTypes.OPEN_DIALOG_BEGIN): {
      return Object.assign({}, state, <GeneratorBulkLayoutState>{
        dialog: action.payload.dialog,
        dialogState: GeneratorBulkLayoutDialogState.Opening
      });
    }
    case (LayoutActionTypes.OPEN_DIALOG_END): {
      return Object.assign({}, state, <GeneratorBulkLayoutState>{
        dialogRef: action.payload.dialogRef,
        dialogState: GeneratorBulkLayoutDialogState.Opened
      });
    }
    case (LayoutActionTypes.CLOSE_DIALOG_BEGIN): {
      state.dialogRef.close();
      return Object.assign({}, state, <GeneratorBulkLayoutState>{
        dialogState: GeneratorBulkLayoutDialogState.Closing
      });
    }
    case (LayoutActionTypes.CLOSE_DIALOG_END): {
      return Object.assign({}, state, <GeneratorBulkLayoutState>{
        dialog: null,
        dialogRef: null,
        dialogState: null
      });
    }
    default: {
      return state || <GeneratorBulkLayoutState>{};
    }
  }
}
