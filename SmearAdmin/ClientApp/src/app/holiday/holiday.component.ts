import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Masters } from '../shared/models/masters.interface';
import { Holiday } from '../shared/models/holiday.interface';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../shared/services/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HolidayService } from '../shared/services/holiday.service';
import { debug } from 'util';

@Component({
    selector: 'app-holiday',
    templateUrl: './holiday.component.html',
    styleUrls: ['./holiday.component.css']
})
export class HolidayComponent implements OnInit {
    showSpinner: boolean = false;
    isLinear = false;

    holidayFormGroup: FormGroup;
    CommunityList: Masters[];

    minDateFestival: any;
    maxDateFestival: any;

    //Model For Forms
    holidayData: Holiday = {
        ID: 0,
        FestivalName: '',
        FestivalDate: '',
        FestivalDay: '',
        FestivalDescription: '',
        IsNationalFestival: 0,
        BelongToCommunity: ''
    };

    constructor(private _formBuilder: FormBuilder, private holidayService: HolidayService, private userService: UserService, private toastr: ToastrService,
        private router: Router, private route: ActivatedRoute) { }

    ngOnInit() {
        //Form's Data
        this.holidayFormGroup = this._formBuilder.group({
            "FestivalNameCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
            "FestivalDateCtrl": ['', Validators.required],
            "FestivalDescriptionCtrl": ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(30)])],
            "IsNationalFestivalCtrl": ['', Validators.required],
            "CommunityCtrl": ['', Validators.required],
        });

        let today = new Date();
        this.minDateFestival = new Date(today.getFullYear() - 1, today.getMonth(), today.getDate());
        this.maxDateFestival = new Date(today.getFullYear() + 1, today.getMonth(), today.getDate());

        //Load Drop Downs
        this.loadMastersForHoliday();

        var paramID = this.route.snapshot.queryParams['id'];
        if (paramID != null) {
            this.loadHolidayToEdit(paramID.toString());
        }
        else {
            //Rest Forms
            this.holidayFormGroup.reset();
        }
    }

    get FestivalNameCtrl() { return this.holidayFormGroup.get('FestivalNameCtrl'); }
    get FestivalDateCtrl() { return this.holidayFormGroup.get('FestivalDateCtrl'); }
    get FestivalDescriptionCtrl() { return this.holidayFormGroup.get('FestivalDescriptionCtrl'); }
    get IsNationalFestivalCtrl() { return this.holidayFormGroup.get('IsNationalFestivalCtrl'); }
    get CommunityCtrl() { return this.holidayFormGroup.get('CommunityCtrl'); }

    loadMastersForHoliday() {
        this.showSpinner = true;
        this.holidayService.getMastersForHoliday()
            .finally(() => this.showSpinner = false)
            .subscribe(
                result => {
                    if (result) {
                        this.CommunityList = result.ItemsCommunity;
                    }
                },
                error => this.toastr.error(error, "Error"));
    }

    loadHolidayToEdit(ID: string) {
        this.showSpinner = true;
        this.holidayService.getHolidayByID(ID)
            .finally(() => this.showSpinner = false)
            .subscribe(
                result => {
                    if (result) {
                        this.holidayData = result;
                        this.BindHolidayValues(this.holidayData);
                    }
                },
                errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
    }

    BindHolidayValues(holiday: Holiday) {
        this.FestivalNameCtrl.setValue(holiday.FestivalName);

        var dateFestival = new Date(holiday.FestivalDate.toString().substr(0, 10));
        this.FestivalDateCtrl.setValue(dateFestival);

        this.FestivalDescriptionCtrl.setValue(holiday.FestivalDescription);
        this.IsNationalFestivalCtrl.setValue(holiday.IsNationalFestival);
        this.CommunityCtrl.setValue(holiday.BelongToCommunity);
    }

    submitHolidayForm() {
        this.holidayData.FestivalName = this.FestivalNameCtrl.value;
        this.holidayData.FestivalDate = this.FestivalDateCtrl.value;
        this.holidayData.FestivalDescription = this.FestivalDescriptionCtrl.value;
        this.holidayData.IsNationalFestival = this.IsNationalFestivalCtrl.value;
        this.holidayData.BelongToCommunity = this.CommunityCtrl.value;

        //console.log(this.stockData);
        this.showSpinner = true;
        if (this.holidayData.ID == 0) {
            console.log(this.holidayData);
            this.holidayService.addHoliday(this.holidayData)
                .finally(() => this.showSpinner = false)
                .subscribe(
                    result => {
                        if (result) {
                            this.toastr.success("Record Saved", "Success");
                            this.router.navigate(['./manage-holiday']);
                        }
                    },
                    error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
        }
        else {
            this.holidayService.editHoliday(this.holidayData)
                .finally(() => this.showSpinner = false)
                .subscribe(
                    result => {
                        if (result) {
                            this.toastr.success("Record Saved", "Success");
                            this.router.navigate(['./manage-holiday']);
                        }
                    },
                    error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
        }

    }
}
