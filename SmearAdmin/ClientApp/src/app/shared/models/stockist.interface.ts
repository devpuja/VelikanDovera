import { ContactResource } from "./contact-resource.interface";
import { ChemistStockistResource } from "./chemist-stockist-resourse.interface";
import { AuditableEntityResource } from "./auditable-entity.interface";

export interface Stockist {
  ID: number;
  StockistName: string;
  Contact: ContactResource;
  Common: ChemistStockistResource;
  AuditableEntity: AuditableEntityResource
}
