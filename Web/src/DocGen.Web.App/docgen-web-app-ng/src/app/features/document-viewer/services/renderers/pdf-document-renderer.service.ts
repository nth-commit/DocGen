import { Injectable } from '@angular/core';

import * as PDFDocument from '../../../../../thirdparty/pdfkit';
import * as blobStream from '../../../../../thirdparty/blob-stream';
import * as PDFKit from 'pdfkit';

import { SerializableDocument } from '../../../core';

import { IDocumentRenderer } from './document-renderer.service';

@Injectable()
export class PdfDocumentRendererService {

  constructor() { }

  render(document: SerializableDocument): Promise<string> {
    const pdfDoc: PDFKit.PDFDocument = new PDFDocument();

    const stream = pdfDoc.pipe(blobStream());
    pdfDoc.text('Hello world!');
    pdfDoc.save();
    pdfDoc.end();

    return new Promise(resolve => {
      stream.on('finish', () => {
        resolve(stream.toBlobURL('application/pdf'));
      });
    });
  }
}
