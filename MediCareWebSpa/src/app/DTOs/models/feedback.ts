import { Appointment } from "./appointment";
import { Patient } from "./patient";

export class Feedback {
  id: number;
  createdAt: Date;
  description: string;
  rate: number;
  patient: Patient;
  appointmentId: number;
  appointment: Appointment;

  constructor(
    id: number,
    createdAt: Date,
    description: string,
    rate: number,
    patient: Patient,
    appointmentId: number,
    appointment: Appointment
  ) {
    this.id = id;
    this.createdAt = createdAt;
    this.description = description;
    this.rate = rate;
    this.patient = patient;
    this.appointmentId = appointmentId;
    this.appointment = appointment;
  }
}
