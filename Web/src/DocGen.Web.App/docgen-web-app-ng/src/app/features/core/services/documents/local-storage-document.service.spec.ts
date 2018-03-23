import { TestBed, inject } from '@angular/core/testing';

import { LocalStorageDocumentService } from './local-storage-document.service';

describe('LocalStorageDocumentService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LocalStorageDocumentService]
    });
  });

  it('should be created', inject([LocalStorageDocumentService], (service: LocalStorageDocumentService) => {
    expect(service).toBeTruthy();
  }));
});
