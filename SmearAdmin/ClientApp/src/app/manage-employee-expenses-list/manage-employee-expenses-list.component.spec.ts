import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageEmployeeExpensesListComponent } from './manage-employee-expenses-list.component';

describe('ManageEmployeeExpensesListComponent', () => {
  let component: ManageEmployeeExpensesListComponent;
  let fixture: ComponentFixture<ManageEmployeeExpensesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageEmployeeExpensesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageEmployeeExpensesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
