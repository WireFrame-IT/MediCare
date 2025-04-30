import { MedicamentUnit } from "../../enums/medicament-unit";
import { Medicament } from "./medicament";

export class PrescriptionMedicament {
  dosage: string;
  quantity: number;
  medicamentUnit: MedicamentUnit;
  prescriptionId: number;
  medicamentId: number;
  medicament!: Medicament;
  notes: string | null;

  constructor(
    dosage: string,
    quantity: number,
    prescriptionId: number,
    medicamentUnit: MedicamentUnit,
    medicamentId: number,
    notes: string | null
  ) {
    this.dosage = dosage
    this.quantity = quantity;
    this.prescriptionId = prescriptionId;
    this.medicamentUnit = medicamentUnit;
    this.medicamentId = medicamentId;
    this.notes = notes;
  }
}
