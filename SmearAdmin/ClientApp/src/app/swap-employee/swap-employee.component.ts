import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SwapEmployeeService } from '../shared/services/swap-employee.service';

@Component({
    selector: 'app-swap-employee',
    templateUrl: './swap-employee.component.html',
    styleUrls: ['./swap-employee.component.css']
})
export class SwapEmployeeComponent implements OnInit {

    showSpinner: boolean = false;
    isLinear = false;

    swapEmpFormGroup: FormGroup;
    EmployeeList: any;

    //Model For Forms
    userNameData: any = {
        SwapFrom: '',
        SwapTo: '',
    };

    constructor(private _formBuilder: FormBuilder, private swapService: SwapEmployeeService, private toastr: ToastrService, private router: Router, private route: ActivatedRoute) { }

    ngOnInit() {
        //Form's Data
        this.swapEmpFormGroup = this._formBuilder.group({
            "SwapFromCtrl": ['', Validators.required],
            "SwapToCtrl": ['', Validators.required],
        });


        this.loadEmployeeUserNames();
    }

    get SwapFromCtrl() { return this.swapEmpFormGroup.get('SwapFromCtrl'); }
    get SwapToCtrl() { return this.swapEmpFormGroup.get('SwapToCtrl'); }


    loadEmployeeUserNames() {
        this.showSpinner = true;
        this.swapService.getEmployeeUserNamesList()
            .finally(() => this.showSpinner = false)
            .subscribe(
                result => {
                    if (result) {
                        this.EmployeeList = result;
                    }
                },
                errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
    }

    submitSwapForm() {
        this.userNameData.SwapFrom = this.SwapFromCtrl.value;
        this.userNameData.SwapTo = this.SwapToCtrl.value;

        this.showSpinner = true;
        if (this.userNameData.SwapFrom !== this.userNameData.SwapTo) {
            this.swapService.updateEmployeeUserName(this.userNameData)
                .finally(() => this.showSpinner = false)
                .subscribe(
                    result => {
                        if (result) {
                            if (result.status == "ok")
                                this.toastr.success("Record Saved", "Success");
                            else
                                this.toastr.warning(result.status, "Info");
                        }
                    },
                    error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
        }
        else {
            this.showSpinner = false;
            this.toastr.info("Both the user name's are same", "Info");
        }

    }

}
