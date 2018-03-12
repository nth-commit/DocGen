export interface IDocumentBuilderV1 {
    result: string;
    beginWriteDocument(): Promise<void>;
    endWriteDocument(): Promise<void>;

    beginWritePage(): Promise<void>;
    endWritePage(): Promise<void>;

    beginWriteList(): Promise<void>;
    endWriteList(): Promise<void>;

    beginWriteListItem(): Promise<void>;
    endWriteListItem(): Promise<void>;

    writeParagraphBreak(): Promise<void>;
    writeBreak(): Promise<void>;
    writeText(text: string, reference: string, conditions: string[]): Promise<void>;
}

