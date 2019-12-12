import { MediaMatcher } from '@angular/cdk/layout';
import { Component, ChangeDetectorRef, OnInit, OnDestroy, HostListener } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { UserService } from './shared/services/user.service';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    providers: [UserService]
})
export class AppComponent implements OnInit, OnDestroy {
    title = 'app';

    mobileQuery: MediaQueryList;
    status: boolean;
    subscription: Subscription;

    private _mobileQueryListener: () => void;

    constructor(changeDetectorRef: ChangeDetectorRef, media: MediaMatcher, private userService: UserService, private router: Router) {
        this.mobileQuery = media.matchMedia('(max-width: 600px)');

        this._mobileQueryListener = () => changeDetectorRef.detectChanges();
        this.mobileQuery.addListener(this._mobileQueryListener);
    }

    ngOnDestroy(): void {
        this.mobileQuery.removeListener(this._mobileQueryListener);
        this.subscription.unsubscribe();
    }

    onLogout() {
        this.userService.logout();
    }

    ngOnInit() {
        this.subscription = this.userService.authNavStatus$.subscribe(status => this.status = status);
    }

    @HostListener('window:unload', ['$event'])
    unloadHandler(event) {
        return true;
        //this.onLogout();
    }

    //@HostListener('window:beforeunload', ['$event'])
    //beforeUnloadHander(event) {
    //  return false;
    //}

}
