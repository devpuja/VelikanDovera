import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MastersDoctorEmployeeDialogComponent } from './masters-doctor-employee-dialog.component';

describe('MastersDoctorEmployeeDialogComponent', () => {
  let component: MastersDoctorEmployeeDialogComponent;
  let fixture: ComponentFixture<MastersDoctorEmployeeDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MastersDoctorEmployeeDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MastersDoctorEmployeeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
