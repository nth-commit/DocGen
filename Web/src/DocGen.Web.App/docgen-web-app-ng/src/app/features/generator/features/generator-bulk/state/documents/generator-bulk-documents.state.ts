import { DocumentCreate, InputValueCollection, InputValue, Template } from '../../../../../core/';

export interface Document extends DocumentCreate {}

export interface GeneratorBulkDocumentsState {
    template?: Template;
    savedDocuments: Document[];
    draftDocument?: Document;
}
