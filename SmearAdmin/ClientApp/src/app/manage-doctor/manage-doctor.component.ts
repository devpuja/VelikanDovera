import { Component, OnInit, ViewChild } from '@angular/core';
import { Doctor } from '../shared/models/doctor.interface';
import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { Observable } from 'rxjs';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { MatPaginator, MatDialog } from '@angular/material';
import { DoctorService } from '../shared/services/doctor.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { ConfirmDialogComponent } from '../shared/dialog/confirm-dialog.component';
import { ConfigService } from '../shared/utils/config.service';
import { UserService } from '../shared/services/user.service';

@Component({
  selector: 'app-manage-doctor',
  templateUrl: './manage-doctor.component.html',
  styleUrls: ['./manage-doctor.component.css'],
  animations: [trigger('detailExpand', [
    state('collapsed', style({ height: '0px', minHeight: '0', display: 'none' })),
    state('expanded', style({ height: '*' })),
    transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
  ]),
  ]
})

export class ManageDoctorComponent implements OnInit {
  showSpinner: boolean = false;
  baseUrl: string = '';
  IsAdmin: boolean = true;
  IsFilterValue: string = "";
  IsSearchValue: string = "";

  // Pagination
  @ViewChild(MatPaginator) paginator: MatPaginator;
  dataSource: DoctorDataSource;
  displayedColumns = ['Name', 'Qualification', 'Speciality', 'VisitFrequency', 'BestDayToMeet', 'Brand', 'Class', 'IsActive', 'action'];
  totalItemsCount: number;
  pageSizeOptions: number[] = [5, 10, 25];

  constructor(public dialog: MatDialog, private docService: DoctorService, private toastr: ToastrService, private router: Router, private configService: ConfigService, private userService: UserService) {
    this.baseUrl = this.configService.getApiURI();
    this.IsAdmin = this.userService.isAdmin;
  }

  ngOnInit() {
    if (this.IsFilterValue != "") {
      this.getValuesByUserName(this.IsFilterValue);
    } else if (this.IsSearchValue != "") {
      this.getValuesBySearch(this.IsSearchValue);
    }
    else {
      this.getValues();
    }
  }

  ngAfterViewInit() {
    this.paginator.page.pipe(tap(() => this.loadPages())).subscribe();
  }

  loadPages() {
    if (this.IsFilterValue != "") {
      this.loadDataByUserName(this.paginator.pageIndex, this.paginator.pageSize, this.IsFilterValue);
    } else if (this.IsSearchValue != "") {
      this.loadDataBySearch(this.paginator.pageIndex, this.paginator.pageSize, this.IsSearchValue);
    } else {
      this.loadData(this.paginator.pageIndex, this.paginator.pageSize);
    }
  }

  getValues() {
    this.loadData(0, 5);
  }

  loadData(pgIndex: number, pgSize: number) {
    this.showSpinner = true;
    this.docService.getAllDoctors(pgIndex, pgSize)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            //console.log(result.Items);
            this.dataSource = new DoctorDataSource(result.Items);
            this.totalItemsCount = result.TotalCount;
            this.toastr.success("Records Loaded", "Success");
          }
        },
        errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
  }

  //Filter by User
  getValuesByUserName($event: string) {
    this.IsFilterValue = $event;
    if (this.IsFilterValue != "") {
      this.loadDataByUserName(0, 5, this.IsFilterValue);
    }
    else {
      this.loadData(0, 5);
    }
  }

  loadDataByUserName(pgIndex: number, pgSize: number, userName: string) {
    this.showSpinner = true;
    this.docService.getAllDoctorsByUserName(pgIndex, pgSize, userName)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            this.dataSource = new DoctorDataSource(result.Items);
            this.totalItemsCount = result.TotalCount;
            this.toastr.success("Records Loaded", "Success");
          }
        },
        errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
  }
  //Filter by User
  
  //Search by User
  getValuesBySearch($event: string) {
    this.IsSearchValue = $event;
    if (this.IsSearchValue != "") {
      this.loadDataBySearch(0, 5, this.IsSearchValue);
    }
    else {
      this.loadData(0, 5);
    }
  }

  loadDataBySearch(pgIndex: number, pgSize: number, searchValue: string) {
    this.showSpinner = true;
    this.docService.getAllDoctorsBySearch(pgIndex, pgSize, searchValue)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            //console.log(result.Items);
            this.dataSource = new DoctorDataSource(result.Items);
            this.totalItemsCount = result.TotalCount;
            this.toastr.success("Records Loaded", "Success");
          }
        },
        errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
  }
  //Search by User

  //reset Filter
  resetFilter() {
    this.IsFilterValue = "";
    this.IsSearchValue = "";
    this.getValues();
  }
  //reset Filter

  onEdit(doc: Doctor): void {
    //https://github.com/angular/angular/issues/11379
    //https://alligator.io/angular/navigation-routerlink-navigate-navigatebyurl/
    //https://alligator.io/angular/query-parameters/

    var docId = doc.ID.toString();
    this.router.navigate(['/doctor'], { queryParams: { id: docId } });
  }

  openDialogDelete(doc: Doctor, action: string): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.onDelete(doc, action);
      }
      else {
        this.toastr.show("Delete operation cancelled.");
      }
    });
  }

  onDelete(doc: Doctor, action: string) {
    this.showSpinner = true;
    this.docService.deleteDoctor(doc, action)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          //console.log(result);
          if (result) {
            this.getValues();
            this.toastr.success("Record Deleted", "Success");
          }
        },
        error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error") });
  }

  exportExcel() {
    this.showSpinner = true;
    this.docService.exportDoctorsData()
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            var url = this.baseUrl.toString() + "downloads/" + result.FileName;
            const downloadLink = document.createElement("a");
            downloadLink.style.display = "none";
            document.body.appendChild(downloadLink);
            downloadLink.setAttribute("href", url);
            downloadLink.setAttribute("download", result.FileName); //downloads by browser
            downloadLink.setAttribute("target", "_blank");
            downloadLink.click();
            document.body.removeChild(downloadLink);

            this.toastr.success("Records Loaded", "Success");
          }
        },
        error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error") });
  }
}

export class DoctorDataSource extends DataSource<Doctor> {
  constructor(private data: Doctor[]) {
    super();
  }

  connect(collectionViewer: CollectionViewer): Observable<Doctor[]> {
    return Observable.of(this.data);
  }

  disconnect(collectionViewer: CollectionViewer): void {
  }
}
