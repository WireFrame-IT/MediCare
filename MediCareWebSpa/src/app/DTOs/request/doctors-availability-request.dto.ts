export class DoctorsAvailabilityRequest {
  id: number | null;
  from: Date;
  to: Date;

  constructor(
    id: number | null,
    from: Date,
    to: Date
  ) {
    this.id = id;
    this.from = from;
    this.to = to;
  }
}
