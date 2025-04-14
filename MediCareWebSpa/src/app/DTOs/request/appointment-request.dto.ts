import { AppointmentStatus } from "../../enums/appointment-status";

export class AppointmentRequestDTO {
  id: number;
  time: Date;
  status: AppointmentStatus;
  diagnosis: string;
  doctorsUserId?: number;
  serviceId?: number;

  constructor(
    id: number,
    time: Date,
    status: AppointmentStatus,
    diagnosis: string,
    doctorsUserId?: number,
    serviceId?: number
  ) {
    this.id = id;
    this.time = time;
    this.status = status;
    this.diagnosis = diagnosis;
    this.doctorsUserId = doctorsUserId;
    this.serviceId = serviceId;
  }
}
