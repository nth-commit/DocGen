import * as PDFDocument from '../../../../../../../../thirdparty/pdfkit';
import * as blobStream from '../../../../../../../../thirdparty/blob-stream';
import * as PDFKit from 'pdfkit';

import { SerializableDocument } from '../../../../../../core';

import { IDocumentBuilderV1 } from '../../document-builder-v1.interface';

const RGB_DEFAULT: [number, number, number] = [0, 0, 0];
const RGB_CONDITIONAL: [number, number, number] = [201, 44, 44];
const RGB_REFERENCE: [number, number, number] = [201, 191, 46];

const LIST_LABEL_SIZE = 30;
const LIST_INDENT_SIZE = 15;

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
  }

  beginWriteListItem(instructionId: number, indexPath: number[]): Promise<void> | void {
    // const prefix = indexPath.reduce((acc, curr, i) => acc + `${curr + 1}.`, '');
    this.writeListItemLabel(this.formatListItemIndexPath(indexPath));
  }

  endWriteListItem(instructionId: number): Promise<void> | void { }

  writeParagraphBreak(instructionId: number): Promise<void> | void {
    this._pdfDocument.text(' '); // Terminates continuation
    this._pdfDocument.moveDown();
  }

  writeBreak(instructionId: number): Promise<void> | void {
    throw new Error('Not implemented');
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

    if (this.wasLastInstructionText(instructionId)) {
      this._pdfDocument.text(text, { continued: true });
    } else {
      if (this.isInsideList()) {
        this.beginWriteNestedParagraph(text);
      } else {
        const page = this._pdfDocument.page;

        this._pdfDocument.text(text, page.margins.left, this._pdfDocument.y, {
          continued: true,
          width: page.width - page.margins.left - page.margins.right
        });
      }
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

  private beginWriteNestedParagraph(text: string) {
    if (!this.isInsideList()) {
      throw new Error('Invalid operation; should not write list label outside of list');
    }

    const page = this._pdfDocument.page;

    const margin = page.margins.left;
    const listIndentCount = this._listNestingCount - 1; // Don't indent the first list
    const listIndentPadding = listIndentCount * LIST_INDENT_SIZE;
    const listLabelPadding = this._listNestingCount * LIST_LABEL_SIZE;

    const positionX = margin + listIndentPadding + listLabelPadding;
    const width = page.width - page.margins.right - positionX;

    let positionY = this._pdfDocument.y;
    if (this._listNestingCount > 0) {
      positionY -= this._pdfDocument.currentLineHeight(true);
    }

    this._pdfDocument.text(
      text,
      positionX,
      positionY,
      {
        continued: true,
        width
      });
  }

  private writeListItemLabel(text: string) {
    if (!this.isInsideList()) {
      throw new Error('Invalid operation; should not write list label outside of list');
    }

    const page = this._pdfDocument.page;

    const margin = page.margins.left;
    const listIndentCount = this._listNestingCount - 1; // Don't indent the first list
    const listIndentPadding = listIndentCount * LIST_INDENT_SIZE;
    const listLabelPadding = listIndentCount === 0 ? 0 : listIndentCount * LIST_LABEL_SIZE;

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

  private wasLastInstructionText(currentInstructionId: number): boolean {
    return this._lastTextInstructionId === currentInstructionId - 1;
  }

  private isInsideList(): boolean {
    return this._listNestingCount > 0;
  }

  private formatListItemIndexPath(indexPath: number[]) {
    switch (indexPath.length) {
      case 1:
        return `${indexPath[0] + 1}.`;
      case 2:
        return `${indexPath[0] + 1}.${indexPath[1] + 1}.`;
      case 3:
        return `${String.fromCharCode(97 + indexPath[2] % 26)}.`;
      case 4:
        return `${romanize(indexPath[3]).toLowerCase()}.`;
      case 5:
        return `${indexPath[4] + 1}.`;
      default:
        throw new Error('Expected a value between 1 and 5');
    }

  }
}

// tslint:disable
function romanize (num): string {
  if (!+num)
      return NaN;
  var digits = String(+num).split(""),
      key = ["","C","CC","CCC","CD","D","DC","DCC","DCCC","CM",
             "","X","XX","XXX","XL","L","LX","LXX","LXXX","XC",
             "","I","II","III","IV","V","VI","VII","VIII","IX"],
      roman = "",
      i = 3;
  while (i--)
      roman = (key[+digits.pop() + (i * 10)] || "") + roman;
  return Array(+digits.join("") + 1).join("M") + roman;
}