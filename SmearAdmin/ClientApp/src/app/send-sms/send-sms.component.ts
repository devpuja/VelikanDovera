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

        this.getValues();
    }

    get MessageCtrl() { return this.SMSFormGroup.get('MessageCtrl'); }

    ngAfterViewInit() {
        this.paginator.page.pipe(tap(() => this.loadPages())).subscribe();
    }

    loadPages() {
        this.loadData(this.paginator.pageIndex, this.paginator.pageSize);
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
                error => this.toastr.error(error, "Error"));
    }

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
                    error => { debugger; this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
        }
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
