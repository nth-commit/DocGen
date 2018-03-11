export interface IDocumentBuilderV1 {
    result: string;
    beginWriteDocument(): Promise<void>;
    endWriteDocument(): Promise<void>;
    beginPageDocument(): Promise<void>;
    endPageDocument(): Promise<void>;
    writeText(text: string, reference: string, conditional): Promise<void>;
}
