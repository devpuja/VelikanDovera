import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeExpensesListComponent } from './employee-expenses-list.component';

describe('EmployeeExpensesListComponent', () => {
  let component: EmployeeExpensesListComponent;
  let fixture: ComponentFixture<EmployeeExpensesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeExpensesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeExpensesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
