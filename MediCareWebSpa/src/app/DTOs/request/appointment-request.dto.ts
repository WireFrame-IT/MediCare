export class AppointmentRequestDTO {
  time: Date;
  doctorsUserId?: number;
  serviceId?: number;

  constructor(time: Date, doctorsUserId?: number, serviceId?: number) {
    this.time = time;
    this.doctorsUserId = doctorsUserId;
    this.serviceId = serviceId;
  }
}
