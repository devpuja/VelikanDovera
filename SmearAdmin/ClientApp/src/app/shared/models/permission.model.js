"use strict";
// ======================================
// Author: Ebenezer Monney
// Email:  info@ebenmonney.com
// Copyright (c) 2017 www.ebenmonney.com
// 
// ==> Gun4Hire: contact@ebenmonney.com
// ======================================
Object.defineProperty(exports, "__esModule", { value: true });
var Permission = /** @class */ (function () {
    function Permission(name, value, groupName, description) {
        this.name = name;
        this.value = value;
        this.groupName = groupName;
        this.description = description;
    }
    Permission.UsersAddPermission = "permissions.users.add";
    Permission.UsersViewPermission = "permissions.users.view";
    Permission.UsersEditPermission = "permissions.users.edit";
    Permission.AdminPermission = "permissions.admins.CRUD";
    return Permission;
}());
exports.Permission = Permission;
var Roles = /** @class */ (function () {
    function Roles() {
    }
    Roles.ADMIN = "administrator";
    Roles.USER = "user";
    return Roles;
}());
exports.Roles = Roles;
var ReferenceTableNames = /** @class */ (function () {
    function ReferenceTableNames() {
    }
    ReferenceTableNames.EMPLOYEE = "aspnetusers";
    ReferenceTableNames.DOCTOR = "doctor";
    ReferenceTableNames.CHEMIST = "chemist";
    ReferenceTableNames.STOCKIST = "stockist";
    ReferenceTableNames.PATIENT = "patient";
    return ReferenceTableNames;
}());
exports.ReferenceTableNames = ReferenceTableNames;
var PresentType = /** @class */ (function () {
    function PresentType() {
    }
    PresentType.FULLDAY = "Full-Day";
    PresentType.HALFDAY = "Half-Day";
    PresentType.LEAVE = "Leave";
    return PresentType;
}());
exports.PresentType = PresentType;
var EmployeeExpensesStatus = /** @class */ (function () {
    function EmployeeExpensesStatus() {
    }
    EmployeeExpensesStatus.NotSubmitted = 0;
    EmployeeExpensesStatus.Submitted = 1;
    EmployeeExpensesStatus.Approved = 2;
    EmployeeExpensesStatus.Rejected = 3;
    EmployeeExpensesStatus.NotFilledName = "Not Filled";
    EmployeeExpensesStatus.NotSubmittedName = "Not Submitted";
    EmployeeExpensesStatus.SubmittedName = "Submitted";
    EmployeeExpensesStatus.ApprovedName = "Approved";
    EmployeeExpensesStatus.RejectedName = "Rejected";
    EmployeeExpensesStatus.NotSubmittedColor = "orange";
    EmployeeExpensesStatus.SubmittedColor = "blue";
    EmployeeExpensesStatus.ApprovedColor = "green";
    EmployeeExpensesStatus.RejectedColor = "red";
    return EmployeeExpensesStatus;
}());
exports.EmployeeExpensesStatus = EmployeeExpensesStatus;
//# sourceMappingURL=permission.model.js.map