// ======================================
// Author: Ebenezer Monney
// Email:  info@ebenmonney.com
// Copyright (c) 2017 www.ebenmonney.com
// 
// ==> Gun4Hire: contact@ebenmonney.com
// ======================================

//export type PermissionNames =
//    "View Users" | "Manage Users" |
//    "View Roles" | "Manage Roles" | "Assign Roles";

//export type PermissionValues =
//    "users.view" | "users.manage" |
//    "roles.view" | "roles.manage" | "roles.assign";

//export class Permission {

//    public static readonly viewUsersPermission: PermissionValues = "users.view";
//    public static readonly manageUsersPermission: PermissionValues = "users.manage";

//    public static readonly viewRolesPermission: PermissionValues = "roles.view";
//    public static readonly manageRolesPermission: PermissionValues = "roles.manage";
//    public static readonly assignRolesPermission: PermissionValues = "roles.assign";


//    constructor(name?: PermissionNames, value?: PermissionValues, groupName?: string, description?: string) {
//        this.name = name;
//        this.value = value;
//        this.groupName = groupName;
//        this.description = description;
//    }

//    public name: PermissionNames;
//    public value: PermissionValues;
//    public groupName: string;
//    public description: string;
//}

export type PermissionNames =
  "Users Add" | "Users View" | "Users Edit" |
  "Admin CRUD"

export type PermissionValues =
  "permissions.users.add" | "permissions.users.view" | "permissions.users.edit" |
  "permissions.admins.CRUD"

export class Permission {

  public static readonly UsersAddPermission: PermissionValues = "permissions.users.add";
  public static readonly UsersViewPermission: PermissionValues = "permissions.users.view";
  public static readonly UsersEditPermission: PermissionValues = "permissions.users.edit";

  public static readonly AdminPermission: PermissionValues = "permissions.admins.CRUD";
  
  constructor(name?: PermissionNames, value?: PermissionValues, groupName?: string, description?: string) {
    this.name = name;
    this.value = value;
    this.groupName = groupName;
    this.description = description;
  }

  public name: PermissionNames;
  public value: PermissionValues;
  public groupName: string;
  public description: string;
}

export class Roles {
  public static readonly ADMIN = "administrator";
  public static readonly USER = "user";
}

export class ReferenceTableNames {
  public static readonly EMPLOYEE = "aspnetusers";
  public static readonly DOCTOR = "doctor";
  public static readonly CHEMIST = "chemist";
  public static readonly STOCKIST = "stockist";
  public static readonly PATIENT = "patient";
}

export class PresentType {
  public static readonly FULLDAY = "Full-Day";
  public static readonly HALFDAY = "Half-Day";
  public static readonly LEAVE = "Leave";
}

export class ConstEmployeeExpensesStatus {
  public static readonly NotSubmitted = 0;
  public static readonly Submitted = 1;
  public static readonly Approved = 2;
  public static readonly Rejected = 3;

  public static readonly NotFilledName = "Not Filled";
  public static readonly NotSubmittedName = "Not Submitted";
  public static readonly SubmittedName = "Submitted";
  public static readonly ApprovedName = "Approved";
  public static readonly RejectedName = "Rejected";

  public static readonly NotSubmittedColor = "orange";
  public static readonly SubmittedColor = "blue";
  public static readonly ApprovedColor = "green";
  public static readonly RejectedColor = "red";
}
