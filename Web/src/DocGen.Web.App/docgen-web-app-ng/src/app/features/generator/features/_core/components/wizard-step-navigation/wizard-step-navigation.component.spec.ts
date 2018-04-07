import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateStepNavigationComponent } from './template-step-navigation.component';

describe('TemplateStepNavigationComponent', () => {
  let component: TemplateStepNavigationComponent;
  let fixture: ComponentFixture<TemplateStepNavigationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TemplateStepNavigationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TemplateStepNavigationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
