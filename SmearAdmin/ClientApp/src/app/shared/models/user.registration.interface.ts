import { ContactResource } from "./contact-resource.interface";
import { RoleClaims } from "./role-claims.interface";

export interface UserRegistration {
  ID: string;
  Email: string;
  FirstName: string;
  LastName: string;
  MiddleName: string;
  PasswordRaw: string;
  PictureUrl: string;
  CustomUserName: string;
  Department: number;
  DepartmentName: string;
  Grade: number;
  GradeName: string;
  HQ: number;
  HQName: string;
  Region: number[];
  RegionName: string[];
  Pancard: string;
  DOJ: string;
  DOB: string;
  Desigination: number;
  DesiginationName: string;
  Contact: ContactResource;
  Roles: RoleClaims;
}


