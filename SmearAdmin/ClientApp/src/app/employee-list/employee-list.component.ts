import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatDialog } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { EmployeeService } from '../shared/services/employee.service';
import { tap } from 'rxjs/operators';
import { UserRegistration } from '../shared/models/user.registration.interface';
import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { Observable } from 'rxjs';
import { ConfirmDialogComponent } from '../shared/dialog/confirm-dialog.component';
import { debug } from 'util';
import { Router } from '@angular/router';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css'],  
})
export class EmployeeListComponent implements OnInit {
  showSpinner: boolean = false;

  // Pagination
  @ViewChild(MatPaginator) paginator: MatPaginator;
  dataSource: EmployeeDataSource;
  displayedColumns = ['Img', 'FirstName', 'CustomUserName', 'HQ', 'DOJ', 'Mobile', 'Role', 'Claims', 'Password', 'IsEnabled', 'action'];
  totalItemsCount: number;
  pageSizeOptions: number[] = [5, 10, 25];

  constructor(public dialog: MatDialog, private empService: EmployeeService, private toastr: ToastrService, private router: Router) {
  }

  ngOnInit() {
    this.getValues();
  }

  ngAfterViewInit() {
    this.paginator.page.pipe(tap(() => this.loadPages())).subscribe();
  }

  loadPages() {
    this.loadData(this.paginator.pageIndex, this.paginator.pageSize);
  }

  getValues() {
    this.loadData(0,5);
  }

  loadData(pgIndex: number, pgSize: number) {
    this.showSpinner = true;
    this.empService.getAllEmployees(pgIndex, pgSize)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            //console.log(result.Items);
            this.dataSource = new EmployeeDataSource(result.Items);
            this.totalItemsCount = result.TotalCount;
            this.toastr.success("Records Loaded", "Success");
          }
        },
        error => this.toastr.error(error, "Error"));
  }

  onDelete(usrReg: UserRegistration, action: string) {
    this.showSpinner = true;
    this.empService.deleteEmployee(usrReg, action)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          //console.log(result);
          if (result) {
            this.getValues();
            this.toastr.success("Record Deleted", "Success");
          }
        },
        error => this.toastr.error(error, "Error"));
  }

  onEdit(usrReg: UserRegistration): void {
    //https://github.com/angular/angular/issues/11379
    //https://alligator.io/angular/navigation-routerlink-navigate-navigatebyurl/
    //https://alligator.io/angular/query-parameters/

    var usrId = usrReg.ID.toString();
    this.router.navigate(['/employee'], { queryParams: { id: usrId } });
  }

  openDialogDelete(usrReg: UserRegistration, action: string): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.onDelete(usrReg, action);
      }
      else {
        this.toastr.show("Delete operation cancelled.");
      }
    });
  }

}

export class EmployeeDataSource extends DataSource<UserRegistration> {
  constructor(private data: UserRegistration[]) {
    super();
  }

  connect(collectionViewer: CollectionViewer): Observable<UserRegistration[]> {
    return Observable.of(this.data);
  }

  disconnect(collectionViewer: CollectionViewer): void {
  }
}
