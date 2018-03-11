import { TestBed, inject } from '@angular/core/testing';

import { DocumentRenderingService } from './document-rendering.service';

describe('DocumentRenderingService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DocumentRenderingService]
    });
  });

  it('should be created', inject([DocumentRenderingService], (service: DocumentRenderingService) => {
    expect(service).toBeTruthy();
  }));
});
