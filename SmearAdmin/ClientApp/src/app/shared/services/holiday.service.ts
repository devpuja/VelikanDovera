import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpEvent, HttpResponse, HttpRequest, HttpEventType } from '@angular/common/http';
import { ConfigService } from '../utils/config.service';
import { Observable } from 'rxjs';
import { Masters } from '../models/masters.interface';
import { Holiday } from '../models/holiday.interface';

@Injectable({
  providedIn: 'root'
})
export class HolidayService {
  baseUrl: string = '';

  constructor(private httpClient: HttpClient, private configService: ConfigService) {
    this.baseUrl = this.configService.getApiURI();
  }

  getMastersForHoliday(): Observable<any> {
    return this.httpClient.get<Masters[]>(this.baseUrl + "api/Holiday/GetMastersForHoliday");
  }

  addHoliday(holidayVM: Holiday): Observable<any> {
    let httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.httpClient.post<any>(`${this.baseUrl}api/Holiday/AddHoliday`, holidayVM,
      { headers: httpHeaders, observe: 'response' });
  }

  getAllHolidays(pageIndex: number, pageSize: number): Observable<any> {
    const params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString());

    return this.httpClient.get<Holiday[]>(this.baseUrl + "api/Holiday/GetAllHolidays", { params });
  }
  
  deleteHoliday(id: number) {
    const params = new HttpParams().set("ID", id.toString());
    return this.httpClient.delete<Holiday>(`${this.baseUrl}api/Holiday/DeleteHoliday`, { params });
  }

  getHolidayByID(id: string): Observable<any> {
    const params = new HttpParams().set("ID", id);
    return this.httpClient.get<any>(this.baseUrl + "api/Holiday/GetHoliday", { params });
  }

  editHoliday(holiday: Holiday): Observable<Holiday> {
    return this.httpClient.put<Holiday>(this.baseUrl + "api/Holiday/EditHoliday", holiday);
  }
}
