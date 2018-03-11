import { SerializableDocument } from '../../../core';

export interface IDocumentRenderer {
    render(document: SerializableDocument): Promise<string>;
}
