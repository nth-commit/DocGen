import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HtmlDocumentComponent } from './html-document.component';

describe('HtmlDocumentComponent', () => {
  let component: HtmlDocumentComponent;
  let fixture: ComponentFixture<HtmlDocumentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HtmlDocumentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HtmlDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
