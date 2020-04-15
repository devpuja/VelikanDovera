import { Component, OnInit, Inject, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, FormControl, ReactiveFormsModule, Validators, AbstractControl, FormArray, ValidatorFn } from '@angular/forms';
import { MastersFor, Masters, MastersGroup } from '../shared/models/masters.interface';
import { DoctorService } from '../shared/services/doctor.service';
import { ToastrService } from 'ngx-toastr';
import { ReferenceTableNames } from '../shared/models/permission.model';
import { Router, ActivatedRoute } from '@angular/router';
import { Doctor } from '../shared/models/doctor.interface';
import { UserService } from '../shared/services/user.service';
import { debug } from 'util';
import { DOCUMENT } from '@angular/platform-browser';

@Component({
  selector: 'app-doctor',
  templateUrl: './doctor.component.html',
  styleUrls: ['./doctor.component.css']
})
export class DoctorComponent implements OnInit {
  showSpinner: boolean = false;
  isLinear = false;

  docFormGroup: FormGroup;
  QualificationList: Masters[];
  SpecialityList: Masters[];
  BrandList: Masters[];
  ClassList: Masters[];
  BestDayToMeetList: Masters[];
  BestTimeToMeetList: Masters[];
  GenderList: Masters[];
  VisitFrequencyList: Masters[] = [];
  CommunityList: Masters[];
  VisitFreqValue: any;
  //VisitFreq: FormControl[] = [];
  VisitPlanData: string[] = [];

  minDateDOB: any;
  maxDateDOB: any;
  minDateDOA: any;
  maxDateDOA: any;
  maxDateFoundation: any;

  //Model For Forms
  docData: Doctor = {
    ID: 0,
    Name: '',
    Qualification: '',
    RegistrationNo: '',
    Speciality: '',
    Gender: '',
    VisitFrequency: '',
    VisitPlan: '',
    BestDayToMeet: '',
    BestTimeToMeet: '',
    Brand: '',
    BrandName:[],
    Class: '',
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
    Common: {
      ID: 0,
      RefTableID: '',
      RefTableName: '',
      DrugLicenseNo: '',
      FoodLicenseNo: '',
      GSTNo: '',
      BestTimeToMeet: '',
      ContactPersonName: '',
      ContactPersonMobileNumber: '',
      ContactPersonDateOfBirth: '',
      ContactPersonDateOfAnniversary: '',
    },
    AuditableEntity: {
      ID: 0,
      RefTableID: '',
      RefTableName: '',
      FoundationDay: '',
      CommunityID: 0,
      CommunityName:'',
      IsActive: 0,
      CreateDate: '',
      CreatedBy: '',
    }
  };

  constructor(private _formBuilder: FormBuilder, private docService: DoctorService, private userService: UserService, private toastr: ToastrService,
    private router: Router, private route: ActivatedRoute, @Inject(DOCUMENT) document) { }

  ngOnInit() {
    /*SELECT * FROM Doctor : Doctors details
    SELECT * FROM ContactResourse : Contact Details
    SELECT * FROM ChemistStockistResourse : DOB & DOA
    SELECT * FROM AuditableEntity : Foundation, Community and rest all details*/

    //Form's Data
    this.docFormGroup = this._formBuilder.group({
      "NameCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
      "QualificationCtrl": ['', Validators.required],
      "RegistrationNoCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
      "SpecialityCtrl": ['', Validators.required],
      "GenderCtrl": ['', Validators.required],
      "VisitFrequencyCtrl": ['', Validators.required],
      "VisitPlanCtrl": this._formBuilder.array([]),
      "BestDayToMeetCtrl": ['', Validators.required],
      "BestTimeToMeetCtrl": ['', Validators.required],
      "BrandCtrl": ['', Validators.required],
      "ClassCtrl": ['', Validators.required],
      "DOACtrl": ['', Validators.required],
      "DOBCtrl": ['', Validators.required],
      "contact": this._formBuilder.group({
        "AddressCtrl": ['', Validators.required],
        "StateCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
        "CityCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
        "PinCodeCtrl": ['', Validators.compose([Validators.required, Validators.minLength(6), Validators.maxLength(6)])],
        "MobileCtrl": ['', Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(10)])],
        "ResidenceCtrl": ['', Validators.compose([Validators.minLength(10), Validators.maxLength(12)])],
      }),
      "FoundationDayCtrl": ['',],
      "CommunityCtrl": ['', Validators.required],
    });

    let today = new Date();
    this.minDateDOB = new Date(today.getFullYear() - 100, today.getMonth(), today.getDate());
    this.maxDateDOB = new Date(today.getFullYear() - 18, today.getMonth(), today.getDate());

    this.minDateDOA = new Date(today.getFullYear() - 80, today.getMonth(), today.getDate());
    this.maxDateDOA = new Date(today.getFullYear(), today.getMonth(), today.getDate());

    this.maxDateFoundation = new Date(today.getFullYear(), today.getMonth(), today.getDate());

    //Load Drop Downs
    this.loadMastersForDoctor();
    
    var paramID = this.route.snapshot.queryParams['id'];
    if (paramID != null) {
      this.loadDoctorToEdit(paramID.toString());      
    }
    else {
      //Rest Forms
      this.docFormGroup.reset();
    }
  }

 
  get NameCtrl() { return this.docFormGroup.get('NameCtrl'); }
  get QualificationCtrl() { return this.docFormGroup.get('QualificationCtrl'); }
  get RegistrationNoCtrl() { return this.docFormGroup.get('RegistrationNoCtrl'); }
  get SpecialityCtrl() { return this.docFormGroup.get('SpecialityCtrl'); }
  get GenderCtrl() { return this.docFormGroup.get('GenderCtrl'); }
  get VisitFrequencyCtrl() { return this.docFormGroup.get('VisitFrequencyCtrl'); }
  get VisitPlanCtrl() { return this.docFormGroup.get('VisitPlanCtrl'); }  
  get BestDayToMeetCtrl() { return this.docFormGroup.get('BestDayToMeetCtrl'); }
  get BestTimeToMeetCtrl() { return this.docFormGroup.get('BestTimeToMeetCtrl'); }
  get BrandCtrl() { return this.docFormGroup.get('BrandCtrl'); }
  get ClassCtrl() { return this.docFormGroup.get('ClassCtrl'); }
  get DOACtrl() { return this.docFormGroup.get('DOACtrl'); }
  get DOBCtrl() { return this.docFormGroup.get('DOBCtrl'); }
  get FoundationDayCtrl() { return this.docFormGroup.get('FoundationDayCtrl'); }
  get CommunityCtrl() { return this.docFormGroup.get('CommunityCtrl'); }
  get AddressCtrl() { return this.docFormGroup.get('contact.AddressCtrl'); }
  get StateCtrl() { return this.docFormGroup.get('contact.StateCtrl'); }
  get CityCtrl() { return this.docFormGroup.get('contact.CityCtrl'); }
  get PinCodeCtrl() { return this.docFormGroup.get('contact.PinCodeCtrl'); }
  get MobileCtrl() { return this.docFormGroup.get('contact.MobileCtrl'); }
  get ResidenceCtrl() { return this.docFormGroup.get('contact.ResidenceCtrl'); }

  //Add Dynamic control
  get VisitFreqFn(): FormArray {
    return this.docFormGroup.get('VisitPlanCtrl') as FormArray;
  }

  createItem(): FormGroup {
    return this._formBuilder.group({
      VisitPlans: ['', Validators.compose([Validators.required, maxValue(26)])],
    });
  }

  addSellingPoint() {
    this.VisitFreqFn.push(this.createItem());
  }

  //removeSellingPoint(index) {
  //  this.VisitFreqFn.controls.splice(index, 1);
  //}

  onVisitFrequencyChange(event) {
    this.VisitFreqFn.controls.splice(0);

    for (var i = 0; i < event; i++) {
      this.addSellingPoint();
    }
  }

  loadMastersForDoctor() {
    this.showSpinner = true;
    this.docService.getMastersForDoctor()
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            this.QualificationList = result.ItemsQualification;
            this.SpecialityList = result.ItemsSpeciality;
            this.BrandList = result.ItemsBrand;
            this.ClassList = result.ItemsClass;
            this.BestDayToMeetList = result.ItemsBestDayToMeet;
            this.BestTimeToMeetList = result.ItemsBestTimeToMeet;
            this.GenderList = result.ItemsGender;
            this.VisitFreqValue = result.ItemVisitFrequency;
            this.CommunityList = result.ItemsCommunity;
            
            for (var i = 1; i <= this.VisitFreqValue; i++) {
              this.VisitFrequencyList.push({ Id: i, Value: i.toString(), Type: "VisitFrequency" });
            }
          }
        },
        error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error") });
  }

  loadDoctorToEdit(ID: string) {
    this.showSpinner = true;
    this.docService.getDoctorByID(ID)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            this.docData = result;
            this.BindDoctorValues(this.docData);
          }
        },
          errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
  }

  BindDoctorValues(doc: Doctor) {
    //console.log(doc);
    this.NameCtrl.setValue(doc.Name);
    this.NameCtrl.disable();

    this.QualificationCtrl.setValue(doc.Qualification);
    this.RegistrationNoCtrl.setValue(doc.RegistrationNo);
    this.SpecialityCtrl.setValue(doc.Speciality);

    var dateDOA = new Date(doc.Common.ContactPersonDateOfAnniversary.toString().substr(0, 10));
    this.DOACtrl.setValue(dateDOA);

    var dateDOB = new Date(doc.Common.ContactPersonDateOfBirth.toString().substr(0, 10));
    this.DOBCtrl.setValue(dateDOB);
    
    if (doc.AuditableEntity.FoundationDay !== null) {
      var dateFD = new Date(doc.AuditableEntity.FoundationDay.toString().substr(0, 10));
      this.FoundationDayCtrl.setValue(dateFD);
    }

    this.CommunityCtrl.setValue(doc.AuditableEntity.CommunityID);
    this.GenderCtrl.setValue(doc.Gender);
    this.BestDayToMeetCtrl.setValue(doc.BestDayToMeet);
    this.BestTimeToMeetCtrl.setValue(doc.BestTimeToMeet);

    this.AddressCtrl.setValue(doc.Contact.Address);
    this.StateCtrl.setValue(doc.Contact.State);
    this.CityCtrl.setValue(doc.Contact.City);
    this.PinCodeCtrl.setValue(doc.Contact.PinCode);
    this.MobileCtrl.setValue(doc.Contact.MobileNumber);
    this.ResidenceCtrl.setValue(doc.Contact.ResidenceNumber);

    //Brand
    let brand: number[] = [];
    let brandArr = doc.Brand.split(',');    
    if (brandArr.length > 0) {
      for (var i = 0; i < brandArr.length; i++) {
        brand.push(+brandArr[i]);
      }
      this.BrandCtrl.setValue(brand);
    }
    this.BrandCtrl.setValue(brand);

    this.ClassCtrl.setValue(doc.Class);
    this.VisitFrequencyCtrl.setValue(doc.VisitFrequency);
 
    let vpArr: Array<VisitFreq> = [];
    let visitPlanArr = doc.VisitPlan.split(',');
    if (visitPlanArr.length > 0) {
      for (var j = 0; j < visitPlanArr.length; j++) {
        vpArr.push({ VisitPlans: +visitPlanArr[j] });
      }
      this.VisitFreqFn.patchValue(vpArr);
    }
  }

  submitDoctorForm() {
    this.docData.Name = this.NameCtrl.value;
    this.docData.Qualification = this.QualificationCtrl.value;
    this.docData.RegistrationNo = this.RegistrationNoCtrl.value;
    this.docData.Speciality = this.SpecialityCtrl.value;
    this.docData.Gender = this.GenderCtrl.value;
    this.docData.VisitFrequency = this.VisitFrequencyCtrl.value;

    for (var i = 0; i < this.VisitPlanCtrl.value.length; i++) {
      this.VisitPlanData.push(this.VisitPlanCtrl.value[i].VisitPlans);
    }
    this.docData.VisitPlan = this.VisitPlanData.join(); 

    this.docData.BestDayToMeet = this.BestDayToMeetCtrl.value;
    this.docData.BestTimeToMeet = this.BestTimeToMeetCtrl.value;
    this.docData.Brand = this.BrandCtrl.value.join();
    this.docData.Class = this.ClassCtrl.value;

    this.docData.Contact.RefTableName = ReferenceTableNames.DOCTOR;
    this.docData.Contact.Address = this.AddressCtrl.value;
    this.docData.Contact.State = this.StateCtrl.value;
    this.docData.Contact.City = this.CityCtrl.value;
    this.docData.Contact.PinCode = this.PinCodeCtrl.value;
    this.docData.Contact.MobileNumber = this.MobileCtrl.value;
    this.docData.Contact.ResidenceNumber = this.ResidenceCtrl.value;

    this.docData.Common.RefTableName = ReferenceTableNames.DOCTOR;
    this.docData.Common.ContactPersonMobileNumber = this.MobileCtrl.value;
    this.docData.Common.ContactPersonDateOfBirth = this.DOBCtrl.value;
    this.docData.Common.ContactPersonDateOfAnniversary = this.DOACtrl.value;

    this.docData.AuditableEntity.RefTableName = ReferenceTableNames.DOCTOR;
    this.docData.AuditableEntity.FoundationDay = this.FoundationDayCtrl.value;
    this.docData.AuditableEntity.CommunityID = this.CommunityCtrl.value;
    this.docData.AuditableEntity.IsActive = 1;
    this.docData.AuditableEntity.CreatedBy = this.userService.getUserName();

    //console.log(this.docData);
    this.showSpinner = true;
    if (this.docData.ID == 0) {
      //console.log(this.docData);
      this.docService.addDoctor(this.docData)
        .finally(() => this.showSpinner = false)
        .subscribe(
          result => {
            if (result) {
              this.toastr.success("Record Saved", "Success");
              this.router.navigate(['./manage-doctor']);
            }
          },
          error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
    }
    else {
      this.docService.editDoctor(this.docData)
        .finally(() => this.showSpinner = false)
        .subscribe(
          result => {
            if (result) {
              this.toastr.success("Record Saved", "Success");
              this.router.navigate(['./manage-doctor']);
            }
          },
          error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
    }

  }
}

export function maxValue(max: Number): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } =>
  {
    const input = control.value,
      isValid = input > max;
    if (isValid)
      return { 'maxValue': { max } }
    else
      return null;
  };
}

export class VisitFreq {
  VisitPlans: number;
}
