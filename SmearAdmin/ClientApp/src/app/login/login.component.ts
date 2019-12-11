import { Subscription } from 'rxjs';
//import { BehaviorSubject, Observable } from 'rxjs';
import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from '../shared/services/user.service';
import { Credentials } from '../shared/models/credentials.interface';
import { UserRegistration } from '../shared/models/user.registration.interface';
import { JwtHelper } from '../shared/utils/jwt-helper';
import { IdToken } from '../shared/models/login-response';
import { PermissionValues, Permission, Roles } from '../shared/models/permission.model';
import { LocalStorage } from '@ngx-pwa/local-storage';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [UserService]
})
export class LoginComponent implements OnInit, OnDestroy {
  private subscription: Subscription;

  showSpinner: boolean = false;
  errors: string;
  credentials: Credentials = { email: '', password: '' };

  constructor(private userService: UserService, private router: Router, protected localStorage: LocalStorage, private activatedRoute: ActivatedRoute,
    private toastr: ToastrService) { }

  ngOnInit() {
    // subscribe to router event
    //this.subscription = this.activatedRoute.queryParams.subscribe(
    //    (param: any) => {
    //        this.brandNew = param['brandNew'];
    //        this.credentials.email = param['email'];
    //    });
  }

  ngOnDestroy() {
    // prevent memory leak by unsubscribing
    //this.subscription.unsubscribe();
  }


  login({ value, valid }: { value: Credentials, valid: boolean }) {
    //https://loiane.com/2017/08/angular-hide-navbar-login-page/
    //https://stackblitz.com/edit/angular-login-hide-navbar-ngif

    this.showSpinner = true;
    this.errors = '';
    if (valid) {
      this.userService.login(value.email, value.password)
        .finally(() => this.showSpinner = false)
        .subscribe(
          result => {
            if (result) {
              this.getTokenValues();
              if (this.userService.isAdmin)
                this.router.navigate(['./admin-dashboard']);
              else
                this.router.navigate(['./employee-dashboard']);
            }
          },
          //error => this.errors = error);
        error => this.toastr.error(error));
        //error => console.log(error));
    }
  }

  //get Token Details
  public getTokenValues() {
    let accessToken = localStorage.getItem('auth_token');

    if (accessToken == null)
      throw new Error("Received accessToken was empty");

    //let idToken = auth_token.id_token;
    //let refreshToken = response.refresh_token || this.refreshToken;
    //let expiresIn = response.expires_in;
    //let tokenExpiryDate = new Date();
    //tokenExpiryDate.setSeconds(tokenExpiryDate.getSeconds() + expiresIn);
    //let accessTokenExpiry = tokenExpiryDate;

    //debugger;
    //let jwtHelper = new JwtHelper();
    //let decodedIdToken = <IdToken>jwtHelper.decodeToken(accessToken);
    //console.log(decodedIdToken);
    //let roleData = decodedIdToken.roles;
    //let permissions: PermissionValues[] = Array.isArray(decodedIdToken.permission) ? decodedIdToken.permission : [decodedIdToken.permission];

    //localStorage.setItem("username", decodedIdToken.username);
    //localStorage.setItem("fullname", decodedIdToken.fullname);
    //localStorage.setItem("email", decodedIdToken.email);
    //localStorage.setItem("roles", JSON.stringify(decodedIdToken.roles));
    //localStorage.setItem("permissions", JSON.stringify(permissions));
  }
}
