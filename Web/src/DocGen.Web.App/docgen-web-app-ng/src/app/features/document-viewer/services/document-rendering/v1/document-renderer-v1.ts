import { SerializableDocument, Instruction, TextInstruction, TextInstructionBody, ElementType, WriteType } from '../../../../core';

import { IDocumentRenderer } from '../document-renderer.interface';
import { IDocumentBuilderV1 } from './document-builder-v1.interface';
import { PdfDocumentBuilderV1 } from './builders/pdf/pdf-document-builder-v1';

function isTextInstruction(instruction: Instruction): instruction is TextInstruction {
  return instruction.elementType === ElementType.Text;
}

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
      if (isTextInstruction(i)) {
        await builder.writeText(i.body.text, i.body.reference, i.conditions.length > 0);
      }
    });

    await builder.endWriteDocument();

    return builder.result;
  }
}
