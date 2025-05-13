export class FeedbackRequestDTO {
  rate: number;
  description: string;
  appointmentId: number;

  constructor(
    rate: number,
    description: string,
    appointmentId: number
  ) {
    this.rate = rate;
    this.description = description;
    this.appointmentId = appointmentId;
  }
}
