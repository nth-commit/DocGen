import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TextDocumentComponent } from './text-document.component';

describe('TextDocumentComponent', () => {
  let component: TextDocumentComponent;
  let fixture: ComponentFixture<TextDocumentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TextDocumentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TextDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
