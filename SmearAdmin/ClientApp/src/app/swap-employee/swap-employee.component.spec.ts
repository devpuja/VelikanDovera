import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SwapEmployeeComponent } from './swap-employee.component';

describe('SwapEmployeeComponent', () => {
  let component: SwapEmployeeComponent;
  let fixture: ComponentFixture<SwapEmployeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SwapEmployeeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SwapEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
