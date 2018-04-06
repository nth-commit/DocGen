import { DocumentCreate, InputValueCollection, InputValue, Template } from '../../../../../core/';

export interface Document extends DocumentCreate {}

export interface GeneratorBulkDocumentState {
    template?: Template;
    completedDocuments: Document[];
    draftDocuments: Document[];
}
