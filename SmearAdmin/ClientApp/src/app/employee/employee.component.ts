import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, FormControl, ReactiveFormsModule, Validators, AbstractControl, FormArray } from '@angular/forms';
import { MastersFor, Masters, MastersGroup } from '../shared/models/masters.interface';
import { ToastrService } from 'ngx-toastr';
import { UserRegistration } from '../shared/models/user.registration.interface';
import { Roles, Permission, ReferenceTableNames } from '../shared/models/permission.model';
import { EmployeeService } from '../shared/services/employee.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})

export class EmployeeComponent implements OnInit {
  showSpinner: boolean = false;
  isLinear = true;

  firstFormGroup: FormGroup;

  RolesFor: MastersFor[];
  PermissionsFor: any;
  Dep: Masters[];
  Des: Masters[];
  Grade: Masters[];
  HQ: Masters[];
  Region: Masters[];

  minDateDOB: any;
  maxDateDOB: any;
  minDateDOJ: any;
  maxDateDOJ: any;

  //Model For Forms
  empData: UserRegistration = {
    ID: '',
    Email: '',
    FirstName: '',
    LastName: '',
    MiddleName: '',
    PasswordRaw: '',
    PictureUrl: '',
    CustomUserName: '',
    Department: 0,
    DepartmentName: '',
    Grade: 0,
    GradeName: '',
    HQ: 0,
    HQName:'',
    Region: [],
    RegionName: [],
    Pancard: '',
    DOJ: '',
    DOB: '',
    Desigination: 0,
    DesiginationName: '',
    Contact: {
      ID: 0,
      RefTableID: '',
      RefTableName: '',
      EmailId: '',
      Address: '',
      Area: '',
      State: '',
      City: '',
      PinCode: '',
      MobileNumber: '',
      ResidenceNumber: '',
      OfficeNumber: ''
    },
    Roles: {
      RoleName: '',
      RoleClaims: [],
      UserClaimsOnRoles:[]
    }
  };

  constructor(private _formBuilder: FormBuilder, private empService: EmployeeService, private toastr: ToastrService, private router: Router,
    private route: ActivatedRoute) { }

  ngOnInit() {
    //Load Dropdown for  Roles & Permissions
    this.RolesFor = [
      { value: 'administrator', viewValue: 'Administrator' },
      { value: 'user', viewValue: 'User' },
    ];

    this.PermissionsFor = ["Add", "Edit", "View", "Delete"];

    //Form's Data
    this.firstFormGroup = this._formBuilder.group({
      "firstNameCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
      "middleNameCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
      "lastNameCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
      "emailIdCtrl": ['', Validators.compose([Validators.required, Validators.email])],
      "DOBCtrl": ['', Validators.required],
      "DOJCtrl": ['', Validators.required],
      "contact": this._formBuilder.group({
        "AddressCtrl": ['', Validators.required],
        "StateCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
        "CityCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
        "PinCodeCtrl": ['', Validators.compose([Validators.required, Validators.minLength(6), Validators.maxLength(6)])],
        "MobileCtrl": ['', Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(10)])],
        "ResidenceCtrl": ['', Validators.compose([Validators.minLength(10), Validators.maxLength(10)])],
      }),
      "selectDepartmentCtrl": ['', Validators.required],
      "selectDesiginationCtrl": ['', Validators.required],
      "selectGradeCtrl": ['', Validators.required],
      "selectHQCtrl": ['', Validators.required],
      "selectRegionCtrl": ['', Validators.required],
      "PancardCtrl": ['', Validators.required],
      "selectRolesCtrl": ['', Validators.required],
      "PermissionsForCtrl": ['', Validators.required]
    });

    let today = new Date();
    this.minDateDOB = new Date(today.getFullYear() - 70, today.getMonth(), today.getDate());
    this.maxDateDOB = new Date(today.getFullYear() - 18, today.getMonth(), today.getDate());

    this.minDateDOJ = new Date(1980, 0, 1);
    this.maxDateDOJ = new Date(today.getFullYear(), today.getMonth(), today.getDate());

    //Load Drop Downs
    this.loadMastersForEmployee();

    var paramID = this.route.snapshot.queryParams['id'];
    if (paramID != null) {
      this.loadEmployeeToEdit(paramID.toString());
    }
    else {
      //Rest Forms
      this.firstFormGroup.reset();

      this.firstNameCtrl.enable();
      this.middleNameCtrl.enable();
      this.lastNameCtrl.enable();
      this.emailIdCtrl.enable();
    }
  }

  onRoleChange(event) {
    this.firstFormGroup.controls["PermissionsForCtrl"].reset();
  }

  get firstNameCtrl() { return this.firstFormGroup.get('firstNameCtrl'); }
  get middleNameCtrl() { return this.firstFormGroup.get('middleNameCtrl'); }
  get lastNameCtrl() { return this.firstFormGroup.get('lastNameCtrl'); }
  get emailIdCtrl() { return this.firstFormGroup.get('emailIdCtrl'); }
  get DOBCtrl() { return this.firstFormGroup.get('DOBCtrl'); }
  get DOJCtrl() { return this.firstFormGroup.get('DOJCtrl'); }

  get AddressCtrl() { return this.firstFormGroup.get('contact.AddressCtrl'); }
  get StateCtrl() { return this.firstFormGroup.get('contact.StateCtrl'); }
  get CityCtrl() { return this.firstFormGroup.get('contact.CityCtrl'); }
  get PinCodeCtrl() { return this.firstFormGroup.get('contact.PinCodeCtrl'); }
  get MobileCtrl() { return this.firstFormGroup.get('contact.MobileCtrl'); }
  get ResidenceCtrl() { return this.firstFormGroup.get('contact.ResidenceCtrl'); }

  get selectDepartmentCtrl() { return this.firstFormGroup.get('selectDepartmentCtrl'); }
  get selectDesiginationCtrl() { return this.firstFormGroup.get('selectDesiginationCtrl'); }
  get selectGradeCtrl() { return this.firstFormGroup.get('selectGradeCtrl'); }
  get selectHQCtrl() { return this.firstFormGroup.get('selectHQCtrl'); }
  get selectRegionCtrl() { return this.firstFormGroup.get('selectRegionCtrl'); }
  get PancardCtrl() { return this.firstFormGroup.get('PancardCtrl'); }

  get selectRolesCtrl() { return this.firstFormGroup.get('selectRolesCtrl'); }
  get PermissionsForCtrl() { return this.firstFormGroup.get('PermissionsForCtrl'); }

  //get canUserAdd() {
  //  return this.userService.userHasPermission(Permission.UsersAddPermission);
  //}

  //get canUserEdit() {
  //  return this.userService.userHasPermission(Permission.UsersEditPermission);
  //}

  //compareTwoDates() {
  //(blur)="compareTwoDates()"
  //  let datediff: number;
  //  datediff = Date.parse(this.DOBCtrl.value) - Date.parse(this.DOJCtrl.value);
  //  console.log(datediff);
  //}

  //Get All masters For Employee

  loadMastersForEmployee() {
    this.showSpinner = true;
    this.empService.getMastersForEmployee()
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            //console.log(result);
            this.Dep = result.ItemsDep;
            this.Des = result.ItemsDes;
            this.Grade = result.ItemsGrade;
            this.HQ = result.ItemsHQ;
            this.Region = result.ItemsRegion;
          }
        },
        error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
  }

  loadEmployeeToEdit(ID: string) {
    this.showSpinner = true;
    this.empService.getEmployeeByID(ID)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            //console.log(result.Item1);
            this.empData = result.Item1;
            this.BindEmployeeValues(this.empData);
          }
        },
        error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
  }

  BindEmployeeValues(usrReg: UserRegistration) {
    console.log(usrReg);
    this.firstNameCtrl.setValue(usrReg.FirstName);
    this.firstNameCtrl.disable();
    
    this.middleNameCtrl.setValue(usrReg.MiddleName);
    this.middleNameCtrl.disable();

    this.lastNameCtrl.setValue(usrReg.LastName);
    this.lastNameCtrl.disable();

    this.emailIdCtrl.setValue(usrReg.Email);
    this.emailIdCtrl.disable();

    var dateDOB = new Date(usrReg.DOB.toString().substr(0, 10));
    this.DOBCtrl.setValue(dateDOB);

    var dateDOJ = new Date(usrReg.DOJ.toString().substr(0, 10));
    this.DOJCtrl.setValue(dateDOJ);

    this.AddressCtrl.setValue(usrReg.Contact.Address);
    this.StateCtrl.setValue(usrReg.Contact.State);
    this.CityCtrl.setValue(usrReg.Contact.City);
    this.PinCodeCtrl.setValue(usrReg.Contact.PinCode);
    this.MobileCtrl.setValue(usrReg.Contact.MobileNumber);
    this.ResidenceCtrl.setValue(usrReg.Contact.ResidenceNumber);

    this.selectDepartmentCtrl.setValue(usrReg.Department);
    this.selectDesiginationCtrl.setValue(usrReg.Desigination);
    this.selectGradeCtrl.setValue(usrReg.Grade);
    this.selectHQCtrl.setValue(usrReg.HQ);
    this.selectRegionCtrl.setValue(usrReg.Region);
    //this.selectRegionCtrl.setValue(usrReg.RegionName);
    this.PancardCtrl.setValue(usrReg.Pancard);

    this.selectRolesCtrl.setValue(usrReg.Roles.RoleName);
    this.PermissionsForCtrl.setValue(usrReg.Roles.UserClaimsOnRoles);
  }

  submitEmployeeForm() {
    this.empData.FirstName = this.firstNameCtrl.value;
    this.empData.MiddleName = this.middleNameCtrl.value;
    this.empData.LastName = this.lastNameCtrl.value;
    this.empData.Email = this.emailIdCtrl.value;
    this.empData.DOB = this.DOBCtrl.value;
    this.empData.DOJ = this.DOJCtrl.value;
    this.empData.Department = this.selectDepartmentCtrl.value;
    this.empData.Desigination = this.selectDesiginationCtrl.value;
    this.empData.Grade = this.selectGradeCtrl.value;
    this.empData.HQ = this.selectHQCtrl.value;
    this.empData.Region = this.selectRegionCtrl.value;
    this.empData.Pancard = this.PancardCtrl.value;

    this.empData.Contact.RefTableName = ReferenceTableNames.EMPLOYEE;
    this.empData.Contact.Address = this.AddressCtrl.value;
    this.empData.Contact.State = this.StateCtrl.value;
    this.empData.Contact.City = this.CityCtrl.value;
    this.empData.Contact.PinCode = this.PinCodeCtrl.value;
    this.empData.Contact.MobileNumber = this.MobileCtrl.value;
    this.empData.Contact.ResidenceNumber = this.ResidenceCtrl.value;
    
    this.empData.Roles.RoleName = this.selectRolesCtrl.value;
    this.empData.Roles.RoleClaims = this.PermissionsForCtrl.value;
    //console.log(this.empData);

    this.showSpinner = true;
    if (this.empData.ID == "") {
      this.empService.createEmployee(this.empData)
        .finally(() => this.showSpinner = false)
        .subscribe(
          result => {
            if (result) {
              this.toastr.success("Record Saved", "Success");
              this.router.navigate(['./employee-list']);
            }
          },
          error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
    }
    else {
      this.empService.editEmployee(this.empData)
        .finally(() => this.showSpinner = false)
        .subscribe(
          result => {
            if (result) {
              this.toastr.success("Record Saved", "Success");
              this.router.navigate(['./employee-list']);
            }
          },
          error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
    }
  }
}
