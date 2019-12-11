import { Component, OnInit, Inject, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, FormControl, ReactiveFormsModule, Validators, AbstractControl, FormArray, ValidatorFn } from '@angular/forms';
import { MastersFor, Masters, MastersGroup } from '../shared/models/masters.interface';
import { DoctorService } from '../shared/services/doctor.service';
import { ToastrService } from 'ngx-toastr';
import { ReferenceTableNames } from '../shared/models/permission.model';
import { Router, ActivatedRoute } from '@angular/router';

import { Chemist } from '../shared/models/chemist.interface';
import { ChemistService } from '../shared/services/chemist.service';
import { UserService } from '../shared/services/user.service';

@Component({
  selector: 'app-chemist',
  templateUrl: './chemist.component.html',
  styleUrls: ['./chemist.component.css']
})
export class ChemistComponent implements OnInit {
  showSpinner: boolean = false;
  isLinear = false;

  chemFormGroup: FormGroup;
  ClassList: Masters[];
  CommunityList: Masters[];

  maxDateFoundation: any;

  //Model For Forms
  chemData: Chemist = {
    ID: 0,
    MedicalName: '',
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
      CommunityName: '',
      IsActive: 0,
      CreateDate: '',
      CreatedBy: '',
    }
  };

  constructor(private _formBuilder: FormBuilder, private chemService: ChemistService, private userService: UserService, private toastr: ToastrService,
    private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    //Form's Data
    this.chemFormGroup = this._formBuilder.group({
      "MedicalNameCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
      "ClassCtrl": ['', Validators.required],
      "common": this._formBuilder.group({
        "DrugLicenseNoCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
        "FoodLicenseNoCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
        "GSTNoCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
        "BestTimeToMeetCtrl": ['', Validators.required],        
        "AreaCtrl": ['', Validators.required],
        "OwnerNameCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
        "DOACtrl": ['', Validators.required],
        "DOBCtrl": ['', Validators.required],
        "EmailIDCtrl": ['',],
      }),
      "contact": this._formBuilder.group({
        "AddressCtrl": ['', Validators.required],
        "StateCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
        "CityCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
        "PinCodeCtrl": ['', Validators.compose([Validators.required, Validators.minLength(6), Validators.maxLength(6)])],
        "MobileCtrl": ['', Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(10)])],
        "ResidenceCtrl": ['', Validators.compose([Validators.minLength(10), Validators.maxLength(10)])],        
      }),
      "FoundationDayCtrl": ['',],
      "CommunityCtrl": ['', Validators.required],
    });

    let today = new Date();
    this.maxDateFoundation = new Date(today.getFullYear(), today.getMonth(), today.getDate());

    //Load Drop Downs
    this.loadMastersForChemist();
    
    var paramID = this.route.snapshot.queryParams['id'];
    if (paramID != null) {
      this.loadChemistToEdit(paramID.toString());
    }
    else {
      //Rest Forms
      this.chemFormGroup.reset();
    }
  }
  
  get MedicalNameCtrl() { return this.chemFormGroup.get('MedicalNameCtrl'); }
  get ClassCtrl() { return this.chemFormGroup.get('ClassCtrl'); }

  get DrugLicenseNoCtrl() { return this.chemFormGroup.get('common.DrugLicenseNoCtrl'); }
  get FoodLicenseNoCtrl() { return this.chemFormGroup.get('common.FoodLicenseNoCtrl'); }
  get GSTNoCtrl() { return this.chemFormGroup.get('common.GSTNoCtrl'); }
  get BestTimeToMeetCtrl() { return this.chemFormGroup.get('common.BestTimeToMeetCtrl'); }  
  get AreaCtrl() { return this.chemFormGroup.get('common.AreaCtrl'); }
  get OwnerNameCtrl() { return this.chemFormGroup.get('common.OwnerNameCtrl'); }
  get DOACtrl() { return this.chemFormGroup.get('common.DOACtrl'); }
  get DOBCtrl() { return this.chemFormGroup.get('common.DOBCtrl'); }
  get EmailIDCtrl() { return this.chemFormGroup.get('common.EmailIDCtrl'); }

  get AddressCtrl() { return this.chemFormGroup.get('contact.AddressCtrl'); }
  get StateCtrl() { return this.chemFormGroup.get('contact.StateCtrl'); }
  get CityCtrl() { return this.chemFormGroup.get('contact.CityCtrl'); }
  get PinCodeCtrl() { return this.chemFormGroup.get('contact.PinCodeCtrl'); }
  get MobileCtrl() { return this.chemFormGroup.get('contact.MobileCtrl'); }
  get ResidenceCtrl() { return this.chemFormGroup.get('contact.ResidenceCtrl'); }

  get FoundationDayCtrl() { return this.chemFormGroup.get('FoundationDayCtrl'); }
  get CommunityCtrl() { return this.chemFormGroup.get('CommunityCtrl'); }

  loadMastersForChemist() {
    this.showSpinner = true;
    this.chemService.getMastersForChemist()
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            this.ClassList = result.ItemsClass;
            this.CommunityList = result.ItemsCommunity;
          }
          //console.log(this.AreaList);
          //console.log(this.BestTimeToMeetList);          
        },
        error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error") });
  }

  loadChemistToEdit(ID: string) {
    this.showSpinner = true;
    this.chemService.getChemistByID(ID)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            this.chemData = result;
            this.BindChemistValues(this.chemData);
          }
        },
        error => this.toastr.error(error, "Error"));
  }

  BindChemistValues(chem: Chemist) {
    //console.log(chem);
    this.MedicalNameCtrl.setValue(chem.MedicalName);
    this.MedicalNameCtrl.disable();

    this.ClassCtrl.setValue(chem.Class);

    this.DrugLicenseNoCtrl.setValue(chem.Common.DrugLicenseNo);
    this.FoodLicenseNoCtrl.setValue(chem.Common.FoodLicenseNo);
    this.GSTNoCtrl.setValue(chem.Common.GSTNo);
    this.BestTimeToMeetCtrl.setValue(chem.Common.BestTimeToMeet);
    this.OwnerNameCtrl.setValue(chem.Common.ContactPersonName);

    var dateDOA = new Date(chem.Common.ContactPersonDateOfAnniversary.toString().substr(0, 10));
    this.DOACtrl.setValue(dateDOA);

    var dateDOB = new Date(chem.Common.ContactPersonDateOfBirth.toString().substr(0, 10));
    this.DOBCtrl.setValue(dateDOB);

    this.AreaCtrl.setValue(chem.Contact.Area);
    this.EmailIDCtrl.setValue(chem.Contact.EmailId);
    this.AddressCtrl.setValue(chem.Contact.Address);
    this.StateCtrl.setValue(chem.Contact.State);
    this.CityCtrl.setValue(chem.Contact.City);
    this.PinCodeCtrl.setValue(chem.Contact.PinCode);
    this.MobileCtrl.setValue(chem.Contact.MobileNumber);
    this.ResidenceCtrl.setValue(chem.Contact.ResidenceNumber);

    this.CommunityCtrl.setValue(chem.AuditableEntity.CommunityID);

    if (chem.AuditableEntity.FoundationDay !== null) {
      var dateFD = new Date(chem.AuditableEntity.FoundationDay.toString().substr(0, 10));
      this.FoundationDayCtrl.setValue(dateFD);
    }
  }

  submitChemistForm() {
    this.chemData.MedicalName = this.MedicalNameCtrl.value;
    this.chemData.Class = this.ClassCtrl.value;

    this.chemData.Contact.RefTableName = ReferenceTableNames.CHEMIST;
    this.chemData.Contact.Address = this.AddressCtrl.value;
    this.chemData.Contact.State = this.StateCtrl.value;
    this.chemData.Contact.City = this.CityCtrl.value;
    this.chemData.Contact.PinCode = this.PinCodeCtrl.value;
    this.chemData.Contact.MobileNumber = this.MobileCtrl.value;
    this.chemData.Contact.ResidenceNumber = this.ResidenceCtrl.value;
    this.chemData.Contact.EmailId = this.EmailIDCtrl.value;
    this.chemData.Contact.Area = this.AreaCtrl.value;

    this.chemData.Common.RefTableName = ReferenceTableNames.CHEMIST;
    this.chemData.Common.ContactPersonName = this.OwnerNameCtrl.value;
    this.chemData.Common.ContactPersonMobileNumber = this.MobileCtrl.value;
    this.chemData.Common.ContactPersonDateOfBirth = this.DOBCtrl.value;
    this.chemData.Common.ContactPersonDateOfAnniversary = this.DOACtrl.value;
    this.chemData.Common.DrugLicenseNo = this.DrugLicenseNoCtrl.value;
    this.chemData.Common.FoodLicenseNo = this.FoodLicenseNoCtrl.value;
    this.chemData.Common.GSTNo = this.GSTNoCtrl.value;
    this.chemData.Common.BestTimeToMeet = this.BestTimeToMeetCtrl.value;   

    this.chemData.AuditableEntity.RefTableName = ReferenceTableNames.CHEMIST;
    this.chemData.AuditableEntity.FoundationDay = this.FoundationDayCtrl.value;
    this.chemData.AuditableEntity.CommunityID = this.CommunityCtrl.value;
    this.chemData.AuditableEntity.IsActive = 1;
    this.chemData.AuditableEntity.CreatedBy = this.userService.getUserName();

    //console.log(this.chemData);
    this.showSpinner = true;
    if (this.chemData.ID == 0) {
      console.log(this.chemData);
      this.chemService.addChemist(this.chemData)
        .finally(() => this.showSpinner = false)
        .subscribe(
          result => {
            if (result) {
              this.toastr.success("Record Saved", "Success");
              this.router.navigate(['./manage-chemist']);
            }
          },
          error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
    }
    else {
      this.chemService.editChemist(this.chemData)
        .finally(() => this.showSpinner = false)
        .subscribe(
          result => {
            if (result) {
              this.toastr.success("Record Saved", "Success");
              this.router.navigate(['./manage-chemist']);
            }
          },
          error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
    }

  }

}
