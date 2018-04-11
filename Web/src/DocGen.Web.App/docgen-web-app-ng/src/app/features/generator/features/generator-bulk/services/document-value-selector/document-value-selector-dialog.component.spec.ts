import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentValueSelectorDialogComponent } from './document-value-selector-dialog.component';

describe('DocumentValueSelectorDialogComponent', () => {
  let component: DocumentValueSelectorDialogComponent;
  let fixture: ComponentFixture<DocumentValueSelectorDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentValueSelectorDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentValueSelectorDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
