import * as PDFDocument from '../../../../../../../../thirdparty/pdfkit';
import * as blobStream from '../../../../../../../../thirdparty/blob-stream';
import * as PDFKit from 'pdfkit';

import { SerializableDocument } from '../../../../../../core';

import { IDocumentBuilderV1 } from '../../document-builder-v1.interface';

const RGB_DEFAULT: [number, number, number] = [0, 0, 0];
const RGB_CONDITIONAL: [number, number, number] = [201, 44, 44];
const RGB_REFERENCE: [number, number, number] = [201, 191, 46];

const LIST_LABEL_SIZE = 30;
const LIST_INDENT_SIZE = 30;

const longString = 'The Organisation agrees to make the Confidential Information available to the' +
' Contractor in accordance with the terms and conditions set out in this agreement.';

export class PdfDocumentBuilderV1 implements IDocumentBuilderV1 {

  private _result = null;
  private _pdfDocument: PDFKit.PDFDocument;
  private _stream: any;
  private _fillColor: [number, number, number];
  private _highlightDynamic = false;
  private _listNestingCount = 0;
  private _lastTextInstructionId = -1;

  get result(): string {
    if (!this._pdfDocument) {
      throw new Error('Invalid operation; document building not started');
    }

    if (!this._result) {
      throw new Error('Invalid operation; document building not finished');
    }

    return this._result;
  }

  beginWriteDocument(instructionId: number): Promise<void> | void {
    this._pdfDocument = new PDFDocument();
    this._stream = this._pdfDocument.pipe(blobStream());
    this.setFillColor(RGB_DEFAULT);

    const invokeInstruction = (instructionFunc: ((instructionId: number, ...args: any[]) => void), ...args) => {
      instructionId++;
      instructionFunc.call(this, instructionId, ...args);
    };

    const writeText = text => {
      invokeInstruction(this.writeText, text, null, []);
    };

    const writeParagraphBreak = () => invokeInstruction(this.writeParagraphBreak);
    const beginWriteList = () => invokeInstruction(this.beginWriteList);
    const endWriteList = () => invokeInstruction(this.endWriteList);
    const beginWriteListItem = (indexPath) => invokeInstruction(this.beginWriteListItem, indexPath);
    const endWriteListItem = () => invokeInstruction(this.endWriteListItem);

    const writeSimpleListItem = (text, indexPath) => {
      beginWriteListItem(indexPath);
      writeText(text);
      endWriteListItem();
    };

    invokeInstruction(this.beginWritePage);

    writeText(longString);
    writeText(' This breaks the two long strings. ');
    writeText(longString);
    writeParagraphBreak();

    beginWriteList();
    writeSimpleListItem('First list item', [0]);
    writeParagraphBreak();
    writeSimpleListItem('Second list item', [1]);
    writeParagraphBreak();

    beginWriteListItem([2]);
    writeText('Some text');
    writeParagraphBreak();
    beginWriteList();
    writeSimpleListItem(longString, [2, 0]);
    writeParagraphBreak();
    writeSimpleListItem(longString, [2, 1]);
    endWriteList();


    endWriteList();

    invokeInstruction(this.endWritePage);
  }

  endWriteDocument(instructionId: number): Promise<void> | void {
    this._pdfDocument.end();
    return new Promise(resolve => {
      this._stream.on('finish', () => {
        this._result = this._stream.toBlobURL('application/pdf');
        resolve();
      });
    });
  }

  beginWritePage(instructionId: number): Promise<void> | void { }

  endWritePage(instructionId: number): Promise<void> | void { }

  beginWriteList(instructionId: number): Promise<void> | void {
    this._listNestingCount++;
  }

  endWriteList(instructionId: number): Promise<void> | void {
    this._listNestingCount--;
    this.writeParagraphBreak(instructionId);
  }

  beginWriteListItem(instructionId: number, indexPath: number[]): Promise<void> | void {
    const prefix = indexPath.reduce((acc, curr, i) => acc + `${curr + 1}.`, '');
    // this.writeIndentedText(prefix + ' ', false, false);
    this.writeListItemLabel(prefix);
  }

  endWriteListItem(instructionId: number): Promise<void> | void {
    this._pdfDocument.text(' '); // Terminate continuation
  }

  writeParagraphBreak(instructionId: number): Promise<void> | void {
    this._pdfDocument.text(' '); // Terminates continuation
    this._pdfDocument.moveDown();
    this._pdfDocument.text('');
  }

  writeBreak(instructionId: number): Promise<void> | void {
    this._pdfDocument.text('');
    this._pdfDocument.moveDown();
    this._pdfDocument.text('');
  }

  writeText(instructionId: number, text: string, reference: string, conditions: string[]): Promise<void> | void {
    const currentFillColor = this._fillColor;
    const hasCondition = conditions.length > 0;

    if (this._highlightDynamic) {
      if (reference) {
        this._pdfDocument.fillColor(RGB_REFERENCE);
      } else if (hasCondition) {
        this._pdfDocument.fillColor(RGB_CONDITIONAL);
      }
    }

    // TODO: Use instruction ID to decide whether to add width to text or not (continue should work)
    if (this._lastTextInstructionId === instructionId - 1) {
      this._pdfDocument.text(text, { continued: true });
    } else {
      // this.writeIndentedText(text, true, true);
      this.writeIndentedParagraph(text);
    }

    if (this._highlightDynamic) {
      if (reference || hasCondition) {
        this._pdfDocument.fillColor(currentFillColor);
      }
    }

    this._lastTextInstructionId = instructionId;
  }

  private setFillColor(fillColor: [number, number, number]) {
    this._fillColor = fillColor;
    this._pdfDocument.fillColor(fillColor);
  }

  private writeIndentedParagraph(text: string) {
    const page = this._pdfDocument.page;

    const margin = page.margins.left;
    const listIndentCount = Math.max(this._listNestingCount - 1, 0); // Don't indent the first list
    const listIndentPadding = listIndentCount * LIST_INDENT_SIZE;
    const listLabelPadding = this._listNestingCount === 0 ? 0 : (this._listNestingCount + 1) * LIST_LABEL_SIZE;

    const padding = margin + listIndentPadding + listLabelPadding;
    const width = page.width - page.margins.right - padding;

    this._pdfDocument.text(
      text,
      padding,
      this._pdfDocument.y,
      {
        continued: true,
        width
      });
  }

  private writeListItemLabel(text: string) {
    const page = this._pdfDocument.page;

    const margin = page.margins.left;
    const listIndentCount = Math.max(this._listNestingCount - 1, 0); // Don't indent the first list
    const listIndentPadding = listIndentCount * LIST_INDENT_SIZE;
    const listLabelPadding = this._listNestingCount === 0 ? 0 : this._listNestingCount * LIST_LABEL_SIZE;

    const padding = margin + listIndentPadding + listLabelPadding;
    const width = page.width - page.margins.right - padding;

    this._pdfDocument.text(
      text,
      padding,
      this._pdfDocument.y,
      {
        width
      });
  }

  private writeIndentedText(text: string, includeListItemLabel: boolean, continued: boolean) {
    const page = this._pdfDocument.page;

    const margin = page.margins.left;
    const listIndentCount = Math.max(this._listNestingCount - 1, 0); // Don't indent the first list
    const listIndentPadding = listIndentCount * LIST_INDENT_SIZE;
    const listLabelPadding = this._listNestingCount === 0 ? 0 : this._listNestingCount + (includeListItemLabel ? 1 : 0) * LIST_LABEL_SIZE;

    const padding = margin + listIndentPadding + listLabelPadding;
    const width = page.width - page.margins.right - padding;

    this._pdfDocument.text(
      text,
      padding,
      this._pdfDocument.y - this._pdfDocument.currentLineHeight(true),
      {
        continued,
        width
      });
  }

  private getListIndentation(includeListItemLabel: boolean) {
    const margin = this._pdfDocument.page.margins.left;
    const listIndentPadding = (this._listNestingCount - 1) * LIST_INDENT_SIZE;
    const listLabelPadding = (this._listNestingCount - 1 + (includeListItemLabel ? 1 : 0)) * LIST_LABEL_SIZE;

    return margin + listIndentPadding + listLabelPadding;
  }
}
