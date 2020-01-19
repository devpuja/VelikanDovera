import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpEvent, HttpResponse, HttpRequest, HttpEventType } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { ConfigService } from '../utils/config.service';
import { Observable, BehaviorSubject, Subject } from 'rxjs/Rx';
import { HttpParamsOptions } from '@angular/common/http/src/params';
import { UserRegistration } from '../models/user.registration.interface';
import '../../rxjs-operators';
import { Masters } from '../models/masters.interface';
import { Chemist } from '../models/chemist.interface';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class ChemistService {
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

  getMastersForChemist(): Observable<any> {
    return this.httpClient.get<Masters[]>(this.baseUrl + "api/Chemist/GetMastersForChemist");
  }

  addChemist(chemistVM: Chemist): Observable<any> {
    let httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.httpClient.post<any>(`${this.baseUrl}api/Chemist/AddChemist`, chemistVM,
      { headers: httpHeaders, observe: 'response' });
  }

  getAllChemists(pageIndex: number, pageSize: number): Observable<any> {
    let userName = this.userService.getUserName();

    if (this.userService.isAdmin) {
      const params = new HttpParams()
        .set("pageIndex", pageIndex.toString())
        .set("pageSize", pageSize.toString());

      this.getList = this.httpClient.get<Chemist[]>(this.baseUrl + "api/Chemist/GetAllChemists", { params });
    }
    else {
      const params = new HttpParams()
        .set("pageIndex", pageIndex.toString())
        .set("pageSize", pageSize.toString())
        .set("userName", userName.toString());

      this.getList = this.httpClient.get<Chemist[]>(this.baseUrl + "api/Chemist/GetAllChemistsByUser", { params });
    }
    return this.getList;
  }

  getAllChemistsByUserName(pageIndex: number, pageSize: number, userName: string): Observable<any> {
    const params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("userName", userName.toString());

    this.getList = this.httpClient.get<Chemist[]>(this.baseUrl + "api/Chemist/GetAllChemistsByUser", { params });
    return this.getList;
  }

  getAllChemistsBySearch(pageIndex: number, pageSize: number, searchValue: string): Observable<any> {
    const params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("searchValue", searchValue.toString());

    this.getList = this.httpClient.get<Chemist[]>(this.baseUrl + "api/Chemist/GetAllChemistsBySearch", { params });
    return this.getList;
  }

  deleteChemist(chem: Chemist, action: string): Observable<Chemist> {
    const params = new HttpParams().set("action", action.toString());
    return this.httpClient.put<Chemist>(this.baseUrl + "api/Chemist/DeleteChemist", chem, { params });
  }

  getChemistByID(id: string): Observable<any> {
    const params = new HttpParams().set("ID", id);
    return this.httpClient.get<any>(this.baseUrl + "api/Chemist/GetChemist", { params });
  }

  editChemist(chem: Chemist): Observable<Chemist> {
    return this.httpClient.put<Chemist>(this.baseUrl + "api/Chemist/EditChemist", chem);
  }

  exportChemistData(): Observable<any> {
    if (this.userService.isAdmin) {
      this.getExcelFile = this.httpClient.get<any>(`${this.baseUrl}api/Chemist/ExportChemistsData`);
    }
    else {
      const params = new HttpParams()
        .set("userName", this.userName.toString());

      this.getExcelFile = this.httpClient.get<any>(`${this.baseUrl}api/Chemist/ExportChemistsDataByUser`, { params });
    }
    return this.getExcelFile;
  }
}
