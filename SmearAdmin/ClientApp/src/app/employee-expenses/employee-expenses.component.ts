import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, FormControl, ReactiveFormsModule, Validators, AbstractControl, FormArray, ValidatorFn } from '@angular/forms';
import { MastersFor, Masters } from '../shared/models/masters.interface';
import { EmployeeExpenses } from '../shared/models/employee.expenses.interface';
import { ToastrService } from 'ngx-toastr';
import { EmployeeExpensesService } from '../shared/services/employee-expenses.service';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from '../shared/services/user.service';
import { debug } from 'util';
import { PresentType } from '../shared/models/permission.model';

@Component({
  selector: 'app-employee-expenses',
  templateUrl: './employee-expenses.component.html',
  styleUrls: ['./employee-expenses.component.css']
})
export class EmployeeExpensesComponent implements OnInit {
  showSpinner: boolean = false;
  expensesFormGroup: FormGroup;
  minDateExp: any;
  maxDateExp: any;

  PresentType: Masters[];
  Daily: Masters;
  Bike: Masters;
  HQ: Masters;
  HQValue: string;
  Region: Masters[];
  RegionName: string[];

  DailyAmount: number = 0;
  BikeAmount: number = 0;
  OtherAmount: number = 0;

  empExpenses: EmployeeExpenses =
    {
      ID: 0,
      UserName: '',
      PresentType: '',
      ExpenseMonth: '',
      Date: '',
      HQ: 0,
      HQName: '',
      Region: '',
      BikeAllowance: 0,
      DailyAllowance: 0,
      OtherAmount: 0,
      ClaimAmount: 0,
      EmployeeRemark: '',
      ApprovedAmount: 0,
      ApprovedBy: '',
      ApproverRemark: '',
      Status: 0,
    };

  constructor(private _formBuilder: FormBuilder, private empExpService: EmployeeExpensesService, private userService: UserService,
    private toastr: ToastrService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.expensesFormGroup = this._formBuilder.group({
      "PresentTypeCtrl": ['', Validators.required],
      "ExpDateCtrl": ['', Validators.required],
      "RegionCtrl": ['', Validators.required],
      "BikeAllowanceCtrl": ['', Validators.compose([Validators.required, Validators.maxLength(3)])],
      "DailyAllowanceCtrl": ['', Validators.compose([Validators.required, Validators.maxLength(3)])],
      "OtherAmountCtrl": ['', Validators.compose([Validators.required, Validators.maxLength(4)])],
      "EmployeeRemarkCtrl": ['']
    });

    //Set Month & Year
    this.SetMonthYear();

    //Load DropDowns
    var userID = this.userService.getUserID();
    if (userID != null) {
      this.loadEmployeeAllowance(userID);
    }
  }

  get PresentTypeCtrl() { return this.expensesFormGroup.get('PresentTypeCtrl'); }
  get ExpDateCtrl() { return this.expensesFormGroup.get('ExpDateCtrl'); }
  get RegionCtrl() { return this.expensesFormGroup.get('RegionCtrl'); }
  get BikeAllowanceCtrl() { return this.expensesFormGroup.get('BikeAllowanceCtrl'); }
  get DailyAllowanceCtrl() { return this.expensesFormGroup.get('DailyAllowanceCtrl'); }
  get OtherAmountCtrl() { return this.expensesFormGroup.get('OtherAmountCtrl'); }
  get EmployeeRemarkCtrl() { return this.expensesFormGroup.get('EmployeeRemarkCtrl'); }

  SetMonthYear() {
    let today = new Date();
    this.minDateExp = new Date(today.getFullYear(), today.getMonth() - 3, today.getDate());
    this.maxDateExp = new Date(today.getFullYear(), today.getMonth(), today.getDate());
  }

  loadEmployeeAllowance(userID: string) {
    this.showSpinner = true;
    this.empExpService.getEmployeeAllowance(userID)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            //console.log(result);
            this.PresentType = result.ItemsPresentType;
            this.Daily = result.ItemsDaily;
            this.Bike = result.ItemsBike;
            //this.HQ = result.ItemsHQ;
            this.HQ = result.HQ;
            this.Region = result.ItemsRegion;
            this.RegionName = result.ItemsRegionName;

            this.HQValue = this.HQ.Value.toString();
            this.RegionCtrl.setValue(this.RegionName);
            this.DailyAllowanceCtrl.setValue(+this.Daily.Value);
            this.BikeAllowanceCtrl.setValue(+this.Bike.Value);
            this.OtherAmountCtrl.setValue(+0);

            this.DailyAmount = +this.Daily.Value;
            this.BikeAmount = +this.Bike.Value;

            this.expensesFormGroup.controls['DailyAllowanceCtrl'].setValidators(Validators.compose([Validators.required, Validators.maxLength(3), maxValue(this.DailyAmount)]));
            this.expensesFormGroup.controls['BikeAllowanceCtrl'].setValidators(Validators.compose([Validators.required, Validators.maxLength(3), maxValue(this.BikeAmount)]));
            
          }
        },
        error => this.toastr.error(error, "Error"));
  }

  onPresentTypeChange(event) {
    if (this.PresentTypeCtrl.value != undefined) {
      if (this.PresentTypeCtrl.value == PresentType.FULLDAY) {
        this.enableAllowance();

        this.DailyAmount = +this.Daily.Value;
        this.BikeAmount = +this.Bike.Value;
        this.OtherAmount = +0;

        this.DailyAllowanceCtrl.setValue(this.DailyAmount);
        this.BikeAllowanceCtrl.setValue(this.BikeAmount);
        this.OtherAmountCtrl.setValue(this.OtherAmount);
      }
      else if (this.PresentTypeCtrl.value == PresentType.HALFDAY) {
        this.enableAllowance();

        this.DailyAmount = +this.Daily.Value / 2;
        this.BikeAmount = +this.Bike.Value / 2;
        this.OtherAmount = +0;

        this.DailyAllowanceCtrl.setValue(this.DailyAmount);
        this.BikeAllowanceCtrl.setValue(this.BikeAmount);
        this.OtherAmountCtrl.setValue(this.OtherAmount);
      }
      else if (this.PresentTypeCtrl.value == PresentType.LEAVE) {
        this.isOnLeave();
      }
    }
    else {
      this.isOnLeave();
    }
  }

  isOnLeave() {
    this.DailyAllowanceCtrl.setValue(0);
    this.BikeAllowanceCtrl.setValue(0);
    this.OtherAmountCtrl.setValue(0);

    this.DailyAllowanceCtrl.disable();
    this.BikeAllowanceCtrl.disable();
    this.OtherAmountCtrl.disable();
  }

  enableAllowance() {
    this.DailyAllowanceCtrl.enable()
    this.BikeAllowanceCtrl.enable();
    this.OtherAmountCtrl.enable();
  }

  submitEmployeeExpensesForm() {
    this.empExpenses.PresentType = this.PresentTypeCtrl.value;
    this.empExpenses.Date = this.ExpDateCtrl.value;
    this.empExpenses.Region = this.RegionCtrl.value.join();
    this.empExpenses.BikeAllowance = this.BikeAllowanceCtrl.value;
    this.empExpenses.DailyAllowance = this.DailyAllowanceCtrl.value;
    this.empExpenses.OtherAmount = this.OtherAmountCtrl.value;
    this.empExpenses.EmployeeRemark = this.EmployeeRemarkCtrl.value;
    this.empExpenses.UserName = this.userService.getUserName();
    this.empExpenses.HQ = +this.HQ.Id;
    this.empExpenses.HQName = this.HQValue;
    
    this.showSpinner = true;
    if (this.empExpenses.ID == 0) {
      this.empExpService.addEmployeeExpense(this.empExpenses)
        .finally(() => this.showSpinner = false)
        .subscribe(
          result => {
            if (result) {
              this.toastr.success("Record Saved", "Success");
              this.resetControls();
              //this.router.navigate(['./employee-expenses-list']);
            }
          },
          error => this.toastr.error(error, "Error"));
    }
  }

  resetControls() {
    this.expensesFormGroup.controls["PresentTypeCtrl"].setValue("");
    this.expensesFormGroup.controls["ExpDateCtrl"].reset();
    this.expensesFormGroup.controls["BikeAllowanceCtrl"].reset();
    this.expensesFormGroup.controls["DailyAllowanceCtrl"].reset();
    this.expensesFormGroup.controls["OtherAmountCtrl"].reset();
    this.expensesFormGroup.controls["EmployeeRemarkCtrl"].reset();
  }
}

export function maxValue(max: Number): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } => {
    const input = control.value,
      isValid = input > max;
    if (isValid)
      return { 'maxValue': { max } }
    else
      return null;
  };
}

