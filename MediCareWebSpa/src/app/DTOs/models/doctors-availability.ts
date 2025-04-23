import { Doctor } from "./doctor";

export class DoctorsAvailability {
  id: number;
  doctorsUserId: number;
  doctor: Doctor;
  from: Date;
  to: Date;

  constructor(
    id: number,
    doctorsUserId: number,
    doctor: Doctor,
    from: Date,
    to: Date
  ) {
    this.id = id;
    this.doctorsUserId = doctorsUserId
    this.doctor = doctor;
    this.from = from;
    this.to = to;
  }
}
