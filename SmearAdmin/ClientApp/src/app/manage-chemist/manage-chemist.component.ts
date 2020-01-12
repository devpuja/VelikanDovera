import { Component, OnInit, ViewChild } from '@angular/core';
import { Doctor } from '../shared/models/doctor.interface';
import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { Observable } from 'rxjs';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { MatPaginator, MatDialog } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { ConfirmDialogComponent } from '../shared/dialog/confirm-dialog.component';
import { Chemist } from '../shared/models/chemist.interface';
import { ChemistService } from '../shared/services/chemist.service';
import { ConfigService } from '../shared/utils/config.service';
import { UserService } from '../shared/services/user.service';

@Component({
  selector: 'app-manage-chemist',
  templateUrl: './manage-chemist.component.html',
  styleUrls: ['./manage-chemist.component.css'],
  animations: [trigger('detailExpand', [
    state('collapsed', style({ height: '0px', minHeight: '0', display: 'none' })),
    state('expanded', style({ height: '*' })),
    transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
  ]),
  ]
})
export class ManageChemistComponent implements OnInit {
  showSpinner: boolean = false;
  baseUrl: string = '';
  IsAdmin: boolean = true;

  // Pagination
  @ViewChild(MatPaginator) paginator: MatPaginator;
  dataSource: ChemistDataSource;
  displayedColumns = ['MedicalName', 'DrugLicenseNo', 'FoodLicenseNo', 'GSTNo', 'BestDayToMeet', 'Area', 'Class', 'IsActive', 'action'];
  totalItemsCount: number;
  pageSizeOptions: number[] = [5, 10, 25];

  constructor(public dialog: MatDialog, private chemService: ChemistService, private toastr: ToastrService, private router: Router, private configService: ConfigService, private userService: UserService) {
    this.baseUrl = this.configService.getApiURI();
    this.IsAdmin = this.userService.isAdmin;
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
    this.loadData(0, 5);
  }

  loadData(pgIndex: number, pgSize: number) {
    this.showSpinner = true;
    this.chemService.getAllChemists(pgIndex, pgSize)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            console.log(result.Items);
            this.dataSource = new ChemistDataSource(result.Items);
            this.totalItemsCount = result.TotalCount;
            this.toastr.success("Records Loaded", "Success");
          }
        },
        errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
  }

  onEdit(chem: Chemist): void {
    //https://github.com/angular/angular/issues/11379
    //https://alligator.io/angular/navigation-routerlink-navigate-navigatebyurl/
    //https://alligator.io/angular/query-parameters/

    var chemId = chem.ID.toString();
    this.router.navigate(['/chemist'], { queryParams: { id: chemId } });
  }

  openDialogDelete(chem: Chemist, action: string): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.onDelete(chem, action);
      }
      else {
        this.toastr.show("Delete operation cancelled.");
      }
    });
  }

  onDelete(chem: Chemist, action: string) {
    this.showSpinner = true;
    this.chemService.deleteChemist(chem, action)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          //console.log(result);
          if (result) {
            this.getValues();
            this.toastr.success("Record Deleted", "Success");
          }
        },
        error => { this.toastr.error(error.message, "Error"); this.toastr.error(error.error.message, "Error"); });
  }

  exportExcel() {
    this.showSpinner = true;
    this.chemService.exportChemistData()
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

export class ChemistDataSource extends DataSource<Chemist> {
  constructor(private data: Chemist[]) {
    super();
  }

  connect(collectionViewer: CollectionViewer): Observable<Chemist[]> {
    return Observable.of(this.data);
  }

  disconnect(collectionViewer: CollectionViewer): void {
  }
}
