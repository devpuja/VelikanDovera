import { Component, OnInit, NgZone, ElementRef } from '@angular/core';
import { MatDialog } from '@angular/material';
import { EmployeeService } from '../shared/services/employee.service';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../shared/services/user.service';
import { UserRegistration } from '../shared/models/user.registration.interface';
import { FormBuilder, FormGroup, Validators} from '@angular/forms';
import { HttpEventType, HttpErrorResponse } from '@angular/common/http';
import { EmployeeExpensesStatus } from '../shared/models/employee.expenses.interface';
import { EmployeeExpensesService } from '../shared/services/employee-expenses.service';
//import { BaseService } from "../shared/services/base.service";

@Component({
  selector: 'app-employee-dashboard',
  templateUrl: './employee-dashboard.component.html',
  styleUrls: ['./employee-dashboard.component.css']
})
export class EmployeeDashboardComponent implements OnInit {
  showSpinner: boolean = false;
  FullName: string;
  Email: string;
  Pancard: string;
  DOJ: string;
  DOB: string;
  Address: string;
  Mobile1: string;
  Mobile2: string;
  Department: string;
  Desigination: string;
  Grade: string;
  HQ: string;
  RegionName: string[];
  UserName: string;
  UserRole: string;
  UserClaims: string[];
  PictureUrl: string;

  //Allowance
  Daily: string;
  Bike : string;
  Mobile: string;
  Fare: string;
  Stationery: string;
  Cyber: string;

  empExpensesStatus: EmployeeExpensesStatus[]

  empData: UserRegistration;
  photoFormGroup: FormGroup;
  passwordFormGroup: FormGroup;

  //Photos Upload
  filePhoto: ElementRef; //use if seperate upload button is needed
  selectedFile: File = null;
  uploadProgress: number = 0;
  uploadComplete: boolean = false;
  uploadingProgressing: boolean = false;
  serverResponse: any;
  MAX_BYTES = 2 * 1024 * 1024; //2 MB (2097152)
  ACCEPTED_FILE_TYPES = [ "jpg", "jpeg", "png"];

  constructor(public dialog: MatDialog, private _formBuilder: FormBuilder, private empService: EmployeeService, private empExpService: EmployeeExpensesService,
    private userService: UserService, private toastr: ToastrService, private zone: NgZone) {
    //super();
  }

  ngOnInit() {
    var userID = this.userService.getUserID();
    var userName = this.userService.getUserName();
    
    if (userID != null) {
      this.fetchUserDetails(userID);
    }

    this.employeeExpenseNotify(userName);

    this.photoFormGroup = this._formBuilder.group({});

    this.passwordFormGroup = this._formBuilder.group({
      "PasswordCtrl": ['', Validators.required]
    });
  }

  get PasswordCtrl() { return this.passwordFormGroup.get('PasswordCtrl'); }

  fetchUserDetails(userID: string) {
    this.showSpinner = true;
    this.empService.getEmployeeByID(userID)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            console.log(result);
            //this.empData = result.Item1;
            this.empData = result.Item1;
            this.Daily = result.Item2;
            this.Bike = result.Item3;
            this.Mobile = result.Item4;
            this.Fare = result.Item5;
            this.Stationery = result.Item6;
            this.Cyber = result.Item7;

            this.BindValuesToControls(this.empData);
          }
        },
        error => this.toastr.error(error, "Error"));
  }

  BindValuesToControls(usrReg: UserRegistration) {
    this.FullName = usrReg.FirstName + " " + usrReg.MiddleName + " " + usrReg.LastName;
    this.Email = usrReg.Email;
    this.DOB = usrReg.DOB;
    this.DOJ = usrReg.DOJ;
    this.Pancard = usrReg.Pancard;
    this.Address = usrReg.Contact.Address + ", " + usrReg.Contact.City + ": " + usrReg.Contact.PinCode + ", " + usrReg.Contact.State;
    this.Mobile1 = usrReg.Contact.MobileNumber;
    this.Mobile2 = usrReg.Contact.ResidenceNumber;
    this.Department = usrReg.DepartmentName;
    this.Desigination = usrReg.DesiginationName;
    this.Grade = usrReg.GradeName;
    this.HQ = usrReg.HQName;
    this.RegionName = usrReg.RegionName;
    this.UserName = usrReg.CustomUserName;
    this.UserRole = usrReg.Roles.RoleName;
    this.UserClaims = usrReg.Roles.UserClaimsOnRoles;
    this.PictureUrl = (usrReg.PictureUrl == null || usrReg.PictureUrl == "") ? "https://via.placeholder.com/100x100" : usrReg.PictureUrl;
  }

  employeeExpenseNotify(userName: string) {
    this.showSpinner = true;
    this.empExpService.getNotification(userName)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            this.bindNotification(result);
            this.toastr.success("Record Loaded", "Success");
          }
        },
        error => this.toastr.error(error, "Error"));
  }

  bindNotification(data) {
    this.empExpensesStatus = data;
  }

  submitPasswordForm(): void {
    var newPassword = this.PasswordCtrl.value;
    this.empData.PasswordRaw = newPassword;

    this.showSpinner = true;
    this.empService.updatePassword(this.empData)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            this.PasswordCtrl.setValue("");
            this.toastr.success("Password Updated Successfully", "Success");
          }
        },
        error => this.toastr.error(error, "Error"));
  }

  //PhotoUpload
  handleProgress(event) {
    if (event.type === HttpEventType.DownloadProgress) {
      this.uploadingProgressing = true
      this.uploadProgress = Math.round(100 * event.loaded / event.total)
    }

    if (event.type === HttpEventType.UploadProgress) {
      this.uploadingProgressing = true
      this.uploadProgress = Math.round(100 * event.loaded / event.total)
    }

    if (event.type === HttpEventType.Response) {
      // console.log(event.body);
      this.uploadComplete = true
      this.serverResponse = event.body;
      //this.photoFormGroup.reset();
      this.toastr.success("Profile picture updated successfully.", "Success");
    }
  }

  onFileChanged(event) {
    this.selectedFile = <File>event.target.files[0];
    var extn = this.selectedFile.name.split(".").pop().toLowerCase();
    var isValid = this.ACCEPTED_FILE_TYPES.some(x => x === extn);

    if (isValid) {
      if (this.selectedFile.size <= this.MAX_BYTES) {
        this.empService.uploadPhoto(this.empData.ID, this.selectedFile)
          .subscribe(
          event => {
            this.handleProgress(event);
            this.toastr.success("Profile picture updated successfully.", "Success");
          },          
          errors => {
            this.toastr.error(errors.error.message, "Error");
          }
        );
      }
      else {
        this.toastr.error("Max file size is 2 MB", "Error");
      }
    }
    else {
      this.toastr.error("Allowed file type *.jpg,*.jpeg & *.png", "Error");      
    }

    this.reloadPage();
  }

  reloadPage() { // click handler or similar
    this.zone.runOutsideAngular(() => {
      location.reload();
    });
  }

}
