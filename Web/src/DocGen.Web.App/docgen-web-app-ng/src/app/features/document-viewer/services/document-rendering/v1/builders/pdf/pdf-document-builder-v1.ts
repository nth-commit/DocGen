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

export class PdfDocumentBuilderV1 implements IDocumentBuilderV1 {

  private _result = null;
  private _pdfDocument: PDFKit.PDFDocument;
  private _stream: any;
  private _fillColor: [number, number, number];
  private _highlightDynamic = false;
  private _listNestingCount = 0;

  get result(): string {
    if (!this._pdfDocument) {
      throw new Error('Invalid operation; document building not started');
    }

    if (!this._result) {
      throw new Error('Invalid operation; document building not finished');
    }

    return this._result;
  }

  beginWriteDocument(): Promise<void> | void {
    this._pdfDocument = new PDFDocument();
    this._stream = this._pdfDocument.pipe(blobStream());
    this.setFillColor(RGB_DEFAULT);

    // this.writeParagraphBreak();

    const writeText = text => {
      this.writeText(text, null, []);
    };

    const writeSimpleListItem = (text, indexPath) => {
      this.beginWriteListItem(indexPath);
      writeText(text);
      this.endWriteListItem();
    };

    // this.beginWritePage();

    // writeText('This is a list example');
    // this.writeParagraphBreak();

    // this.beginWriteList();
    // writeSimpleListItem('First list item', [0]);
    // this.writeParagraphBreak();
    // writeSimpleListItem('Second list item', [1]);
    // this.endWriteList();

    // this.endWritePage();
  }

  endWriteDocument(): Promise<void> | void {
    this._pdfDocument.end();
    return new Promise(resolve => {
      this._stream.on('finish', () => {
        this._result = this._stream.toBlobURL('application/pdf');
        resolve();
      });
    });
  }

  beginWritePage(): Promise<void> | void { }

  endWritePage(): Promise<void> | void { }

  beginWriteList(): Promise<void> | void {
    this._listNestingCount++;
    this.updateListIndentation();
  }

  endWriteList(): Promise<void> | void {
    this.updateListIndentation();
    this._listNestingCount--;
    this.writeParagraphBreak();
  }

  beginWriteListItem(indexPath: number[]): Promise<void> | void {
    const prefix = indexPath.reduce((acc, curr, i) => acc + `${curr + 1}.`, '');
    this._pdfDocument.text(prefix + ' ', {
      continued: true
    });
  }

  endWriteListItem(): Promise<void> | void {
    this._pdfDocument.text(' '); // Terminate continuation
  }

  writeParagraphBreak(): Promise<void> | void {
    this._pdfDocument.text(' '); // Terminates continuation
    this._pdfDocument.moveDown();
    this._pdfDocument.text('');
  }

  writeBreak(): Promise<void> | void {
    this._pdfDocument.text('');
    this._pdfDocument.moveDown();
    this._pdfDocument.text('');
  }

  writeText(text: string, reference: string, conditions: string[]): Promise<void> | void {
    const currentFillColor = this._fillColor;
    const hasCondition = conditions.length > 0;

    if (this._highlightDynamic) {
      if (reference) {
        this._pdfDocument.fillColor(RGB_REFERENCE);
      } else if (hasCondition) {
        this._pdfDocument.fillColor(RGB_CONDITIONAL);
      }
    }

    this._pdfDocument.text(text, {
      continued: true
    });

    if (this._highlightDynamic) {
      if (reference || hasCondition) {
        this._pdfDocument.fillColor(currentFillColor);
      }
    }
  }

  private setFillColor(fillColor: [number, number, number]) {
    this._fillColor = fillColor;
    this._pdfDocument.fillColor(fillColor);
  }

  private updateListIndentation() {
    this._pdfDocument.text(
      '',
      this._pdfDocument.page.margins.left + (this._listNestingCount - 1) * LIST_INDENT_SIZE,
      this._pdfDocument.y,
      {
        continued: true
      });
  }
}
