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
} from '../../../../core';

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

    await this.invokeInstruction(() => builder.beginWriteDocument());

    document.instructions.forEach(async i => {

      if (isBeginWritePage(i)) {
        await this.invokeInstruction(() => builder.beginWritePage());
      }

      if (isBeginWriteList(i)) {
        await this.invokeInstruction(() => builder.beginWriteList());
      }

      if (isBeginWriteListItem(i)) {
        await this.invokeInstruction(() => builder.beginWriteListItem(i.indexPath));
      }

      if (isEndWritePage(i)) {
        await this.invokeInstruction(() => builder.endWritePage());
      }

      if (isEndWriteList(i)) {
        await this.invokeInstruction(() => builder.endWriteList());
      }

      if (isEndWriteListItem(i)) {
        await this.invokeInstruction(() => builder.endWriteListItem());
      }

      if (isWriteParagraphBreak(i)) {
        await this.invokeInstruction(() => builder.writeParagraphBreak());
      }

      if (isWriteText(i)) {
        await this.invokeInstruction(() => builder.writeText(i.text, i.reference, i.conditions));
      }
    });

    await this.invokeInstruction(() => builder.endWriteDocument());

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
