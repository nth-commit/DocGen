import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateStepComponent } from './template-step.component';

describe('TemplateStepComponent', () => {
  let component: TemplateStepComponent;
  let fixture: ComponentFixture<TemplateStepComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TemplateStepComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TemplateStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
