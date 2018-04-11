import { TestBed, inject } from '@angular/core/testing';

import { DocumentValueSelectorService } from './document-value-selector.service';

describe('DocumentValueSelectorService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DocumentValueSelectorService]
    });
  });

  it('should be created', inject([DocumentValueSelectorService], (service: DocumentValueSelectorService) => {
    expect(service).toBeTruthy();
  }));
});
