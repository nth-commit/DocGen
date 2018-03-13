export interface IDocumentBuilderV1 {
    result: string;
    beginWriteDocument(instructionId: number): Promise<void> | void;
    endWriteDocument(instructionId: number): Promise<void> | void;

    beginWritePage(instructionId: number): Promise<void> | void;
    endWritePage(instructionId: number): Promise<void> | void;

    beginWriteList(instructionId: number): Promise<void> | void;
    endWriteList(instructionId: number): Promise<void> | void;

    beginWriteListItem(instructionId: number, indexPath: number[]): Promise<void> | void;
    endWriteListItem(instructionId: number): Promise<void> | void;

    writeParagraphBreak(instructionId: number): Promise<void> | void;
    writeBreak(instructionId: number): Promise<void> | void;
    writeText(instructionId: number, text: string, reference: string, conditions: string[]): Promise<void> | void;
}

