import { SerializableDocument } from '../../../_core';
import { DocumentType } from '../../models';

export interface IDocumentRenderer {
    render(document: SerializableDocument, documentType: DocumentType): Promise<string>;
}
