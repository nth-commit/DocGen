import * as PDFDocument from '../../../../../../../../thirdparty/pdfkit';
import * as blobStream from '../../../../../../../../thirdparty/blob-stream';
import * as PDFKit from 'pdfkit';

import { SerializableDocument } from '../../../../../../core';

import { IDocumentBuilderV1 } from '../../document-builder-v1.interface';

const IDENTITY_PROMISE = new Promise<void>(resolve => resolve());

const RGB_DEFAULT: [number, number, number] = [0, 0, 0];
const RGB_CONDITIONAL: [number, number, number] = [201, 44, 44];
const RGB_REFERENCE: [number, number, number] = [201, 191, 46];

export class PdfDocumentBuilderV1 implements IDocumentBuilderV1 {

  private _result = null;
  private _pdfDocument: PDFKit.PDFDocument;
  private _stream: any;
  private _fillColor: [number, number, number];

  get result(): string {
    if (!this._pdfDocument) {
      throw new Error('Invalid operation; document building not started');
    }

    if (!this._result) {
      throw new Error('Invalid operation; document building not finished');
    }

    return this._result;
  }

  beginWriteDocument(): Promise<void> {
    this._pdfDocument = new PDFDocument();
    this._stream = this._pdfDocument.pipe(blobStream());
    this.setFillColor(RGB_DEFAULT);
    return IDENTITY_PROMISE;
  }

  endWriteDocument(): Promise<void> {
    this._pdfDocument.end();
    return new Promise(resolve => {
      this._stream.on('finish', () => {
        this._result = this._stream.toBlobURL('application/pdf');
        resolve();
      });
    });
  }

  beginPageDocument(): Promise<void> { return IDENTITY_PROMISE; }
  endPageDocument(): Promise<void> { return IDENTITY_PROMISE; }

  writeText(text: string, reference: string): Promise<void> {
    const currentFillColor = this._fillColor;
    if (reference) {
      this._pdfDocument.fillColor(RGB_REFERENCE);
    }

    this._pdfDocument.text(text, {
      continued: true
    });

    if (reference) {
      this._pdfDocument.fillColor(currentFillColor);
    }

    return IDENTITY_PROMISE;
  }

  private writeParagraphBreak() {
    this._pdfDocument.moveDown();
  }

  private setFillColor(fillColor: [number, number, number]) {
    this._fillColor = fillColor;
    this._pdfDocument.fillColor(fillColor);
  }
}
