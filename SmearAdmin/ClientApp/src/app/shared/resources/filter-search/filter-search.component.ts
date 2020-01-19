import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { SwapEmployeeService } from '../../services/swap-employee.service';

@Component({
  selector: 'app-filter-search',
  templateUrl: './filter-search.component.html',
  styleUrls: ['./filter-search.component.css']
})
export class FilterSearchComponent implements OnInit {
  showSpinner: boolean = false;
  isLinear = false;

  @Input('parentForm')
  @Output("parentFun") parentFun: EventEmitter<any> = new EventEmitter();
  @Output("parentFun2") parentFun2: EventEmitter<any> = new EventEmitter();
  @Output("parentFun3") parentFun3: EventEmitter<any> = new EventEmitter();
   
  public searchForm: FormGroup;
  EmployeeList: any;

  //Model For Forms
  userNameData: any = {
    UserName: '',
    DocChemSto:''
  };

  constructor(private _formBuilder: FormBuilder, private swapService: SwapEmployeeService, private toastr: ToastrService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.searchForm = this._formBuilder.group({
      "UserNameCtrl": ['', Validators.required],
      "DocChemStoCtrl": ['', Validators.required]
    });

    this.loadEmployeeUserNames();
  }

  get UserNameCtrl() { return this.searchForm.get('UserNameCtrl'); }
  get DocChemStoCtrl() { return this.searchForm.get('DocChemStoCtrl'); }

  loadEmployeeUserNames() {
    this.showSpinner = true;
    this.swapService.getEmployeeUserNamesList()
      .finally(() => this.showSpinner = false)
      .subscribe(
        result => {
          if (result) {
            this.EmployeeList = result;
          }
        },
        errors => { this.toastr.error(errors.message, "Error"); this.toastr.error(errors.error.message, "Error"); });
  }

  filterByEmployee() {
    this.userNameData.UserName = this.UserNameCtrl.value;    
    if (this.userNameData.UserName == undefined)
      this.userNameData.UserName = "";

    this.parentFun.emit(this.userNameData.UserName);
  }

  filterByNames() {
    this.userNameData.DocChemSto = this.DocChemStoCtrl.value;
    if (this.userNameData.DocChemSto == undefined)
      this.userNameData.DocChemSto = "";

    this.parentFun2.emit(this.userNameData.DocChemSto);
  }

  filterReset() {
    this.searchForm.reset();
    this.parentFun3.emit();
  }
}
