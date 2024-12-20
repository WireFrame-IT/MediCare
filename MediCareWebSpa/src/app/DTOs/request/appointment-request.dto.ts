export class AppointmentRequestDTO {
  time: string;
  doctorId?: number;
  serviceId?: number;

  constructor(time: string, doctorId?: number, serviceId?: number) {
    this.time = time;
    this.doctorId = doctorId;
    this.serviceId = serviceId;
  }
}
