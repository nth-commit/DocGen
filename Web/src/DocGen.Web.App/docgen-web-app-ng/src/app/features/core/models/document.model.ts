export interface SerializableDocument {
    instructions: Instruction[];
}

export interface Instruction<TBody = any> {
    elementType: ElementType;
    writeType: WriteType;
    conditions: string[];
    body?: TBody;
}

export interface TextInstruction extends Instruction<TextInstructionBody> {}

export interface TextInstructionBody {
    text: string;
    reference?: string;
}

export enum ElementType {
    Page = 1,
    Block,
    Text,
    List,
    ListItem
}

export enum WriteType {
    Write = 1,
    BeginWrite,
    EndWrite
}
