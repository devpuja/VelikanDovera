import { TestBed } from '@angular/core/testing';

import { SwapEmployeeService } from './swap-employee.service';

describe('SwapEmployeeService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SwapEmployeeService = TestBed.get(SwapEmployeeService);
    expect(service).toBeTruthy();
  });
});
