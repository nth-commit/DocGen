import { Action, MetaReducer } from '@ngrx/store';

import { GeneratorBulkLayoutState, GeneratorBulkLayoutDialogState } from '../../../../../_core';
import { LayoutAction, LayoutActionTypes } from './layout.actions';

export function generatorBulkLayoutReducer(state: GeneratorBulkLayoutState, action: LayoutAction): GeneratorBulkLayoutState {

  const assertDialog = (dialog: string) => {
    if (state.dialog !== dialog) {
      throw new Error('Unexpected dialog');
    }
  };

  const assertDialogState = (dialogState: GeneratorBulkLayoutDialogState) => {
    if (state.dialogState !== dialogState) {
      throw new Error('Unexpected dialog state');
    }
  };

  switch (action.type) {
    case (LayoutActionTypes.OPEN_DIALOG_BEGIN): {
      assertDialogState(null);

      return Object.assign({}, state, <GeneratorBulkLayoutState>{
        dialog: action.payload.dialog,
        dialogState: GeneratorBulkLayoutDialogState.Opening
      });
    }
    case (LayoutActionTypes.OPEN_DIALOG_END): {
      assertDialog(action.payload.dialog);
      assertDialogState(GeneratorBulkLayoutDialogState.Opening);

      return Object.assign({}, state, <GeneratorBulkLayoutState>{
        dialogRef: action.payload.dialogRef,
        dialogState: GeneratorBulkLayoutDialogState.Opened
      });
    }
    case (LayoutActionTypes.CLOSE_DIALOG_BEGIN):
    case (LayoutActionTypes.CANCEL_DIALOG_BEGIN): {
      assertDialog(action.payload.dialog);
      assertDialogState(GeneratorBulkLayoutDialogState.Opened);

      state.dialogRef.close();

      return Object.assign({}, state, <GeneratorBulkLayoutState>{
        dialogRef: null,
        dialogState: action.type === LayoutActionTypes.CLOSE_DIALOG_BEGIN ?
          GeneratorBulkLayoutDialogState.Closing :
          GeneratorBulkLayoutDialogState.Cancelling
      });
    }
    case (LayoutActionTypes.CLOSE_DIALOG_END):
    case (LayoutActionTypes.CANCEL_DIALOG_END): {
      assertDialog(action.payload.dialog);

      if (action.type === LayoutActionTypes.CLOSE_DIALOG_END) {
        assertDialogState(GeneratorBulkLayoutDialogState.Closing);
      } else {
        assertDialogState(GeneratorBulkLayoutDialogState.Cancelling);
      }

      return Object.assign({}, state, <GeneratorBulkLayoutState>{
        dialog: null,
        dialogState: null
      });
    }
    default: {
      return state || <GeneratorBulkLayoutState>{
        dialogState: null
      };
    }
  }
}
