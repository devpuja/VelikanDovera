import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormBuilder, FormGroup, FormsModule, FormControl, ReactiveFormsModule, Validators, AbstractControl } from '@angular/forms';
import { Masters, MastersGroup } from '../../shared/models/masters.interface';
import { MastersService } from '../../shared/services/masters.service';
import { ToastrService } from 'ngx-toastr';
import { debug } from 'util';

@Component({
  selector: 'app-masters-doctor-employee-dialog',
  templateUrl: './masters-doctor-employee-dialog.component.html',
  styleUrls: ['./masters-doctor-employee-dialog.component.css']
})
export class MastersDoctorEmployeeDialogComponent implements OnInit {
  showSpinner: boolean = false;
  masterFormGroup: FormGroup;
  masters: Masters = { Id: 0, Type: '', Value: '' };

  constructor(public dialogRef: MatDialogRef<MastersDoctorEmployeeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dataMst: Masters,
    fb: FormBuilder,
    private mstService: MastersService,
    private toastr: ToastrService) {
  }

  ngOnInit() {
    this.masterFormGroup = new FormGroup({
      "selectMaster": new FormControl('', [Validators.required]),
      "enterValue": new FormControl('', [Validators.required])
    });

    if (this.dataMst != null) {
      this.masters.Id = this.dataMst.Id;
      this.masters.Type = this.dataMst.Type;
      this.masters.Value = this.dataMst.Value;
    }
  }

  get selectMaster() { return this.masterFormGroup.get('selectMaster'); }
  get enterValue() { return this.masterFormGroup.get('enterValue'); }

  //mastersControl = new FormControl();
  mastersGroups: MastersGroup[] = [
    {
      name: 'Doctor',
      mastersFor: [
        { value: 'Brand', viewValue: 'Brand' },
        { value: 'Class', viewValue: 'Class' },
        { value: 'Community', viewValue: 'Community' },
        { value: 'Qualification', viewValue: 'Qualification' },
        { value: 'Speciality', viewValue: 'Speciality' },
        { value: 'BestDayToMeet', viewValue: 'BestDayToMeet' },
        { value: 'BestTimeToMeet', viewValue: 'BestTimeToMeet' },
        { value: 'VisitFrequency', viewValue:'VisitFrequency' },
      ]
    },
    {
      name: 'Employee',
      //disabled: true,
      mastersFor: [
        { value: 'Desigination', viewValue: 'Desigination' },
        { value: 'Department', viewValue: 'Department' },
        { value: 'Grade', viewValue: 'Grade' },
        { value: 'HQ', viewValue: 'HQ' },
        { value: 'Region', viewValue: 'Region' },        
      ]
    },
    {
      name: 'Allowance',
      mastersFor: [
        { value: 'Bike', viewValue: 'Bike' },
        { value: 'Cyber', viewValue: 'Cyber' },
        { value: 'Daily', viewValue: 'Daily' },
        { value: 'Fare', viewValue: 'Fare' },
        { value: 'Mobile', viewValue: 'Mobile' },
        { value: 'Stationery', viewValue: 'Stationery' },
        { value: 'Present Type', viewValue: 'Present Type' },
      ]
    },
    {
      name: 'Miscellaneous',
      mastersFor: [
        { value: 'Gender', viewValue: 'Gender' },
      ]
    }
  ];

  onNoClick(): void {
    this.dialogRef.close();
  }

  onClick(): void {
    this.showSpinner = true;
    if (this.masters.Id <= 0) {
      this.mstService.create(this.masters)
        .finally(() => this.showSpinner = false)
        .subscribe(
          result => {
            if (result) {
              this.toastr.success("Record Saved", "Success");
              this.dialogRef.close();
            }
          },
          error => this.toastr.error(error, "Error"));
    }
    else {
      this.mstService.editMasters(this.masters)
        .finally(() => this.showSpinner = false)
        .subscribe(
          result => {
            if (result) {
              this.toastr.success("Record Updated", "Success");
              this.dialogRef.close();
            }
          },
          error => this.toastr.error(error, "Error"));
    }
  }
}
