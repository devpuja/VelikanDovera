// auth.guard.ts
import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
//import { map, take } from 'rxjs/operators';
import { UserService } from '../app/shared/services/user.service';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private userService: UserService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.userService.isLoggedIn() && this.userService.isUser) {
      return true;
    }

    //this.userService.redirectUrl = state.url;
    this.userService.logout();
    this.router.navigate(['./denied']);
    //this.router.navigate(['./login']);
    return false;
  }
}


@Injectable()
export class AuthCommanGuard implements CanActivate {
  constructor(private userService: UserService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.userService.isLoggedIn()) {
      return true;
    }

    //this.userService.redirectUrl = state.url;
    this.userService.logout();
    this.router.navigate(['./denied']);
    //this.router.navigate(['./login']);
    return false;
  }
}
