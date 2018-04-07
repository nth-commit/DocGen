import { DocumentCreate, InputValueCollection, InputValue, Template } from '../../../../../core/';

export interface Document {
    id: string;
    values: InputValueCollection;
}

export interface GeneratorBulkDocumentState {
    template?: Template;
    completedDocuments: Document[];
    draftDocuments: Document[];
}
