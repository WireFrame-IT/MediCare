import { MedicamentUnit } from "../../enums/medicament-unit";
import { Medicament } from "./medicament";
import { Prescription } from "./prescription";

export class PrescriptionMedicament {
  dosage: string;
  quantity: number;
  medicamentUnit: MedicamentUnit;
  prescriptionId: number;
  prescription!: Prescription;
  medicamentId: number;
  medicament!: Medicament;
  notes: string | null;

  constructor(
    dosage: string,
    quantity: number,
    medicamentUnit: MedicamentUnit,
    prescriptionId: number,
    medicamentId: number,
    notes: string | null
  ) {
    this.dosage = dosage
    this.quantity = quantity;
    this.medicamentUnit = medicamentUnit;
    this.prescriptionId = prescriptionId;
    this.medicamentId = medicamentId;
    this.notes = notes;
  }
}
