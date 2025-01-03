export class AppointmentRequestDTO {
  time: Date;
  doctorId?: number;
  serviceId?: number;

  constructor(time: Date, doctorId?: number, serviceId?: number) {
    this.time = time;
    this.doctorId = doctorId;
    this.serviceId = serviceId;
  }
}
