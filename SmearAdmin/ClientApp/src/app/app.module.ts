import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA  } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { Http, HttpModule, XHRBackend } from '@angular/http';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ContactResourceComponent } from './shared/resources/contact-resource/contact-resource.component';
import { MaterialModule } from './shared/material.module';
import { LoginComponent } from './login/login.component';
import { AuthenticateXHRBackend } from './authenticate-xhr.backend';
import { AuthGuard, AuthCommanGuard } from '../app/auth.guard';
import { AdminAuthGuard } from '../app/admin.auth.guard';
import { ConfigService } from './shared/utils/config.service';
import { RegistrationComponent } from './registration/registration.component';
import { MastersDoctorEmployeeComponent } from './masters-doctor-employee/masters-doctor-employee.component';
import { MastersDoctorEmployeeDialogComponent } from './masters-doctor-employee/masters-doctor-employee-dialog/masters-doctor-employee-dialog.component';
import { UserService } from './shared/services/user.service';
import { SpinnerComponent } from './spinner/spinner.component';
import { ToastrModule } from 'ngx-toastr';
import { ConfirmDialogComponent } from './shared/dialog/confirm-dialog.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { EmployeeDashboardComponent } from './employee-dashboard/employee-dashboard.component';
import { AccessDeniedComponent } from './access-denied/access-denied.component';
import { EmployeeComponent } from './employee/employee.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { HolidayComponent } from './holiday/holiday.component';
import { EmployeeExpensesComponent } from './employee-expenses/employee-expenses.component';
import { EmployeeExpensesListComponent } from './employee-expenses-list/employee-expenses-list.component';
import { ManageEmployeeExpensesListComponent } from './manage-employee-expenses-list/manage-employee-expenses-list.component';
import { DoctorComponent } from './doctor/doctor.component';
import { ManageDoctorComponent } from './manage-doctor/manage-doctor.component';
import { ChemistComponent } from './chemist/chemist.component';
import { ManageChemistComponent } from './manage-chemist/manage-chemist.component';
import { StockistComponent } from './stockist/stockist.component';
import { ManageStockistComponent } from './manage-stockist/manage-stockist.component';
import { PatientComponent } from './patient/patient.component';
import { ManagePatientComponent } from './manage-patient/manage-patient.component';
import { ChemistStockistResourceComponent } from './shared/resources/chemist-stockist-resource/chemist-stockist-resource.component';
import { ManageHolidayComponent } from './manage-holiday/manage-holiday.component';
import { SwapEmployeeComponent } from './swap-employee/swap-employee.component';
import { SendSmsComponent } from './send-sms/send-sms.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LoginComponent,
    RegistrationComponent,
    MastersDoctorEmployeeComponent,
    SpinnerComponent,
    MastersDoctorEmployeeDialogComponent,
    ConfirmDialogComponent,
    AdminDashboardComponent,
    EmployeeDashboardComponent,
    AccessDeniedComponent,
    EmployeeComponent,
    EmployeeListComponent,
    ContactResourceComponent,
    ChemistStockistResourceComponent,
    HolidayComponent,
    EmployeeExpensesComponent,
    EmployeeExpensesListComponent,
    ManageEmployeeExpensesListComponent,
    DoctorComponent,
    ManageDoctorComponent,
    ChemistComponent,
    ManageChemistComponent,
    StockistComponent,
    ManageStockistComponent,
    PatientComponent,
    ManagePatientComponent,
    ManageHolidayComponent,
    SwapEmployeeComponent,
    SendSmsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 5000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
      progressBar: false,
    }),
    RouterModule.forRoot([
      { path: '', component: LoginComponent, pathMatch: 'full' },
      { path: 'home', component: HomeComponent, canActivate: [AuthCommanGuard] },
      { path: 'counter', component: CounterComponent, canActivate: [AuthCommanGuard] },
      { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthCommanGuard] },
      { path: 'login', component: LoginComponent },
      { path: 'denied', component: AccessDeniedComponent },
      { path: 'masters', component: MastersDoctorEmployeeComponent, canActivate: [AuthCommanGuard] },
      { path: 'holiday', component: HolidayComponent, canActivate: [AuthCommanGuard] },
      { path: 'manage-holiday', component: ManageHolidayComponent, canActivate: [AuthCommanGuard] },
      { path: 'admin-dashboard', component: AdminDashboardComponent, canActivate: [AdminAuthGuard] },
      { path: 'employee-dashboard', component: EmployeeDashboardComponent, canActivate: [AuthGuard] },
      { path: 'employee', component: EmployeeComponent, canActivate: [AdminAuthGuard] },
      { path: 'employee/:id', component: EmployeeComponent, canActivate: [AdminAuthGuard] },
      { path: 'employee-list', component: EmployeeListComponent, canActivate: [AdminAuthGuard] },
      { path: 'employee-expenses', component: EmployeeExpensesComponent, canActivate: [AuthGuard] },
      { path: 'employee-expenses/:id', component: EmployeeExpensesComponent, canActivate: [AuthGuard] },
      { path: 'employee-expenses-list', component: EmployeeExpensesListComponent, canActivate: [AuthGuard] },
      { path: 'manage-employee-expenses-list', component: ManageEmployeeExpensesListComponent, canActivate: [AdminAuthGuard] },
      { path: 'manage-employee-expenses-list/:user/:month', component: ManageEmployeeExpensesListComponent, canActivate: [AdminAuthGuard] },
      { path: 'doctor', component: DoctorComponent, canActivate: [AuthCommanGuard] },
      { path: 'manage-doctor', component: ManageDoctorComponent, canActivate: [AuthCommanGuard] },
      { path: 'chemist', component: ChemistComponent, canActivate: [AuthCommanGuard] },
      { path: 'manage-chemist', component: ManageChemistComponent, canActivate: [AuthCommanGuard] },
      { path: 'stockist', component: StockistComponent, canActivate: [AuthCommanGuard] },
      { path: 'manage-stockist', component: ManageStockistComponent, canActivate: [AuthCommanGuard] },
      { path: 'patient', component: PatientComponent, canActivate: [AuthCommanGuard] },
      { path: 'manage-patient', component: ManagePatientComponent, canActivate: [AuthCommanGuard] },
      { path: 'swap-employee', component: SwapEmployeeComponent, canActivate: [AdminAuthGuard] },
      { path: 'send-sms', component: SendSmsComponent, canActivate: [AdminAuthGuard] },
      { path: '**', redirectTo: 'login'}
    ], { useHash: true }),
  ],
  providers: [ConfigService, AuthGuard, AdminAuthGuard, AuthCommanGuard, UserService, 
    {
    provide: XHRBackend,
    useClass: AuthenticateXHRBackend
    }
  ],
  entryComponents: [
    MastersDoctorEmployeeDialogComponent,
    ConfirmDialogComponent
  ],
  bootstrap: [AppComponent],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],
})
export class AppModule { }


//References
/*
//toastr: https://github.com/CodeSeven/toastr
//ngx-toastr: https://scttcper.github.io/ngx-toastr/
//ngx-toastr: https://www.npmjs.com/package/ngx-toastr
//ng-snotify : https://www.npmjs.com/package/ng-snotify

//Paging https://www.youtube.com/watch?v=ao-nY-9biWs / https://yout.com/watch?v=ao-nY-9biWs
//IMplement HttpInterceptor in this APPLICATION ?
//set DataSource mat-paginator length angular 6
//https://patient.info/forums/discuss/gastritis-timeline-to-cure-273833?page=1
//https://stackoverflow.com/questions/47871840/angular-material-2-table-server-side-pagination#

////http://jasonwatmore.com/post/2018/05/23/angular-6-jwt-authentication-example-tutorial#authentication-service-ts
//https://www.concretepage.com/angular/angular-httpclient-post

Angular 5 Nested Components: https://www.youtube.com/watch?v=JGjN7gNCSoE

 //https://blog.angularindepth.com/insiders-guide-into-interceptors-and-httpclient-mechanics-in-angular-103fbdb397bf
    //https://medium.com/@ryanchenkie_40935/angular-authentication-using-the-http-client-and-http-interceptors-2f9d1540eb8
    //https://www.concretepage.com/angular/angular-httpclient-post
    //https://www.tektutorialshub.com/how-to-pass-url-parameters-query-strings-angular/
    //https://www.youtube.com/watch?v=suTtA0Hlwlk

Repository pattern: https://www.youtube.com/watch?v=rtXpYpZdOzM
http://bengtbe.com/blog/2009/04/14/using-automapper-to-map-view-models-in-asp-net-mvc/

//https://www.academind.com/learn/angular/snippets/angular-image-upload-made-easy/

*/
