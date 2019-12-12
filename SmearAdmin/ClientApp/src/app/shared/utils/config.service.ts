import { Injectable } from '@angular/core';
 
@Injectable()
export class ConfigService {

  _apiURI: string;

  constructor() {
    this._apiURI = 'https://localhost:44301/';
    //this._apiURI = 'http://portal.velikandovera.co.in/';
  }

  getApiURI() {
    return this._apiURI;
  }
}
