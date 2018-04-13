import { GeneratorWizardState } from './_shared/generator-wizard.state';
import { Template, InputValueCollection } from '../../../_core';
import { MatDialogRef } from '@angular/material';

export interface GeneratorBulkLayoutState {
  dialog?: string;
  dialogRef?: MatDialogRef<any>;
  dialogState?: GeneratorBulkLayoutDialogState;
}

export enum GeneratorBulkLayoutDialogState {
  Unknown = 0,
  Opening,
  Opened,
  Closing,
  Cancelling
}

export enum GeneratorBulkDocumentRepeatState {
  Unknown = 0,
  BeforeRepeat,
  Repeating
}

export interface GeneratorBulkDocumentState {
  template?: Template;
  constants?: InputValueCollection;
  completedDocuments: Document[];
  draftDocuments: Document[];
  repeating?: boolean;
  lastCompletedDocument?: Document;
  lastConstants?: InputValueCollection;
}

export interface Document {
  id: string;
  values: InputValueCollection;
}

export interface GeneratorBulkState {
  layout: GeneratorBulkLayoutState;
  wizard: GeneratorWizardState;
  documents: GeneratorBulkDocumentState;
}
