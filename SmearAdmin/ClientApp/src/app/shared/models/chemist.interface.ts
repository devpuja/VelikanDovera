import { ContactResource } from "./contact-resource.interface";
import { ChemistStockistResource } from "./chemist-stockist-resourse.interface";
import { AuditableEntityResource } from "./auditable-entity.interface";

export interface Chemist {
  ID: number;
  MedicalName: string;
  Class: string;
  Contact: ContactResource;
  Common: ChemistStockistResource;
  AuditableEntity: AuditableEntityResource
}
