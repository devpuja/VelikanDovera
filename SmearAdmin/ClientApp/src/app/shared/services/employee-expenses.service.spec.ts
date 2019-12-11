import { TestBed } from '@angular/core/testing';

import { EmployeeExpensesService } from './employee-expenses.service';

describe('EmployeeExpensesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EmployeeExpensesService = TestBed.get(EmployeeExpensesService);
    expect(service).toBeTruthy();
  });
});
