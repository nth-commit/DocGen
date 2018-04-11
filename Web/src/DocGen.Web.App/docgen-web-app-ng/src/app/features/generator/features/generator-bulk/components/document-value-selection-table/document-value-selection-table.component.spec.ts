import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentValueSelectionTableComponent } from './document-value-selection-table.component';

describe('DocumentValueSelectionTableComponent', () => {
  let component: DocumentValueSelectionTableComponent;
  let fixture: ComponentFixture<DocumentValueSelectionTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentValueSelectionTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentValueSelectionTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
