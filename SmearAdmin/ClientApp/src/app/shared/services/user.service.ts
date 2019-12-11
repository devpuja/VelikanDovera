import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Router} from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ConfigService } from '../utils/config.service';

import { BaseService } from "../services/base.service";

import { Observable, BehaviorSubject } from 'rxjs/Rx';

import { LocalStorage } from '@ngx-pwa/local-storage';
import { JwtHelper } from '../utils/jwt-helper';
import { IdToken } from '../models/login-response';
import { PermissionValues, Permission, Roles } from '../models/permission.model';
import '../../rxjs-operators';

@Injectable()

export class UserService extends BaseService {
    baseUrl: string = '';

  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private loggedIn = false;

  constructor(private http: Http, private configService: ConfigService,
    protected localStorage: LocalStorage, private router: Router, private toastr: ToastrService) {

        super();
        this.loggedIn = !!localStorage.getItem('auth_token');
        // ?? not sure if this the best way to broadcast the status but seems to resolve issue on page refresh where auth status is lost in
        // header component resulting in authed user nav links disappearing despite the fact user is still logged in
        this._authNavStatusSource.next(this.loggedIn);
    this.baseUrl = configService.getApiURI();
    }

  login(userName: string, password: string) {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http
      .post(
        this.baseUrl + 'api/Auth/login',
        JSON.stringify({ userName, password }), { headers }
      )
      .map(res => res.json())
      .map(res => {
        localStorage.setItem('auth_token', res.auth_token);
        this.loggedIn = true;
        this._authNavStatusSource.next(true);
        //var getPermissionsLst = this.getPermissions();
        //console.log(getPermissionsLst);
        return true;
      })
      .catch(this.handleError)
      //.catch((err: any) => {
      //  Observable.throw(this.toastr.error(err.error.message, "Error"));
      //});
  }

  logout(){
    //localStorage.removeItem('auth_token');
    localStorage.clear();
    this.loggedIn = false;
    this._authNavStatusSource.next(false);
    this.router.navigate(['./login']);
  }

  isLoggedIn() {
    let authToken = localStorage.getItem('auth_token');
    return this.loggedIn = (authToken == null) ? false : true;    
  }

  getUserID() {
    let accessToken = localStorage.getItem('auth_token');
    let jwtHelper = new JwtHelper();
    let decodedIdToken = <IdToken>jwtHelper.decodeToken(accessToken);
    return decodedIdToken.id;
  }

  getUserName() {
    let accessToken = localStorage.getItem('auth_token');
    let jwtHelper = new JwtHelper();
    let decodedIdToken = <IdToken>jwtHelper.decodeToken(accessToken);
    return decodedIdToken.username;
  }

  getFullName() {
    let accessToken = localStorage.getItem('auth_token');
    let jwtHelper = new JwtHelper();
    let decodedIdToken = <IdToken>jwtHelper.decodeToken(accessToken);
    return decodedIdToken.fullname;
  }

  getRoles() {
    let accessToken = localStorage.getItem('auth_token');
    let jwtHelper = new JwtHelper();
    let decodedIdToken = <IdToken>jwtHelper.decodeToken(accessToken);
    return decodedIdToken.roles;
  }

  getPermissions() {
    let accessToken = localStorage.getItem('auth_token');
    let jwtHelper = new JwtHelper();
    let decodedIdToken = <IdToken>jwtHelper.decodeToken(accessToken);
    let permissions: PermissionValues[] = Array.isArray(decodedIdToken.permission) ? decodedIdToken.permission : [decodedIdToken.permission];
    return permissions;
  }

  get isAdmin() {
    let roleName = this.getRoles().toString().toLowerCase();
    return (roleName == Roles.ADMIN) ? true : false;
  }

  get isUser() {
    let roleName = this.getRoles().toString().toLowerCase();
    return (roleName == Roles.USER) ? true : false;
  }

  userHasPermission(permissionValue: PermissionValues): boolean {
    return this.getPermissions().some(p => p == permissionValue);
  }
}
