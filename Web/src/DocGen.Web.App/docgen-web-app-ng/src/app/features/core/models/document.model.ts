export interface SerializableDocument {
    markupVersion: number;
    instructions: Instruction[];
    isSigned: boolean;
}

export interface Instruction {
    type: InstructionType;
}

export interface BeginWritePageInstruction extends Instruction { }

export interface EndWritePageInstruction extends Instruction { }

export interface BeginWriteListInstruction extends Instruction { }

export interface EndWriteListInstruction extends Instruction { }

export interface BeginWriteListItemInstruction extends Instruction {
    indexPath: number[];
}

export interface EndWriteListItemInstruction extends Instruction { }

export interface WriteParagraphBreakInstruction extends Instruction { }

export interface WriteBreakInstruction extends Instruction { }

export interface WriteTextInstruction extends Instruction {
    text: string;
    reference: string;
    conditions: string[];
}

export interface WriteSignaturePlaceholder extends Instruction { }

export enum InstructionType {
    BeginWritePage = 1,

    EndWritePage,

    BeginWriteList,

    EndWriteList,

    BeginWriteListItem,

    EndWriteListItem,

    WriteParagraphBreak,

    // TODO
    // Currently unsupported by template
    // Need to differentiate between this and paragraph break. Break = shift + enter in word, paragrah break = just enter
    WriteBreak,

    WriteText,

    // TODO
    // Currently unsupported by template
    WriteSignaturePlaceholder
}

export function isBeginWritePage(instruction: Instruction): instruction is BeginWritePageInstruction {
    return instruction.type === InstructionType.BeginWritePage;
}

export function isEndWritePage(instruction: Instruction): instruction is EndWritePageInstruction {
    return instruction.type === InstructionType.EndWritePage;
}

export function isBeginWriteList(instruction: Instruction): instruction is BeginWriteListInstruction {
    return instruction.type === InstructionType.BeginWriteList;
}

export function isEndWriteList(instruction: Instruction): instruction is EndWriteListInstruction {
    return instruction.type === InstructionType.EndWriteList;
}

export function isBeginWriteListItem(instruction: Instruction): instruction is BeginWriteListItemInstruction {
    return instruction.type === InstructionType.BeginWriteListItem;
}

export function isEndWriteListItem(instruction: Instruction): instruction is EndWriteListItemInstruction {
    return instruction.type === InstructionType.EndWriteListItem;
}

export function isWriteParagraphBreak(instruction: Instruction): instruction is WriteParagraphBreakInstruction {
    return instruction.type === InstructionType.WriteParagraphBreak;
}

export function isWriteBreak(instruction: Instruction): instruction is WriteBreakInstruction {
    return instruction.type === InstructionType.WriteBreak;
}

export function isWriteText(instruction: Instruction): instruction is WriteTextInstruction {
    return instruction.type === InstructionType.WriteText;
}
