import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentValuesTableComponent } from './document-values-table.component';

describe('DocumentValuesTableComponent', () => {
  let component: DocumentValuesTableComponent;
  let fixture: ComponentFixture<DocumentValuesTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentValuesTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentValuesTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
