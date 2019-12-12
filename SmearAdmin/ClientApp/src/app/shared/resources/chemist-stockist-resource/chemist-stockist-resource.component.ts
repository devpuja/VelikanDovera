import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Masters } from '../../models/masters.interface';
import { ChemistService } from '../../services/chemist.service';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-chemist-stockist-resource',
    templateUrl: './chemist-stockist-resource.component.html',
    styleUrls: ['./chemist-stockist-resource.component.css']
})
export class ChemistStockistResourceComponent implements OnInit {
    @Input('parentForm')

    public commonForm: FormGroup;

    showSpinner: boolean = false;
    minDateDOB: any;
    maxDateDOB: any;
    minDateDOA: any;
    maxDateDOA: any;

    AreaList: Masters[];
    BestTimeToMeetList: Masters[];

    constructor(private chemService: ChemistService, private toastr: ToastrService, ) { }

    ngOnInit() {
        let today = new Date();
        this.minDateDOB = new Date(today.getFullYear() - 100, today.getMonth(), today.getDate());
        this.maxDateDOB = new Date(today.getFullYear() - 18, today.getMonth(), today.getDate());

        this.minDateDOA = new Date(today.getFullYear() - 80, today.getMonth(), today.getDate());
        this.maxDateDOA = new Date(today.getFullYear(), today.getMonth(), today.getDate());

        this.loadMastersForChemist();
    }


    loadMastersForChemist() {
        this.showSpinner = true;
        this.chemService.getMastersForChemist()
            .finally(() => this.showSpinner = false)
            .subscribe(
                result => {
                    if (result) {
                        this.AreaList = result.ItemsRegion;
                        this.BestTimeToMeetList = result.ItemsBestTimeToMeet;
                    }
                },
                error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error") });
    }

}
