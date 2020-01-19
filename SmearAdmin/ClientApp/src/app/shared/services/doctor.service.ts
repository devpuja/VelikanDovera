import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpEvent, HttpResponse, HttpRequest, HttpEventType } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { ConfigService } from '../utils/config.service';
import { Observable, BehaviorSubject, Subject } from 'rxjs/Rx';
import { HttpParamsOptions } from '@angular/common/http/src/params';
import '../../rxjs-operators';
import { Masters } from '../models/masters.interface';
import { Doctor } from '../models/doctor.interface';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  baseUrl: string = '';
  getList: any;
  getExcelFile: any;
  userService: any;
  userName: any;

  constructor(private httpClient: HttpClient, private configService: ConfigService, userService: UserService) {
    this.baseUrl = this.configService.getApiURI();
    this.userService = userService;

    this.userName = this.userService.getUserName();
  }

  getMastersForDoctor(): Observable<any> {
    return this.httpClient.get<Masters[]>(this.baseUrl + "api/Doctor/GetMastersForDoctor");
  }

  addDoctor(docVM: Doctor): Observable<any> {
    let httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.httpClient.post<any>(`${this.baseUrl}api/Doctor/AddDoctor`, docVM,
      { headers: httpHeaders, observe: 'response' });
  }

  getAllDoctors(pageIndex: number, pageSize: number): Observable<any> {
    if (this.userService.isAdmin) {
      const params = new HttpParams()
        .set("pageIndex", pageIndex.toString()).set("pageSize", pageSize.toString());

      this.getList = this.httpClient.get<Doctor[]>(this.baseUrl + "api/Doctor/GetAllDoctors", { params });
    }
    else {
      const params = new HttpParams()
        .set("pageIndex", pageIndex.toString())
        .set("pageSize", pageSize.toString())
        .set("userName", this.userName.toString());

      this.getList = this.httpClient.get<Doctor[]>(this.baseUrl + "api/Doctor/GetAllDoctorsByUser", { params });
    }

    return this.getList;
  }

  getAllDoctorsByUserName(pageIndex: number, pageSize: number, userName: string): Observable<any> {
    const params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("userName", userName.toString());

    this.getList = this.httpClient.get<Doctor[]>(this.baseUrl + "api/Doctor/GetAllDoctorsByUser", { params });

    return this.getList;
  }

  getAllDoctorsBySearch(pageIndex: number, pageSize: number, searchValue: string): Observable<any> {
    const params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("searchValue", searchValue.toString());

    this.getList = this.httpClient.get<Doctor[]>(this.baseUrl + "api/Doctor/GetAllDoctorsBySearch", { params });

    return this.getList;
  }
  

  deleteDoctor(userReg: Doctor, action: string): Observable<Doctor> {
    const params = new HttpParams().set("action", action.toString());
    return this.httpClient.put<Doctor>(this.baseUrl + "api/Doctor/DeleteDoctor", userReg, { params });
  }

  getDoctorByID(id: string): Observable<any> {
    const params = new HttpParams().set("ID", id);
    return this.httpClient.get<any>(this.baseUrl + "api/Doctor/GetDoctor", { params });
  }

  editDoctor(doc: Doctor): Observable<Doctor> {
    //https://www.c-sharpcorner.com/article/authentication-and-claim-based-authorisation-with-asp-net-identity-core/
    return this.httpClient.put<Doctor>(this.baseUrl + "api/Doctor/EditDoctor", doc);
  }

  exportDoctorsData(): Observable<any> {
    if (this.userService.isAdmin) {
      this.getExcelFile = this.httpClient.get<any>(`${this.baseUrl}api/Doctor/ExportDoctorsData`);
    }
    else {
      const params = new HttpParams()
        .set("userName", this.userName.toString());

      this.getExcelFile = this.httpClient.get<any>(`${this.baseUrl}api/Doctor/ExportDoctorsDataByUser`, { params });
    }
    return this.getExcelFile;
  }
}
