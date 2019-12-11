import { Component, OnInit, Inject, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, FormControl, ReactiveFormsModule, Validators, AbstractControl, FormArray, ValidatorFn } from '@angular/forms';
import { MastersFor, Masters, MastersGroup } from '../shared/models/masters.interface';
import { DoctorService } from '../shared/services/doctor.service';
import { ToastrService } from 'ngx-toastr';
import { ReferenceTableNames } from '../shared/models/permission.model';
import { Router, ActivatedRoute } from '@angular/router';

import { Stockist } from '../shared/models/stockist.interface';
import { StockistService } from '../shared/services/stockist.service';
import { UserService } from '../shared/services/user.service';

@Component({
  selector: 'app-stockist',
  templateUrl: './stockist.component.html',
  styleUrls: ['./stockist.component.css']
})
export class StockistComponent implements OnInit {
  showSpinner: boolean = false;
  isLinear = false;

  stockFormGroup: FormGroup;
  AreaList: Masters[];
  BestTimeToMeetList: Masters[];
  CommunityList: Masters[];

  maxDateFoundation: any;

  //Model For Forms
  stockData: Stockist = {
    ID: 0,
    StockistName: '',
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

  constructor(private _formBuilder: FormBuilder, private stockService: StockistService, private userService: UserService, private toastr: ToastrService,
    private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    //Form's Data
    this.stockFormGroup = this._formBuilder.group({
      "StockistNameCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
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
    this.loadMastersForStockist();

    var paramID = this.route.snapshot.queryParams['id'];
    if (paramID != null) {
      this.loadStockistToEdit(paramID.toString());
    }
    else {
      //Rest Forms
      this.stockFormGroup.reset();
    }
  }

  get StockistNameCtrl() { return this.stockFormGroup.get('StockistNameCtrl'); }

  get DrugLicenseNoCtrl() { return this.stockFormGroup.get('common.DrugLicenseNoCtrl'); }
  get FoodLicenseNoCtrl() { return this.stockFormGroup.get('common.FoodLicenseNoCtrl'); }
  get GSTNoCtrl() { return this.stockFormGroup.get('common.GSTNoCtrl'); }
  get BestTimeToMeetCtrl() { return this.stockFormGroup.get('common.BestTimeToMeetCtrl'); }
  get AreaCtrl() { return this.stockFormGroup.get('common.AreaCtrl'); }
  get OwnerNameCtrl() { return this.stockFormGroup.get('common.OwnerNameCtrl'); }
  get DOACtrl() { return this.stockFormGroup.get('common.DOACtrl'); }
  get DOBCtrl() { return this.stockFormGroup.get('common.DOBCtrl'); }
  get EmailIDCtrl() { return this.stockFormGroup.get('common.EmailIDCtrl'); }

  get AddressCtrl() { return this.stockFormGroup.get('contact.AddressCtrl'); }
  get StateCtrl() { return this.stockFormGroup.get('contact.StateCtrl'); }
  get CityCtrl() { return this.stockFormGroup.get('contact.CityCtrl'); }
  get PinCodeCtrl() { return this.stockFormGroup.get('contact.PinCodeCtrl'); }
  get MobileCtrl() { return this.stockFormGroup.get('contact.MobileCtrl'); }
  get ResidenceCtrl() { return this.stockFormGroup.get('contact.ResidenceCtrl'); }

  get FoundationDayCtrl() { return this.stockFormGroup.get('FoundationDayCtrl'); }
  get CommunityCtrl() { return this.stockFormGroup.get('CommunityCtrl'); }

  loadMastersForStockist() {
    this.showSpinner = true;
    this.stockService.getMastersForStockist()
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            this.CommunityList = result.ItemsCommunity;
          }
        },
        error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error") });
  }

  loadStockistToEdit(ID: string) {
    this.showSpinner = true;
    this.stockService.getStockistByID(ID)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            this.stockData = result;
            this.BindStockistValues(this.stockData);
          }
        },
        error => this.toastr.error(error, "Error"));
  }

  BindStockistValues(stock: Stockist) {
    //console.log(stock);
    this.StockistNameCtrl.setValue(stock.StockistName);
    this.StockistNameCtrl.disable();

    this.DrugLicenseNoCtrl.setValue(stock.Common.DrugLicenseNo);
    this.FoodLicenseNoCtrl.setValue(stock.Common.FoodLicenseNo);
    this.GSTNoCtrl.setValue(stock.Common.GSTNo);
    this.BestTimeToMeetCtrl.setValue(stock.Common.BestTimeToMeet);
    this.OwnerNameCtrl.setValue(stock.Common.ContactPersonName);

    var dateDOA = new Date(stock.Common.ContactPersonDateOfAnniversary.toString().substr(0, 10));
    this.DOACtrl.setValue(dateDOA);

    var dateDOB = new Date(stock.Common.ContactPersonDateOfBirth.toString().substr(0, 10));
    this.DOBCtrl.setValue(dateDOB);

    this.AreaCtrl.setValue(stock.Contact.Area);
    this.EmailIDCtrl.setValue(stock.Contact.EmailId);
    this.AddressCtrl.setValue(stock.Contact.Address);
    this.StateCtrl.setValue(stock.Contact.State);
    this.CityCtrl.setValue(stock.Contact.City);
    this.PinCodeCtrl.setValue(stock.Contact.PinCode);
    this.MobileCtrl.setValue(stock.Contact.MobileNumber);
    this.ResidenceCtrl.setValue(stock.Contact.ResidenceNumber);

    this.CommunityCtrl.setValue(stock.AuditableEntity.CommunityID);

    if (stock.AuditableEntity.FoundationDay !== null) {
      var dateFD = new Date(stock.AuditableEntity.FoundationDay.toString().substr(0, 10));
      this.FoundationDayCtrl.setValue(dateFD);
    }
  }

  submitStockistForm() {
    this.stockData.StockistName = this.StockistNameCtrl.value;

    this.stockData.Contact.RefTableName = ReferenceTableNames.STOCKIST;
    this.stockData.Contact.Address = this.AddressCtrl.value;
    this.stockData.Contact.State = this.StateCtrl.value;
    this.stockData.Contact.City = this.CityCtrl.value;
    this.stockData.Contact.PinCode = this.PinCodeCtrl.value;
    this.stockData.Contact.MobileNumber = this.MobileCtrl.value;
    this.stockData.Contact.ResidenceNumber = this.ResidenceCtrl.value;
    this.stockData.Contact.EmailId = this.EmailIDCtrl.value;
    this.stockData.Contact.Area = this.AreaCtrl.value;

    this.stockData.Common.RefTableName = ReferenceTableNames.STOCKIST;
    this.stockData.Common.ContactPersonName = this.OwnerNameCtrl.value;
    this.stockData.Common.ContactPersonMobileNumber = this.MobileCtrl.value;
    this.stockData.Common.ContactPersonDateOfBirth = this.DOBCtrl.value;
    this.stockData.Common.ContactPersonDateOfAnniversary = this.DOACtrl.value;
    this.stockData.Common.DrugLicenseNo = this.DrugLicenseNoCtrl.value;
    this.stockData.Common.FoodLicenseNo = this.FoodLicenseNoCtrl.value;
    this.stockData.Common.GSTNo = this.GSTNoCtrl.value;
    this.stockData.Common.BestTimeToMeet = this.BestTimeToMeetCtrl.value;

    this.stockData.AuditableEntity.RefTableName = ReferenceTableNames.STOCKIST;
    this.stockData.AuditableEntity.FoundationDay = this.FoundationDayCtrl.value;
    this.stockData.AuditableEntity.CommunityID = this.CommunityCtrl.value;
    this.stockData.AuditableEntity.IsActive = 1;
    this.stockData.AuditableEntity.CreatedBy = this.userService.getUserName();

    //console.log(this.stockData);
    this.showSpinner = true;
    if (this.stockData.ID == 0) {
      console.log(this.stockData);
      this.stockService.addStockist(this.stockData)
        .finally(() => this.showSpinner = false)
        .subscribe(
          result => {
            if (result) {
              this.toastr.success("Record Saved", "Success");
              this.router.navigate(['./manage-stockist']);
            }
          },
          error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
    }
    else {
      this.stockService.editStockist(this.stockData)
        .finally(() => this.showSpinner = false)
        .subscribe(
          result => {
            if (result) {
              this.toastr.success("Record Saved", "Success");
              this.router.navigate(['./manage-stockist']);
            }
          },
          error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
    }

  }
}
