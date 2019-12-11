import { TestBed } from '@angular/core/testing';

import { StockistService } from './stockist.service';

describe('StockistService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: StockistService = TestBed.get(StockistService);
    expect(service).toBeTruthy();
  });
});
