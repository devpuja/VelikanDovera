import { Component, OnInit, ViewChild } from '@angular/core';
import { HolidayService } from '../shared/services/holiday.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { MatDialog, MatPaginator } from '@angular/material';
import { Holiday } from '../shared/models/holiday.interface';
import { ConfirmDialogComponent } from '../shared/dialog/confirm-dialog.component';
import { DataSource } from '@angular/cdk/table';
import { Observable } from 'rxjs';
import { CollectionViewer } from '@angular/cdk/collections';
import { tap } from 'rxjs/operators';

@Component({
    selector: 'app-manage-holiday',
    templateUrl: './manage-holiday.component.html',
    styleUrls: ['./manage-holiday.component.css']
})
export class ManageHolidayComponent implements OnInit {
    showSpinner: boolean = false;
    // Pagination
    @ViewChild(MatPaginator) paginator: MatPaginator;
    dataSource: HolidayDataSource;
    displayedColumns = ['FestivalName', 'FestivalDate', 'FestivalDescription', 'IsNationalFestival', 'BelongToCommunity', 'action'];
    totalItemsCount: number;
    pageSizeOptions: number[] = [5, 10, 25];

    constructor(public dialog: MatDialog, private holidayService: HolidayService, private toastr: ToastrService, private router: Router) {
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
        this.holidayService.getAllHolidays(pgIndex, pgSize)
            .finally(() => this.showSpinner = false)
            .subscribe(
                result => {
                    if (result) {
                        console.log(result.Items);
                        this.dataSource = new HolidayDataSource(result.Items);
                        this.totalItemsCount = result.TotalCount;
                        this.toastr.success("Records Loaded", "Success");
                    }
                },
                errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
    }

    onEdit(holiday: Holiday): void {
        //https://github.com/angular/angular/issues/11379
        //https://alligator.io/angular/navigation-routerlink-navigate-navigatebyurl/
        //https://alligator.io/angular/query-parameters/

        var holidayId = holiday.ID.toString();
        this.router.navigate(['/holiday'], { queryParams: { id: holidayId } });
    }

    openDialogDelete(holiday: Holiday): void {
        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
            width: '250px'
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.onDelete(holiday);
            }
            else {
                this.toastr.show("Delete operation cancelled.");
            }
        });
    }

    onDelete(holiday: Holiday) {
        this.showSpinner = true;
        this.holidayService.deleteHoliday(holiday.ID)
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
}


export class HolidayDataSource extends DataSource<Holiday> {
    constructor(private data: Holiday[]) {
        super();
    }

    connect(collectionViewer: CollectionViewer): Observable<Holiday[]> {
        return Observable.of(this.data);
    }

    disconnect(collectionViewer: CollectionViewer): void {
    }
}
