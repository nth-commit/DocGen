import { SerializableDocument, InputValueCollection, HtmlDocument } from '../../core';

export type DocumentType = 'text' | 'pdf' | 'html';

export interface DocumentResult<TDocument = any> {
    type: DocumentType;
    document: TDocument;
    inputValues: InputValueCollection;
    correlationId: string;
}

export class TextDocumentResult implements DocumentResult<string> {
    readonly type = 'text';
    constructor(
        public document: string,
        public inputValues: InputValueCollection,
        public correlationId: string) { }
}

export class SerializableDocumentResult implements DocumentResult<SerializableDocument> {
    readonly type = 'pdf';
    constructor(
        public document: SerializableDocument,
        public inputValues: InputValueCollection,
        public correlationId: string) { }
}

export class HtmlDocumentResult implements DocumentResult<HtmlDocument> {
    readonly type = 'html';
    constructor(
        public document: HtmlDocument,
        public inputValues: InputValueCollection,
        public correlationId: string) { }
}
