import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpEvent, HttpResponse, HttpRequest, HttpEventType } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { ConfigService } from '../utils/config.service';
import { Observable, BehaviorSubject, Subject } from 'rxjs/Rx';
import { HttpParamsOptions } from '@angular/common/http/src/params';
import { UserRegistration } from '../models/user.registration.interface';
import '../../rxjs-operators';
import { Masters } from '../models/masters.interface';
import { Stockist } from '../models/stockist.interface';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class StockistService {
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

  getMastersForStockist(): Observable<any> {
    return this.httpClient.get<Masters[]>(this.baseUrl + "api/Stockist/GetMastersForStockist");
  }

  addStockist(stockistVM: Stockist): Observable<any> {
    let httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.httpClient.post<any>(`${this.baseUrl}api/Stockist/AddStockist`, stockistVM,
      { headers: httpHeaders, observe: 'response' });
  }

  getAllStockists(pageIndex: number, pageSize: number): Observable<any> {
    let userName = this.userService.getUserName();

    if (this.userService.isAdmin) {
      const params = new HttpParams()
        .set("pageIndex", pageIndex.toString())
        .set("pageSize", pageSize.toString());

      this.getList = this.httpClient.get<Stockist[]>(this.baseUrl + "api/Stockist/GetAllStockists", { params });
    }
    else {
      const params = new HttpParams()
        .set("pageIndex", pageIndex.toString())
        .set("pageSize", pageSize.toString())
        .set("userName", userName.toString());

      this.getList = this.httpClient.get<Stockist[]>(this.baseUrl + "api/Stockist/GetAllStockistsByUser", { params });
    }

    return this.getList;
  }

  getAllStockistsByUserName(pageIndex: number, pageSize: number, userName: string): Observable<any> {

    const params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("userName", userName.toString());

    this.getList = this.httpClient.get<Stockist[]>(this.baseUrl + "api/Stockist/GetAllStockistsByUser", { params });

    return this.getList;
  }

  getAllStockistsBySearch(pageIndex: number, pageSize: number, searchValue: string): Observable<any> {
    const params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("searchValue", searchValue.toString());

    this.getList = this.httpClient.get<Stockist[]>(this.baseUrl + "api/Stockist/GetAllStockistsBySearch", { params });

    return this.getList;
  }

  deleteStockist(stock: Stockist, action: string): Observable<Stockist> {
    const params = new HttpParams().set("action", action.toString());
    return this.httpClient.put<Stockist>(this.baseUrl + "api/Stockist/DeleteStockist", stock, { params });
  }

  getStockistByID(id: string): Observable<any> {
    const params = new HttpParams().set("ID", id);
    return this.httpClient.get<any>(this.baseUrl + "api/Stockist/GetStockist", { params });
  }

  editStockist(stock: Stockist): Observable<Stockist> {
    return this.httpClient.put<Stockist>(this.baseUrl + "api/Stockist/EditStockist", stock);
  }

  exportStockistData(): Observable<any> {
    if (this.userService.isAdmin) {
      this.getExcelFile = this.httpClient.get<any>(`${this.baseUrl}api/Stockist/ExportStockistsData`);
    }
    else {
      const params = new HttpParams()
        .set("userName", this.userName.toString());

      this.getExcelFile = this.httpClient.get<any>(`${this.baseUrl}api/Stockist/ExportStockistsDataByUser`, { params });
    }
    return this.getExcelFile;
  }
}
