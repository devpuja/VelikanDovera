import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthGuard } from '../auth.guard';
import { UserService } from '../shared/services/user.service';
import { Permission } from '../shared/models/permission.model';
import { AdminAuthGuard } from '../admin.auth.guard';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  //providers: [AuthGuard, AdminAuthGuard]
})
export class HomeComponent implements OnInit {
  isLinear = false;
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  thirdFormGroup: FormGroup;
  fourthFormGroup: FormGroup;

  constructor(private _formBuilder: FormBuilder, private userService: UserService) {
    
  }

  ngOnInit() {
    this.firstFormGroup = this._formBuilder.group({
      firstCtrl: ['', Validators.required]
    });
    this.secondFormGroup = this._formBuilder.group({
      secondCtrl: ['', Validators.required]
    });
    this.thirdFormGroup = this._formBuilder.group({
      thirdCtrl: ['', Validators.required]
    });
  }

  get canUserAdd() {
    return this.userService.userHasPermission(Permission.UsersAddPermission);
  }

  get canUserEdit() {
    return this.userService.userHasPermission(Permission.UsersEditPermission);
  }

}
