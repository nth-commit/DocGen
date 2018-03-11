import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PdfDocumentComponent } from './pdf-document.component';

describe('PdfDocumentComponent', () => {
  let component: PdfDocumentComponent;
  let fixture: ComponentFixture<PdfDocumentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PdfDocumentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PdfDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
