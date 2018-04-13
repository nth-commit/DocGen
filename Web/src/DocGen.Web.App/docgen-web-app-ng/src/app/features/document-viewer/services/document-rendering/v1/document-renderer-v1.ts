import {
  SerializableDocument,
  Instruction,
  InstructionType,

  BeginWriteListInstruction, BeginWriteListItemInstruction, BeginWritePageInstruction,
  EndWriteListInstruction, EndWriteListItemInstruction, EndWritePageInstruction,
  WriteParagraphBreakInstruction, WriteTextInstruction,

  isBeginWriteList, isBeginWriteListItem, isBeginWritePage,
  isEndWriteList, isEndWriteListItem, isEndWritePage,
  isWriteParagraphBreak, isWriteText
} from '../../../../_core';

import { IDocumentRenderer } from '../document-renderer.interface';
import { IDocumentBuilderV1 } from './document-builder-v1.interface';
import { PdfDocumentBuilderV1 } from './builders/pdf/pdf-document-builder-v1';

export class DocumentRendererV1 implements IDocumentRenderer {

  async render(document: SerializableDocument, documentType: string): Promise<string> {
    let builder: IDocumentBuilderV1 = null;

    if (documentType === 'pdf') {
      builder = new PdfDocumentBuilderV1();
    } else {
      throw new Error('Invalid operation; unrecognized document type');
    }

    let instructionId = 0;

    instructionId++;
    await this.invokeInstruction(() => builder.beginWriteDocument(instructionId));

    document.instructions.forEach(async instruction => {
      instructionId++;

      if (isBeginWritePage(instruction)) {
        await this.invokeInstruction(() => builder.beginWritePage(instructionId));
      }

      if (isBeginWriteList(instruction)) {
        await this.invokeInstruction(() => builder.beginWriteList(instructionId));
      }

      if (isBeginWriteListItem(instruction)) {
        await this.invokeInstruction(() => builder.beginWriteListItem(instructionId, instruction.indexPath));
      }

      if (isEndWritePage(instruction)) {
        await this.invokeInstruction(() => builder.endWritePage(instructionId));
      }

      if (isEndWriteList(instruction)) {
        await this.invokeInstruction(() => builder.endWriteList(instructionId));
      }

      if (isEndWriteListItem(instruction)) {
        await this.invokeInstruction(() => builder.endWriteListItem(instructionId));
      }

      if (isWriteParagraphBreak(instruction)) {
        await this.invokeInstruction(() => builder.writeParagraphBreak(instructionId));
      }

      if (isWriteText(instruction)) {
        await this.invokeInstruction(() =>
          builder.writeText(instructionId, instruction.text, instruction.reference, instruction.conditions));
      }
    });

    instructionId++;
    await this.invokeInstruction(() => builder.endWriteDocument(instructionId));

    return builder.result;
  }

  private async invokeInstruction(instructionFunc: (() => Promise<void> | void)): Promise<void> {
    const result = instructionFunc();
    if (result) {
      await result;
    } else {
      await new Promise<void>(resolve => resolve());
    }
  }
}
