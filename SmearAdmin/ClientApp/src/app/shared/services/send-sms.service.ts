import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpEvent, HttpResponse, HttpRequest, HttpEventType } from '@angular/common/http';
import { ConfigService } from '../utils/config.service';
import { Observable } from 'rxjs';
import { SendSMSList } from '../models/send-sms.interface';

@Injectable({
    providedIn: 'root'
})
export class SendSmsService {
    baseUrl: string = '';

    constructor(private httpClient: HttpClient, private configService: ConfigService) {
        this.baseUrl = this.configService.getApiURI();
    }

    getAllDoctorSendSMS(pageIndex: number, pageSize: number): Observable<any> {
        const params = new HttpParams()
            .set("pageIndex", pageIndex.toString())
            .set("pageSize", pageSize.toString());

        return this.httpClient.get<any>(`${this.baseUrl}api/SendSMS/GetAllDoctorSendSMS`, { params });
    }

  getAllDoctorSendSMSByUserName(pageIndex: number, pageSize: number, userName: string): Observable<any> {
    const params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("userName", userName.toString());

    return this.httpClient.get<any>(`${this.baseUrl}api/SendSMS/GetAllDoctorSendSMSByUser`, { params });
  }

  getAllDoctorSendSMSBySearch(pageIndex: number, pageSize: number, searchValue: string): Observable<any> {
    const params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("searchValue", searchValue.toString());

    return this.httpClient.get<any>(`${this.baseUrl}api/SendSMS/GetAllDoctorSendSMSBySearch`, { params });
  }

    //Send SMS
    sendSMSToDoctor(sendSMSLst: SendSMSList[]): Observable<any> {
        let httpHeaders = new HttpHeaders({
            'Content-Type': 'application/json'
        });

        return this.httpClient.post<any>(`${this.baseUrl}api/SendSMS/SendSMSData`, sendSMSLst,
            { headers: httpHeaders });
    }

    getSMSCount() {
        return this.httpClient.get<any>(`${this.baseUrl}api/SendSMS/GetSendSMSCount`);
    }
}
