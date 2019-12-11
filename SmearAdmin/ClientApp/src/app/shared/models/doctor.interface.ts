import { ContactResource } from "./contact-resource.interface";
import { ChemistStockistResource } from "./chemist-stockist-resourse.interface";
import { AuditableEntityResource } from "./auditable-entity.interface";

export interface Doctor {
  ID: number;
  Name: string;
  Qualification: string;
  RegistrationNo: string;
  Speciality: string;
  Gender: string;
  VisitFrequency: string;
  VisitPlan: string;
  BestDayToMeet: string;
  BestTimeToMeet: string;
  Brand: string;
  BrandName: string[];
  Class: string;
  Contact: ContactResource;
  Common: ChemistStockistResource;
  AuditableEntity: AuditableEntityResource
}
