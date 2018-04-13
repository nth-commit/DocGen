import { InputValueCollection } from '../../models';

export interface DocumentCreate {
    templateId: string;
    templateVersion: number;
    templateName: string;
    inputValues: InputValueCollection;
}

export interface DocumentResult extends DocumentCreate {
    correlationId: string;
}

export interface IDocumentService {
    createOrUpdate(correlationId: string, document: DocumentCreate): Promise<DocumentResult>;
    list(): Promise<DocumentResult[]>;
}