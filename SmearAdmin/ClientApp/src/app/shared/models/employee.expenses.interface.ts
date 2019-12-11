export interface EmployeeExpenses {
  ID: number;
  UserName: string;
  PresentType: string;
  ExpenseMonth: string;
  Date: string;
  HQ: number;
  HQName: string;
  Region: string;
  BikeAllowance: number;
  DailyAllowance: number;
  OtherAmount: number;
  ClaimAmount: number;
  EmployeeRemark: string;
  ApprovedAmount: number;
  ApprovedBy: string;
  ApproverRemark: string;
  Status: number;
}

export interface EmployeeExpensesStatus {
  ID: number;
  UserName: string;
  ExpenseMonth: string;
  Status: number;
  FullName: string;
}
