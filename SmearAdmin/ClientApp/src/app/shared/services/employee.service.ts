import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpEvent, HttpResponse, HttpRequest, HttpEventType } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { ConfigService } from '../utils/config.service';
import { Observable, BehaviorSubject, Subject } from 'rxjs/Rx';
import { HttpParamsOptions } from '@angular/common/http/src/params';
import { UserRegistration } from '../models/user.registration.interface';
import '../../rxjs-operators';
import { Masters } from '../models/masters.interface';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  baseUrl: string = '';

  constructor(private httpClient: HttpClient, private configService: ConfigService) {
    this.baseUrl = this.configService.getApiURI();
  }

  getMastersForEmployee(): Observable<any> {
    return this.httpClient.get<Masters[]>(this.baseUrl + "api/Employee/GetMastersForEmployee");
  }

  createEmployee(userReg: UserRegistration): Observable<HttpResponse<UserRegistration>> {
    let httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.httpClient.post<UserRegistration>(
      this.baseUrl + "api/Employee/AddEmployee",
      userReg,
      { headers: httpHeaders, observe: 'response' }
    );
  }

  getAllEmployees(pageIndex: number, pageSize: number): Observable<any> {
    const params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString());

    return this.httpClient.get<UserRegistration[]>(this.baseUrl + "api/Employee/GetAllEmployee", { params });
  }

  //deleteEmployee1(usrReg: UserRegistration) {
  //  alert(usrReg.ID);
  //  const params = new HttpParams().set("ID", usrReg.ID.toString());
  //  return this.httpClient.delete<UserRegistration>(this.baseUrl + "api/Employee/DeleteEmployee", { params });
  //}

  deleteEmployee(userReg: UserRegistration, action: string): Observable<UserRegistration> {
    const params = new HttpParams().set("action", action.toString());
    return this.httpClient.put<UserRegistration>(this.baseUrl + "api/Employee/DeleteEmployee", userReg, { params });
  }

  //getEmployeeByID(id: string): Observable<any> {
  //  const params = new HttpParams().set("ID", id);
  //  return this.httpClient.get<UserRegistration>(this.baseUrl + "api/Employee/GetEmployee", { params });
  //}

  getEmployeeByID(id: string): Observable<any> {
    const params = new HttpParams().set("ID", id);
    return this.httpClient.get<any>(this.baseUrl + "api/Employee/GetEmployee", { params });
  }

  editEmployee(userReg: UserRegistration): Observable<UserRegistration> {
    //https://www.c-sharpcorner.com/article/authentication-and-claim-based-authorisation-with-asp-net-identity-core/
    return this.httpClient.put<UserRegistration>(this.baseUrl + "api/Employee/EditEmployee", userReg);
  }

  updatePassword(userReg: UserRegistration): Observable<UserRegistration> {
    return this.httpClient.put<UserRegistration>(this.baseUrl + "api/Employee/ChangePassword", userReg);
  }

  uploadPhoto(userID: string, filePhoto: File): Observable<any> {
    var frmData = new FormData();
    frmData.append('formFile', filePhoto);
    return this.httpClient.post(`${this.baseUrl}api/Employee/${userID}/UploadPhoto`, frmData, {
      reportProgress: true,
      //observe: 'events'
    });
  }
}
