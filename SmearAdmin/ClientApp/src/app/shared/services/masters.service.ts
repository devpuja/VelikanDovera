import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpEvent, HttpResponse } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { Masters } from '../models/masters.interface';
import { ConfigService } from '../utils/config.service';
import { Observable, BehaviorSubject } from 'rxjs/Rx';
import { HttpParamsOptions } from '@angular/common/http/src/params';
import '../../rxjs-operators';

@Injectable({
  providedIn: 'root'
})
export class MastersService {
  baseUrl: string = '';

  constructor(private httpClient: HttpClient, private configService: ConfigService) {
    this.baseUrl = this.configService.getApiURI();   
  }

  create(masters: Masters): Observable<HttpResponse<Masters>> {
    let httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.httpClient.post<Masters>(this.baseUrl + "api/Masters/AddMaster", masters, { headers: httpHeaders, observe: 'response' });
  }

  //using Body
  //create(masters: Masters): Observable<Masters> {
  //  let httpHeaders = new HttpHeaders({
  //    'Content-Type': 'application/json'
  //  });
  //  return this.httpClient.post<Masters>(this.baseUrl + "api/Masters/AddMaster", masters, { headers: httpHeaders, observe: 'body',responseType: 'json' });
  //}


  getAllMasters(selMst: string, pageIndex: number, pageSize: number): Observable<any> {
    const params = new HttpParams()
      .set("masterFor", selMst)
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString());

    return this.httpClient.get<Masters[]>(this.baseUrl + "api/Masters/GetAllMasters", { params });
  }

  deleteMasters(id: number) {
    const params = new HttpParams().set("ID", id.toString());
    return this.httpClient.delete<Masters>(this.baseUrl + "api/Masters/DeleteMaster", { params });
  }

  getMastersByID(id: number) {
    const params = new HttpParams().set("ID", id.toString());
    return this.httpClient.get<Masters>(this.baseUrl + "api/Masters/GetMastersByID", { params });
  }

  editMasters(masters: Masters): Observable<Masters> {
    return this.httpClient.put<Masters>(this.baseUrl + "api/Masters/EditMaster", masters);
  }
}
