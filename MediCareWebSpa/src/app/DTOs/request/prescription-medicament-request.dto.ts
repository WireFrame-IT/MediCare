import { MedicamentUnit } from "../../enums/medicament-unit";

export class PrescriptionMedicamentRequestDTO {
  dosage: string;
  quantity: number;
  medicamentUnit: MedicamentUnit;
  prescriptionAppointmentId: number;
  medicamentId: number;
  notes: string | null;

  constructor(
    dosage: string,
    quantity: number,
    prescriptionAppointmentId: number,
    medicamentUnit: MedicamentUnit,
    medicamentId: number,
    notes: string | null
  ) {
    this.dosage = dosage
    this.quantity = quantity;
    this.prescriptionAppointmentId = prescriptionAppointmentId;
    this.medicamentUnit = medicamentUnit;
    this.medicamentId = medicamentId;
    this.notes = notes;
  }
}
