import { Component, OnInit } from '@angular/core';
import { AdminService } from '../shared/services/admin.service';
import { ToastrService } from 'ngx-toastr';
import { EmployeeExpensesStatus } from '../shared/models/employee.expenses.interface';
import { Router } from '@angular/router';
import { SendSmsService } from '../shared/services/send-sms.service';

@Component({
    selector: 'app-admin-dashboard',
    templateUrl: './admin-dashboard.component.html',
    styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
    showSpinner: boolean = false;
    empExpensesStatus: EmployeeExpensesStatus[]
    SMSBalanceCount: number = 0;
    SMSDoctorCount: number = 0;
    SMSChemsitCount: number = 0;
    SMSStockistCount: number = 0;

    constructor(private adminService: AdminService, private smsService: SendSmsService, private toastr: ToastrService, private router: Router) { }

    ngOnInit() {
        this.employeeExpenseList();
        this.smsStatus();
    }

    async employeeExpenseList() {
        this.showSpinner = true;
        await this.adminService.getSubmitNotification()
            .finally(() => this.showSpinner = false)
            .subscribe(
                result => {
                    if (result) {
                        this.empExpensesStatus = result;
                        this.toastr.success("Record Loaded", "Success");
                    }
                },
                error => this.toastr.error(error, "Error"));
    }

    navigate(exp) {
        this.router.navigate(['/manage-employee-expenses-list'], { queryParams: { user: exp.UserName, month: exp.ExpenseMonth } });
    }

    async smsStatus() {
        this.showSpinner = true;
        await this.smsService.getSMSCount()
            .finally(() => this.showSpinner = false)
            .subscribe(
                result => {
                    if (result) {
                        this.bindSMSValues(result);
                        this.toastr.success("Record Loaded", "Success");
                    }
                },
                error => this.toastr.error(error, "Error"));
    }

    bindSMSValues(smsData) {
        //console.log(smsData);
        var itemsCount = smsData.SendSMSToNameItems;
        if (itemsCount.length > 0) {
            for (var i = 0; i <= itemsCount.length - 1; i++) {
                if (itemsCount[i].SendSMSToName === "Doctor")
                    this.SMSDoctorCount = itemsCount[i].Count;

                if (itemsCount[i].SendSMSToName === "Chemist")
                    this.SMSChemsitCount = itemsCount[i].Count;

                if (itemsCount[i].SendSMSToName === "Stockist")
                    this.SMSStockistCount = itemsCount[i].Count;
            }
        }

        this.SMSBalanceCount = smsData.SMSBalance;
    }
}
