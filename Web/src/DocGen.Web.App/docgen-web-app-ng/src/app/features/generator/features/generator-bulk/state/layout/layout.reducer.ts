import { Action, MetaReducer } from '@ngrx/store';

import { GeneratorBulkLayoutState, GeneratorBulkLayoutDialogState } from '../../../../../_shared';
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
    case (LayoutActionTypes.CLOSE_DIALOG_BEGIN): {
      assertDialog(action.payload.dialog);
      assertDialogState(GeneratorBulkLayoutDialogState.Opened);

      state.dialogRef.close();

      return Object.assign({}, state, <GeneratorBulkLayoutState>{
        dialogRef: null,
        dialogState: GeneratorBulkLayoutDialogState.Closing
      });
    }
    case (LayoutActionTypes.CLOSE_DIALOG_END): {
      assertDialog(action.payload.dialog);
      assertDialogState(GeneratorBulkLayoutDialogState.Closing);

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
