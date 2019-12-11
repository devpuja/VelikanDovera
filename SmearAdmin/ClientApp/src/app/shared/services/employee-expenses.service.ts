import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpEvent, HttpResponse, HttpRequest, HttpEventType } from '@angular/common/http';
import { ConfigService } from '../utils/config.service';
import { Observable, BehaviorSubject, Subject } from 'rxjs/Rx';
import { EmployeeExpenses, EmployeeExpensesStatus } from '../models/employee.expenses.interface';
import '../../rxjs-operators';
import { GeneratePdf } from '../models/generate.pdf.interface';

@Injectable({
  providedIn: 'root'
})
export class EmployeeExpensesService {
  baseUrl: string = '';

  constructor(private httpClient: HttpClient, private configService: ConfigService) {
    this.baseUrl = this.configService.getApiURI();
  }

  getEmployeeAllowance(userID: string): Observable<any> {
    const params = new HttpParams().set("ID", userID);
    return this.httpClient.get<any>(this.baseUrl + "api/EmployeeExpenses/GetEmployeeAllowance", { params });
  }

  addEmployeeExpense(empExpVM: EmployeeExpenses): Observable<any> {
    let httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json'
    });
        
    return this.httpClient.post<any>(`${this.baseUrl}api/EmployeeExpenses/AddEmployeeExpenses`, empExpVM,
      { headers: httpHeaders, observe: 'response' });
  }

  getEmployeeExpenses(userName: string, monthYear: string) {
    const params = new HttpParams()
      .set("UserName", userName)
      .set("MonthYear", monthYear);

    return this.httpClient.get<any>(this.baseUrl + "api/EmployeeExpenses/GetAllEmployeeExpenses", { params });
  }

  deleteEmployeeExpenses(ID: number): Observable<any> {
    const params = new HttpParams().set("ID", ID.toString());
    return this.httpClient.delete<any>(`${this.baseUrl}api/EmployeeExpenses/DeleteEmployeeExpenses`, { params });
  }

  submitEmployeeExpenses(empExpStatusVM: EmployeeExpensesStatus): Observable<any> {
    return this.httpClient.put<any>(`${this.baseUrl}api/EmployeeExpenses/ChangeEmployeeExpenses`, empExpStatusVM);
  }

  generatePdfEmployeeExpense(userName: string, monthYear: string): Observable<GeneratePdf> {
    const httpParams = new HttpParams()
      .set("UserName", userName)
      .set("MonthYear", monthYear);

    return this.httpClient.get<GeneratePdf>(`${this.baseUrl}api/EmployeeExpenses/GeneratePdfEmployeeExpense`, { params: httpParams });
  }

  getNotification(userName: string): Observable<EmployeeExpensesStatus> {
    const httpParams = new HttpParams()
      .set("UserName", userName);

    return this.httpClient.get<EmployeeExpensesStatus>(`${this.baseUrl}api/EmployeeExpenses/GetAllNotification`, { params: httpParams });
  }
}
