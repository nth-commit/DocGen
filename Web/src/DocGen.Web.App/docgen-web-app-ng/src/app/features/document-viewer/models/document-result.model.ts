import { SerializableDocument } from '../../core';

export type DocumentType = 'text' | 'pdf';

export interface DocumentResult<TDocument = any> {
    type: DocumentType;
    document: TDocument;
}

export class TextDocumentResult implements DocumentResult<string> {
    readonly type = 'text';
    constructor(public document: string) { }
}

export class SerializableDocumentResult implements DocumentResult<SerializableDocument> {
    readonly type = 'pdf';
    constructor(public document: SerializableDocument) { }
}
