export class PrescriptionMedicament {
  dosage: string;
  quantity: number;
  prescriptionId: number;
  medicamentId: number;
  notes: string | null;

  constructor(
    dosage: string,
    quantity: number,
    prescriptionId: number,
    medicamentId: number,
    notes: string | null
  ) {
    this.dosage = dosage
    this.quantity = quantity;
    this.prescriptionId = prescriptionId;
    this.medicamentId = medicamentId;
    this.notes = notes;
  }
}
