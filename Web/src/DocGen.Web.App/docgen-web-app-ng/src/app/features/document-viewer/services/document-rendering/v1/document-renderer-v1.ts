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

    await builder.beginWriteDocument();

    document.instructions.forEach(async i => {

      if (isBeginWritePage(i)) {
        await builder.beginWritePage();
      }

      if (isBeginWriteList(i)) {
        await builder.beginWriteList();
      }

      if (isBeginWriteListItem(i)) {
        await builder.beginWriteListItem();
      }

      if (isEndWritePage(i)) {
        await builder.endWritePage();
      }

      if (isEndWriteList(i)) {
        await builder.endWriteList();
      }

      if (isEndWriteListItem(i)) {
        await builder.endWriteListItem();
      }

      if (isWriteParagraphBreak(i)) {
        await builder.writeParagraphBreak();
      }

      if (isWriteText(i)) {
        await builder.writeText(i.text, i.reference, i.conditions);
      }
    });

    await builder.endWriteDocument();

    return builder.result;
  }
}
