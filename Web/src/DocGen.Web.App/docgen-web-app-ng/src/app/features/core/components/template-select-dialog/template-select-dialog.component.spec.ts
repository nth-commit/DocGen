import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateSelectDialogComponent } from './template-select-dialog.component';

describe('TemplateSelectDialogComponent', () => {
  let component: TemplateSelectDialogComponent;
  let fixture: ComponentFixture<TemplateSelectDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TemplateSelectDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TemplateSelectDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
