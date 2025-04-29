import { PrescriptionMedicament } from "./prescription-medicament";

export class Prescription {
  id: number;
  description: string;
  issueDate: Date;
  expirationDate: Date;
  appointmentId: number;
  prescriptionMedicaments: PrescriptionMedicament[] = [];

  constructor(
    id: number,
    description: string,
    issueDate: Date,
    expirationDate: Date,
    appointmentId: number
  ) {
    this.id = id;
    this.description = description;
    this.issueDate = issueDate;
    this.expirationDate = expirationDate;
    this.appointmentId = appointmentId;
  }
}
