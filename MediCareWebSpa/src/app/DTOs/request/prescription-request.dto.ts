export class PrescriptionRequestDTO {
  appointmentId: number;
  description: string;
  expirationDate: Date;

  constructor(
    appointmentId: number,
    description: string,
    expirationDate: Date
  ) {
    this.appointmentId = appointmentId;
    this.description = description;
    this.expirationDate = expirationDate;
  }
}
