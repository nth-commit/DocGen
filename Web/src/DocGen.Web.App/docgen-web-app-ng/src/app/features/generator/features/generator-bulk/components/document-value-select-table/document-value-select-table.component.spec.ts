import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentValueSelectTableComponent } from './document-value-select-table.component';

describe('DocumentValueSelectTableComponent', () => {
  let component: DocumentValueSelectTableComponent;
  let fixture: ComponentFixture<DocumentValueSelectTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentValueSelectTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentValueSelectTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
