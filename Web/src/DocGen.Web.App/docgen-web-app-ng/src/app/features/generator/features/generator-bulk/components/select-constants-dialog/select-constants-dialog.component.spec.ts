import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectConstantsDialogComponent } from './select-constants-dialog.component';

describe('SelectConstantsDialogComponent', () => {
  let component: SelectConstantsDialogComponent;
  let fixture: ComponentFixture<SelectConstantsDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectConstantsDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectConstantsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
