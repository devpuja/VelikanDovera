import { Component, OnInit, Input } from '@angular/core';
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
  @Input() function: any;

 
  public searchForm: FormGroup;
  EmployeeList: any;

  //Model For Forms
  userNameData: any = {
    UserName: ''
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
    alert('filterByEmployee');
    this.function();
  }

  filterByNames() {
    alert('filterByNames');
  }
}
