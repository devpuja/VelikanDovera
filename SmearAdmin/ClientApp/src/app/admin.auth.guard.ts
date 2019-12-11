// admin.auth.guard.ts
import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
//import { map, take } from 'rxjs/operators';
import { UserService } from '../app/shared/services/user.service';

@Injectable()
export class AdminAuthGuard implements CanActivate {
  constructor(private userService: UserService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.userService.isLoggedIn() && this.userService.isAdmin) {
      return true;
    }

    //this.userService.redirectUrl = state.url;
    this.userService.logout();
    this.router.navigate(['./denied']);
    //this.router.navigate(['./login']);
    return false;
  }


  //canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
  //  return this.user.isLoggedIn         // {1}
  //    .pipe(
  //      take(1),                              // {2} 
  //      map((isLoggedIn: boolean) => {         // {3}
  //        if (!isLoggedIn) {
  //          this.router.navigate(['/login']);  // {4}
  //          return false;
  //        }
  //        return true;
  //      }));
  //}

}
