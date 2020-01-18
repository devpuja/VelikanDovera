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
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EmployeeExpenses, EmployeeExpensesStatus } from '../shared/models/employee.expenses.interface';
import { UserService } from '../shared/services/user.service';
import { EmployeeExpensesService } from '../shared/services/employee-expenses.service';
import { ConstEmployeeExpensesStatus } from '../shared/models/permission.model';
import { ConfigService } from '../shared/utils/config.service';

@Component({
  selector: 'app-employee-expenses-list',
  templateUrl: './employee-expenses-list.component.html',
  styleUrls: ['./employee-expenses-list.component.css']
})
export class EmployeeExpensesListComponent implements OnInit {
  showSpinner: boolean = false;
  userName: string;
  monthYear: string;
  monthYearList: string[] = [];
  totalItemsCount: number = 0;

  expensesStatus: number = 0;
  expensesStatusName: string;
  expensesStatusColor: string;
  baseUrl: string = '';
  statusFormGroup: FormGroup;

  empExpensesStatus: EmployeeExpensesStatus =
    {
      ID: 1,
      UserName: '',      
      ExpenseMonth: '',
      Status: 0,
      FullName:'',
    };

  dataSource: EmployeeExpensesDataSource;
  displayedColumns = ['index', 'PresentType', 'ExpenseMonth', 'Date', 'HQ', 'Allowance', 'ClaimAmount', 'EmployeeRemark', 'ApprovedAmount', 'ApproverRemark', 'ApprovedBy', 'action'];
  
  constructor(public dialog: MatDialog, private _formBuilder: FormBuilder, private empExpService: EmployeeExpensesService, private userService: UserService,
    private toastr: ToastrService, private router: Router, private configService: ConfigService) {
    this.baseUrl = this.configService.getApiURI();
  }

  ngOnInit() {
    this.statusFormGroup = this._formBuilder.group({});

    this.SetUserMonthYear();
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

  getValues(event) {
    this.showSpinner = true;
    this.monthYear = (event.value === undefined) ? this.monthYear : event.value;
    this.empExpService.getEmployeeExpenses(this.userName, this.monthYear)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            this.dataSource = new EmployeeExpensesDataSource(result);
            this.totalItemsCount = result.length;

            if (result.length != 0) {
              this.expensesStatus = result[0].Status;

              if (this.expensesStatus == ConstEmployeeExpensesStatus.NotSubmitted) {
                this.expensesStatusName = ConstEmployeeExpensesStatus.NotSubmittedName;
                this.expensesStatusColor = ConstEmployeeExpensesStatus.NotSubmittedColor;
              }
              else if (this.expensesStatus == ConstEmployeeExpensesStatus.Submitted) {
                this.expensesStatusName = ConstEmployeeExpensesStatus.SubmittedName;
                this.expensesStatusColor = ConstEmployeeExpensesStatus.SubmittedColor;
              }
              else if (this.expensesStatus == ConstEmployeeExpensesStatus.Approved) {
                this.expensesStatusName = ConstEmployeeExpensesStatus.ApprovedName;
                this.expensesStatusColor = ConstEmployeeExpensesStatus.ApprovedColor;
              }
              else if (this.expensesStatus == ConstEmployeeExpensesStatus.Rejected) {
                this.expensesStatusName = ConstEmployeeExpensesStatus.RejectedName;
                this.expensesStatusColor = ConstEmployeeExpensesStatus.RejectedColor;
              }
            }
            else {
              this.expensesStatusName = ConstEmployeeExpensesStatus.NotFilledName;
              this.expensesStatusColor = ConstEmployeeExpensesStatus.RejectedColor;
            }

            this.toastr.success("Records Loaded", "Success");
          }
        },
        error => this.toastr.error(error, "Error"));
  }

    generatePdf() {
        this.showSpinner = true;

        this.empExpensesStatus.UserName = this.userName;
        this.empExpensesStatus.ExpenseMonth = this.monthYear;

        this.empExpService.generatePdfEmployeeExpense(this.userName, this.monthYear)
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

    submitExpense() {
        this.empExpensesStatus.UserName = this.userName;
        this.empExpensesStatus.ExpenseMonth = this.monthYear;

        if (this.totalItemsCount <= 0) {
            this.toastr.error("No expenses entered for month " + this.monthYear, "Error");
        }
        else {
            this.showSpinner = true;
            this.empExpService.submitEmployeeExpenses(this.empExpensesStatus)
                .finally(() => this.showSpinner = false)
                .subscribe(
                    result => {
                        if (result) {
                            this.getValues(this.monthYear);
                            this.toastr.success("Record Submitted", "Success");
                        }
                    },
                    errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
        }
    }

    onDelete(ID: number) {
        this.showSpinner = true;
        this.empExpService.deleteEmployeeExpenses(ID)
            .finally(() => this.showSpinner = false)
            .subscribe(
                result => {
                    if (result) {
                        this.getValues(this.monthYear);
                        this.toastr.success("Record Deleted", "Success");
                    }
                },
                errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
    }

    openDialogDelete(ID: number): void {
        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
            width: '250px'
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.onDelete(ID);
            }
            else {
                this.toastr.show("Delete operation cancelled.");
            }
        });
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
