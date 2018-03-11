import { Injectable } from '@angular/core';

import { SerializableDocument } from '../../../core';
import { DocumentType } from '../../models';

import { IDocumentRenderer } from './document-renderer.interface';
import { DocumentRendererV1 } from './v1/document-renderer-v1';

@Injectable()
export class DocumentRenderingService {

  constructor() { }

  render(document: SerializableDocument, documentType: DocumentType): Promise<string> {
    let renderer: IDocumentRenderer = null;

    if (document.markupVersion === 1) {
      renderer = new DocumentRendererV1();
    } else {
      throw new Error('Invalid operation; unsupported markup version');
    }

    return renderer.render(document, documentType);
  }
}
