import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpEvent, HttpResponse, HttpRequest, HttpEventType } from '@angular/common/http';
import { ConfigService } from '../utils/config.service';
import { Observable, BehaviorSubject, Subject } from 'rxjs/Rx';
import { EmployeeExpenses, EmployeeExpensesStatus } from '../models/employee.expenses.interface';
import '../../rxjs-operators';
import { GeneratePdf } from '../models/generate.pdf.interface';
import { ResponseContentType } from '@angular/http';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl: string = '';

  constructor(private httpClient: HttpClient, private configService: ConfigService) {
    this.baseUrl = this.configService.getApiURI();
  }

  getSubmitNotification(): Observable<any> {
    return this.httpClient.get<any>(`${this.baseUrl}api/AdminDashboard/GetAllSubmitNotification`);
  }

  getAllEmployeeUserName(): Observable<any> {
    return this.httpClient.get<any>(`${this.baseUrl}api/AdminDashboard/GetAllUserName`);
  }

  getEmployeeExpenses(userName: string, monthYear: string) {
    const params = new HttpParams()
      .set("UserName", userName)
      .set("MonthYear", monthYear);

    return this.httpClient.get<any>(`${this.baseUrl}api/AdminDashboard/GetEmployeeExpensesInMonth`, { params });
  }

  saveEmployeeExpenses(empExp: EmployeeExpenses): Observable<any> {
    return this.httpClient.put<EmployeeExpenses>(`${this.baseUrl}api/AdminDashboard/SaveEmployeeExpenses`, empExp);
  }

  closeEmployeeExpenses(empExp: EmployeeExpensesStatus): Observable<any> {
    return this.httpClient.put<EmployeeExpensesStatus>(`${this.baseUrl}api/AdminDashboard/CloseEmployeeExpenses`, empExp);
  }

  generatePdfEmployeeExpense(userName: string, monthYear: string): Observable<GeneratePdf> {
    const httpParams = new HttpParams()
      .set("UserName", userName)
      .set("MonthYear", monthYear);

    return this.httpClient.get<GeneratePdf>(`${this.baseUrl}api/AdminDashboard/GeneratePdfEmployeeExpense`, { params: httpParams });
  }
}
