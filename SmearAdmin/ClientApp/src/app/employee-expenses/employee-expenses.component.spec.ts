import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeExpensesComponent } from './employee-expenses.component';

describe('EmployeeExpensesComponent', () => {
  let component: EmployeeExpensesComponent;
  let fixture: ComponentFixture<EmployeeExpensesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeExpensesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeExpensesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
