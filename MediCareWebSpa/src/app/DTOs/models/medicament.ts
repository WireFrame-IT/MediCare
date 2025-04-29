import { MedicamentType } from "../../enums/medicament-type";

export class Medicament {
  id: number;
  name: string;
  description: string;
  medicamentType: MedicamentType;
  prescriptionRequired: boolean;

  constructor(
    id: number,
    name: string,
    description: string,
    medicamentType: MedicamentType,
    prescriptionRequired: boolean
  ) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.medicamentType = medicamentType;
    this.prescriptionRequired = prescriptionRequired;
  }
}
