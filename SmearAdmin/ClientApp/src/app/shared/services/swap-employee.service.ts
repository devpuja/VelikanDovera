import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpEvent, HttpResponse, HttpRequest, HttpEventType } from '@angular/common/http';
import { ConfigService } from '../utils/config.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SwapEmployeeService {
  baseUrl: string = '';

  constructor(private httpClient: HttpClient, private configService: ConfigService) {
    this.baseUrl = this.configService.getApiURI();
  }

  getEmployeeUserNamesList(): Observable<any> {
    return this.httpClient.get<any>(this.baseUrl + "api/SwapEmployee/GetSwapEmployeeList");
  }

  updateEmployeeUserName(userNames: any): Observable<any> {
    return this.httpClient.put<any>(this.baseUrl + "api/SwapEmployee/SwapEmployeeUserName", userNames, { responseType: 'json' });
  }

}
