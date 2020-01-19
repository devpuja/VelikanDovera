import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatDialog, } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { ConfigService } from '../shared/utils/config.service';
import { tap } from 'rxjs/operators';
import { Doctor } from '../shared/models/doctor.interface';
import { Observable } from 'rxjs';
import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { Router } from '@angular/router';
import { SendSmsService } from '../shared/services/send-sms.service';
import { SendSMS, SendSMSList } from '../shared/models/send-sms.interface';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-send-sms',
  templateUrl: './send-sms.component.html',
  styleUrls: ['./send-sms.component.css']
})
export class SendSmsComponent implements OnInit {
  showSpinner: boolean = false;
  baseUrl: string = '';
  IsFilterValue: string = "";
  IsSearchValue: string = "";

  SMSFormGroup: FormGroup;

  // Pagination
  @ViewChild(MatPaginator) paginator: MatPaginator;
  dataSource: DoctorDataSource;
  displayedColumns = ['Name', 'Speciality', 'Brand', 'Class', 'MobileNumber', 'action'];
  totalItemsCount: number;
  pageSizeOptions: number[] = [5, 10, 25];
  sendSmsData: SendSMS[] = [];
  sendSmsDataList: SendSMSList[] = [];

  constructor(private _formBuilder: FormBuilder, public dialog: MatDialog, private smsService: SendSmsService, private toastr: ToastrService, private router: Router, private configService: ConfigService) {
    this.baseUrl = this.configService.getApiURI();
  }

  ngOnInit() {
    //Form's Data
    this.SMSFormGroup = this._formBuilder.group({
      "MessageCtrl": ['', Validators.compose([Validators.required, Validators.maxLength(100)])]
    });

    if (this.IsFilterValue != "") {
      this.getValuesByUserName(this.IsFilterValue);
    } else if (this.IsSearchValue != "") {
      this.getValuesBySearch(this.IsSearchValue);
    }
    else {
      this.getValues();
    }
  }

  get MessageCtrl() { return this.SMSFormGroup.get('MessageCtrl'); }

  ngAfterViewInit() {
    this.paginator.page.pipe(tap(() => this.loadPages())).subscribe();
  }

  loadPages() {
    if (this.IsFilterValue != "") {
      this.loadDataByUserName(this.paginator.pageIndex, this.paginator.pageSize, this.IsFilterValue);
    } else if (this.IsSearchValue != "") {
      this.loadDataBySearch(this.paginator.pageIndex, this.paginator.pageSize, this.IsSearchValue);
    } else {
      this.loadData(this.paginator.pageIndex, this.paginator.pageSize);
    }
  }

  getValues() {
    this.loadData(0, 5);
  }

  loadData(pgIndex: number, pgSize: number) {
    this.showSpinner = true;
    this.smsService.getAllDoctorSendSMS(pgIndex, pgSize)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            //console.log(result.Items);
            this.dataSource = new DoctorDataSource(result.Items);
            this.totalItemsCount = result.TotalCount;
            this.toastr.success("Records Loaded", "Success");
          }
        },
        errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
  }

  //Filter by User
  getValuesByUserName($event: string) {
    this.IsFilterValue = $event;
    if (this.IsFilterValue != "") {
      this.loadDataByUserName(0, 5, this.IsFilterValue);
    }
    else {
      this.loadData(0, 5);
    }
  }

  loadDataByUserName(pgIndex: number, pgSize: number, userName: string) {
    this.showSpinner = true;
    this.smsService.getAllDoctorSendSMSByUserName(pgIndex, pgSize, userName)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            //console.log(result.Items);
            this.dataSource = new DoctorDataSource(result.Items);
            this.totalItemsCount = result.TotalCount;
            this.toastr.success("Records Loaded", "Success");
          }
        },
        errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
  }
  //Filter by User

  //Search by User
  getValuesBySearch($event: string) {
    this.IsSearchValue = $event;
    if (this.IsSearchValue != "") {
      this.loadDataBySearch(0, 5, this.IsSearchValue);
    }
    else {
      this.loadData(0, 5);
    }
  }

  loadDataBySearch(pgIndex: number, pgSize: number, searchValue: string) {
    this.showSpinner = true;
    this.smsService.getAllDoctorSendSMSBySearch(pgIndex, pgSize, searchValue)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            //console.log(result.Items);
            this.dataSource = new DoctorDataSource(result.Items);
            this.totalItemsCount = result.TotalCount;
            this.toastr.success("Records Loaded", "Success");
          }
        },
        errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
  }
  //Search by User

  //reset Filter
  resetFilter() {
    this.IsFilterValue = "";
    this.IsSearchValue = "";
    this.getValues();
  }
  //reset Filter

  onAddMobileNumber(doctor: Doctor) {
    this.sendSmsData.push({ ID: doctor.ID.toString(), Name: doctor.Name.toString(), MobileNumber: doctor.Contact.MobileNumber });
  }

  onRemoveMobileNumber(sendSMS: SendSMS): void {
    let idToRemove = sendSMS.ID;
    const idx = this.sendSmsData.map(itm => itm.ID).indexOf(idToRemove);
    this.sendSmsData.splice(idx, 1);
  }

  submitSMSForm() {
    if (this.sendSmsData.length > 0) {
      for (let i = 0; i < this.sendSmsData.length; i++) {
        let msgTxt = `Dear Dr. ${this.sendSmsData[i].Name}, ${this.MessageCtrl.value}`;
        this.sendSmsDataList.push({ MessageText: msgTxt.toString(), MobileNumber: this.sendSmsData[i].MobileNumber });
      }

      this.showSpinner = true;
      this.smsService.sendSMSToDoctor(this.sendSmsDataList)
        .finally(() => this.showSpinner = false)
        .subscribe(
          result => {
            if (result) {
              this.toastr.success("SMS Send", "Success");
            }
            else {
              this.toastr.warning("Fail To Send SMS", "Success");
            }
          },
          error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
    }
  }

  resetSMSList() {
    this.sendSmsData = [];
    this.sendSmsDataList = [];
    this.SMSFormGroup.reset();
  }
}


export class DoctorDataSource extends DataSource<Doctor> {
  constructor(private data: Doctor[]) {
    super();
  }

  connect(collectionViewer: CollectionViewer): Observable<Doctor[]> {
    return Observable.of(this.data);
  }

  disconnect(collectionViewer: CollectionViewer): void {
  }
}
