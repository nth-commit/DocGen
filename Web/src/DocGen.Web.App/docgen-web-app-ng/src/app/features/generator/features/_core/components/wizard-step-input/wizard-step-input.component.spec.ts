import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateStepInputComponent } from './template-step-input.component';

describe('TemplateStepInputComponent', () => {
  let component: TemplateStepInputComponent;
  let fixture: ComponentFixture<TemplateStepInputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TemplateStepInputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TemplateStepInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
