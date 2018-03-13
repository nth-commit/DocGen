export interface IDocumentBuilderV1 {
    result: string;
    beginWriteDocument(): Promise<void> | void;
    endWriteDocument(): Promise<void> | void;

    beginWritePage(): Promise<void> | void;
    endWritePage(): Promise<void> | void;

    beginWriteList(): Promise<void> | void;
    endWriteList(): Promise<void> | void;

    beginWriteListItem(indexPath: number[]): Promise<void> | void;
    endWriteListItem(): Promise<void> | void;

    writeParagraphBreak(): Promise<void> | void;
    writeBreak(): Promise<void> | void;
    writeText(text: string, reference: string, conditions: string[]): Promise<void> | void;
}

