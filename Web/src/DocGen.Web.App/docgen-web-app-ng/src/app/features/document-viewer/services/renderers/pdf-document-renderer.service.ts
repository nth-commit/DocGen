import { Injectable } from '@angular/core';
import * as jsPDF from 'jspdf';

import { SerializableDocument } from '../../../core';

import { IDocumentRenderer } from './document-renderer.service';

@Injectable()
export class PdfDocumentRendererService {

  constructor() { }

  render(document: SerializableDocument): string {
    const pdfDocument: jsPDF = new jsPDF();

    pdfDocument.setFontSize(12);

    pdfDocument.text('Hello world!', 0, 4);

    return pdfDocument.output('bloburi');
  }
}

declare interface jsPDF {
  setFontSize(size: number): jsPDF;
  text(text: string | string[], x?: number, y?: number): jsPDF;

  output(type: 'bloburi'): string;
  output<T>(type: string): T;
}
