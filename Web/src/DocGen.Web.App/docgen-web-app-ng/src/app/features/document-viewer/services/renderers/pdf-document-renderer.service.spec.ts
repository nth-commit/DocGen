import { TestBed, inject } from '@angular/core/testing';

import { PdfDocumentRendererService } from './pdf-document-renderer.service';

describe('PdfDocumentRendererService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PdfDocumentRendererService]
    });
  });

  it('should be created', inject([PdfDocumentRendererService], (service: PdfDocumentRendererService) => {
    expect(service).toBeTruthy();
  }));
});
