import { TestBed, inject } from '@angular/core/testing';

import { NgxSnotifyService } from './ngx-snotify.service';

describe('NgxSnotifyService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [NgxSnotifyService]
    });
  });

  it('should be created', inject([NgxSnotifyService], (service: NgxSnotifyService) => {
    expect(service).toBeTruthy();
  }));
});
