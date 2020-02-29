import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatDialog } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { EmployeeService } from '../shared/services/employee.service';
import { tap } from 'rxjs/operators';
import { UserRegistration } from '../shared/models/user.registration.interface';
import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { Observable } from 'rxjs';
import { ConfirmDialogComponent } from '../shared/dialog/confirm-dialog.component';
import { debug } from 'util';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EmployeeExpenses, EmployeeExpensesStatus } from '../shared/models/employee.expenses.interface';
import { UserService } from '../shared/services/user.service';
import { EmployeeExpensesService } from '../shared/services/employee-expenses.service';
import { ConstEmployeeExpensesStatus } from '../shared/models/permission.model';
import { AdminService } from '../shared/services/admin.service';
import { error } from 'protractor';
import { ConfigService } from '../shared/utils/config.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
    selector: 'app-manage-employee-expenses-list',
    templateUrl: './manage-employee-expenses-list.component.html',
    styleUrls: ['./manage-employee-expenses-list.component.css']
})
export class ManageEmployeeExpensesListComponent implements OnInit {
    showSpinner: boolean = false;
    userName: string;
    userNameList: string[] = [];
    monthYear: string;
    monthYearList: string[] = [];
    totalItemsCount: number = 0;

    baseUrl: string = '';

    empExpensesStatus: EmployeeExpensesStatus =
        {
            ID: 0,
            UserName: '',
            ExpenseMonth: '',
            Status: 0,
            FullName: '',
        };

    dataSource: EmployeeExpensesDataSource;
    displayedColumns = ['index', 'PresentType', 'ExpenseMonth', 'Date', 'HQ', 'Allowance', 'ClaimAmount', 'EmployeeRemark', 'ApprovedAmount', 'ApproverRemark', 'action'];

    constructor(public dialog: MatDialog, private _formBuilder: FormBuilder, private adminService: AdminService, private userService: UserService,
        private toastr: ToastrService, private activatedRoute: ActivatedRoute, private configService: ConfigService) {
        this.baseUrl = this.configService.getApiURI();
    }

    ngOnInit() {
        this.SetUserMonthYear();
        this.LoadUserNames();

        //Check Urls
        this.activatedRoute.queryParams.subscribe(params => {
            this.userName = params['user'];
            this.monthYear = params['month'];
        });

        if (this.userName != undefined && this.monthYear != undefined) {
            this.findExpense();
        }
    }
  
  SetUserMonthYear() {
    this.userName = this.userService.getUserName();

    let moyr = "";
    var today = new Date();

    for (var dt = 0; dt <= 5; dt++) {
      var makeDate = new Date(today);

      if (dt > 0) {
        makeDate.setMonth(makeDate.getMonth() - dt);
      }

      moyr = makeDate.getMonth() + 1 + "-" + makeDate.getFullYear();
      this.monthYearList.push(moyr);
    }
  }


    LoadUserNames() {
        this.showSpinner = true;
        this.adminService.getAllEmployeeUserName()
            .finally(() => this.showSpinner = false)
            .subscribe(
                result => {
                    if (result) {
                        this.userNameList = result;
                    }
                },
                errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
    }

    changeUser(event) {
        this.userName = (event.value === undefined) ? this.userName : event.value;
    }

    changeMonth(event) {
        this.monthYear = (event.value === undefined) ? this.monthYear : event.value;
    }

    findExpense() {
        this.showSpinner = true;
        this.adminService.getEmployeeExpenses(this.userName, this.monthYear)
            .finally(() => this.showSpinner = false)
            .subscribe(
                result => {
                    if (result) {
                        this.dataSource = new EmployeeExpensesDataSource(result);
                        this.totalItemsCount = result.length;

                        this.toastr.success("Records Loaded", "Success");
                    }
                },
                errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
    }

    saveEmployeeExpense(empExp) {
        //Check if ApprovedAmount is Number only
        empExp.ApprovedBy = this.userService.getFullName();

        var isAmtNbr = Number(empExp.ApprovedAmount);
        if (!isNaN(isAmtNbr)) {
            empExp.ApprovedAmount = isAmtNbr;

            this.showSpinner = true;
            this.adminService.saveEmployeeExpenses(empExp)
                .finally(() => this.showSpinner = false)
                .subscribe(
                    result => {
                        if (result) {
                            this.toastr.success("Records Loaded", "Success");
                        }
                    },
                    errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
        }
        else {
            this.toastr.error("Incorrect Amount Entered.", "Error");
        }
    }

    closeExpense() {
        this.showSpinner = true;
        this.empExpensesStatus.UserName = this.userName;
        this.empExpensesStatus.ExpenseMonth = this.monthYear;

        this.adminService.closeEmployeeExpenses(this.empExpensesStatus)
            .finally(() => this.showSpinner = false)
            .subscribe(
                result => {
                    if (result) {
                        this.toastr.success("Records Loaded", "Success");
                    }
                },
                errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
    }

    generatePdf() {
        this.showSpinner = true;

        this.empExpensesStatus.UserName = this.userName;
        this.empExpensesStatus.ExpenseMonth = this.monthYear;

        this.adminService.generatePdfEmployeeExpense(this.userName, this.monthYear)
            .finally(() => this.showSpinner = false)
            .subscribe(
                result => {
                    var url = this.baseUrl.toString() + "downloads/" + result.FileName;
                    const downloadLink = document.createElement("a");
                    downloadLink.style.display = "none";
                    document.body.appendChild(downloadLink);
                    downloadLink.setAttribute("href", url);
                    downloadLink.setAttribute("download", result.FileName); //downloads by browser
                    downloadLink.setAttribute("target", "_blank");
                    downloadLink.click();
                    document.body.removeChild(downloadLink);

                    this.toastr.success("Records Loaded", "Success");
                },
                errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
    }
}

export class EmployeeExpensesDataSource extends DataSource<EmployeeExpenses> {
    constructor(private data: EmployeeExpenses[]) {
        super();
    }

    connect(collectionViewer: CollectionViewer): Observable<EmployeeExpenses[]> {
        return Observable.of(this.data);
    }

    disconnect(collectionViewer: CollectionViewer): void {
    }
}
