import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneratorBulkComponent } from './generator-bulk.component';

describe('GeneratorBulkComponent', () => {
  let component: GeneratorBulkComponent;
  let fixture: ComponentFixture<GeneratorBulkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneratorBulkComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneratorBulkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
