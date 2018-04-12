import { GeneratorWizardState } from './_shared/generator-wizard.state';
import { Template, InputValueCollection } from '../../../core';
import { MatDialogRef } from '@angular/material';

export interface GeneratorBulkLayoutState {
  dialog?: string;
  dialogRef?: MatDialogRef<any>;
  dialogState?: GeneratorBulkLayoutDialogState;
}

export enum GeneratorBulkLayoutDialogState {
  Opening,
  Opened,
  Closing
}

export enum GeneratorBulkDocumentRepeatState {
  Started
}

export interface GeneratorBulkDocumentState {
  template?: Template;
  constants?: InputValueCollection;
  lastCompletedDocument?: Document;
  completedDocuments: Document[];
  draftDocuments: Document[];
  repeatState?: GeneratorBulkDocumentRepeatState;
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
