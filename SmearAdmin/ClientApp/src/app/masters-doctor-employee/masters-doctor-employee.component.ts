import { Component, OnInit, Inject, ViewChild, AfterViewInit, Input, EventEmitter, Output } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, MatPaginator, MatTableDataSource, PageEvent } from '@angular/material';
//import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MastersDoctorEmployeeDialogComponent } from './masters-doctor-employee-dialog/masters-doctor-employee-dialog.component';
import { Masters, MastersGroup } from '../shared/models/masters.interface';
import { MastersService } from "../shared/services/masters.service";
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, Observable } from 'rxjs';
import { DataSource } from '@angular/cdk/table';
import { CollectionViewer } from '@angular/cdk/collections';
import { tap } from 'rxjs/operators';
import { ConfirmDialogComponent } from '../shared/dialog/confirm-dialog.component';
import { AuthGuard } from '../auth.guard';
import { AdminAuthGuard } from '../admin.auth.guard';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-masters-doctor-employee',
  templateUrl: './masters-doctor-employee.component.html',
  styleUrls: ['./masters-doctor-employee.component.css'],
  //providers: [AuthGuard, AdminAuthGuard]
})
export class MastersDoctorEmployeeComponent implements OnInit, AfterViewInit {
  showSpinner: boolean = false;
  masters: Masters = { Id: 0, Type: '', Value: '' };
  selectedMaster: string;

  // Pagination
  @ViewChild(MatPaginator) paginator: MatPaginator;
  dataSource: MastersDataSource;
  displayedColumns: string[] = ['Id', 'Type', 'Value', 'action'];
  totalItemsCount: number;
  pageSizeOptions: number[] = [5, 10, 25];

  mastersControl = new FormControl();
  mastersGroups: MastersGroup[] = [
    {
      name: 'Doctor',
      mastersFor: [
        { value: 'Brand', viewValue: 'Brand' },
        { value: 'Class', viewValue: 'Class' },
        { value: 'Community', viewValue: 'Community' },
        { value: 'Qualification', viewValue: 'Qualification' },
        { value: 'Speciality', viewValue: 'Speciality' },
        { value: 'BestDayToMeet', viewValue: 'BestDayToMeet' },
        { value: 'BestTimeToMeet', viewValue: 'BestTimeToMeet' },
        { value: 'VisitFrequency', viewValue: 'VisitFrequency' },
      ]
    },
    {
      name: 'Employee',
      //disabled: true,
      mastersFor: [
        { value: 'Desigination', viewValue: 'Desigination' },
        { value: 'Department', viewValue: 'Department' },
        { value: 'Grade', viewValue: 'Grade' },
        { value: 'HQ', viewValue: 'HQ' },
        { value: 'Region', viewValue: 'Region' },        
      ]
    },
    {
      name: 'Allowance',
      mastersFor: [
        { value: 'Bike', viewValue: 'Bike' },
        { value: 'Cyber', viewValue: 'Cyber' },
        { value: 'Daily', viewValue: 'Daily' },
        { value: 'Fare', viewValue: 'Fare' },
        { value: 'Mobile', viewValue: 'Mobile' },        
        { value: 'Stationery', viewValue: 'Stationery' },
        { value: 'Present Type', viewValue: 'Present Type' },
      ]
    },
    {
      name: 'Miscellaneous',
      mastersFor: [
        { value: 'Gender', viewValue: 'Gender' },
      ]
    }
  ];

  constructor(public dialog: MatDialog, private mstService: MastersService, private toastr: ToastrService) {
    this.selectedMaster = "Class";    
  }

  ngOnInit() {
    this.getValues();
  }

  ngAfterViewInit() {
    this.paginator.page.pipe(tap(() => this.loadPages())).subscribe();
  }

  loadPages() {
    this.loadData(this.selectedMaster, this.paginator.pageIndex, this.paginator.pageSize);
  }

  getValues() {
    //this.loadData(this.selectedMaster, this.pgeIndex, this.pgeSize);
    this.loadData(this.selectedMaster, 0, 5);
  }

  loadData(selectedMaster: string, pgIndex: number, pgSize: number) {
    this.showSpinner = true;
    this.mstService.getAllMasters(selectedMaster, pgIndex, pgSize)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            this.dataSource = new MastersDataSource(result.Items);
            this.totalItemsCount = result.TotalCount;
            this.toastr.success("Records Loaded", "Success");
          }
        },
        error => this.toastr.error(error, "Error"));
  }

  onDelete(masters: Masters) {
    this.showSpinner = true;
    this.mstService.deleteMasters(masters.Id)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          console.log(result);
          if (result) {
            this.getValues();
            this.toastr.success("Record Deleted", "Success");
          }
        },
        error => this.toastr.error(error, "Error"));
  }

  onEdit(masters: Masters) {
    this.showSpinner = true;
    this.mstService.getMastersByID(masters.Id)
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            this.openDialogEdit(result);
          }
        },
        error => this.toastr.error(error, "Error"));
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(MastersDoctorEmployeeDialogComponent, {
      width: '300px',
      //data: { type: this.type, value: this.value }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getValues();
    });
  }

  openDialogEdit(masters: Masters): void {
    const dialogRef = this.dialog.open(MastersDoctorEmployeeDialogComponent, {
      width: '300px',
      //data: { id: masters.id, type: masters.type, value: masters.value }
      data: masters
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getValues();
    });
  }

  openDialogDelete(masters: Masters): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.onDelete(masters);
      }
      else {
        this.toastr.show("Delete operation cancelled.");
      }
    });
  }
}

export class MastersDataSource extends DataSource<Masters> {
  //private dataMasters = new BehaviorSubject<Masters[]>([]);
  constructor(private data: Masters[]) {
    super();
  }

  connect(collectionViewer: CollectionViewer): Observable<Masters[]> {
    return Observable.of(this.data);
    //return this.dataMasters.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
    //this.dataMasters.complete();
  }
}
