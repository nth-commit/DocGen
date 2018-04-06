import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractsTableComponent } from './contracts-table.component';

describe('ContractsTableComponent', () => {
  let component: ContractsTableComponent;
  let fixture: ComponentFixture<ContractsTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContractsTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContractsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
