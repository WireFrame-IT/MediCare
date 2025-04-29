export class PrescriptionDTO {
  description: string;
  expirationDate: Date;

  constructor(
    description: string,
    expirationDate: Date
  ) {
    this.description = description;
    this.expirationDate = expirationDate;
  }
}
