import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MastersDoctorEmployeeComponent } from './masters-doctor-employee.component';

describe('MastersDoctorEmployeeComponent', () => {
  let component: MastersDoctorEmployeeComponent;
  let fixture: ComponentFixture<MastersDoctorEmployeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MastersDoctorEmployeeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MastersDoctorEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
