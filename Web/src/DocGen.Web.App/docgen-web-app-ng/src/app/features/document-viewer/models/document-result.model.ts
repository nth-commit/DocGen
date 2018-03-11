import { SerializedDocument } from '../../core';

export type DocumentType = 'text' | 'pdf';

export interface DocumentResult<TDocument = any> {
    type: DocumentType;
    document: TDocument;
}

export class TextDocumentResult implements DocumentResult<string> {
    readonly type = 'text';
    constructor(public document: string) { }
}

export class SerializedDocumentResult implements DocumentResult<SerializedDocument> {
    readonly type = 'pdf';
    constructor(public document: SerializedDocument) { }
}
