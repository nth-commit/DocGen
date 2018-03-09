import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentViewerPageComponent } from './document-viewer-page.component';

describe('DocumentViewerComponent', () => {
  let component: DocumentViewerPageComponent;
  let fixture: ComponentFixture<DocumentViewerPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentViewerPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentViewerPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
