import { Injectable } from '@angular/core';

import { IDocumentService, DocumentCreate, DocumentResult } from './document.service.interface';

@Injectable()
export class LocalStorageDocumentService implements IDocumentService {

  constructor() { }

  createOrUpdate(correlationId: string, document: DocumentCreate): Promise<DocumentResult> {
    const result: DocumentResult = Object.assign({}, document, { correlationId });
    localStorage.setItem(`documents:${correlationId}`, JSON.stringify(result));
    return new Promise(resolve => resolve(result));
  }

  list(): Promise<DocumentResult[]> {
    const jsonResults: string[] = [];

    for (let i = 0; i < localStorage.length; i++) {
      const key = localStorage.key(i);
      if (key.startsWith('documents:')) {
        jsonResults.push(localStorage.getItem(key));
      }
      const x = localStorage[i];
    }
    
    return new Promise(resolve => resolve(jsonResults.map(j => JSON.parse(j))));
  }
}
