import { GeneratorWizardState } from './_shared/generator-wizard.state';
import { Template, InputValueCollection } from '../../../core';

export interface GeneratorBulkDocumentState {
  template?: Template;
  lastCompletedDocument?: Document;
  completedDocuments: Document[];
  draftDocuments: Document[];
}

export interface Document {
  id: string;
  values: InputValueCollection;
}


export interface GeneratorBulkState {
  wizard: GeneratorWizardState;
  documents: GeneratorBulkDocumentState;
}
