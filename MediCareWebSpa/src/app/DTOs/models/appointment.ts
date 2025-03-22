import { AppointmentStatus } from "../../enums/appointment-status";
import { Doctor } from "./doctor";
import { Feedback } from "./feedback";
import { Patient } from "./patient";
import { Service } from "./service";

export class Appointment {
  id: number;
  time: Date;
  status: AppointmentStatus;
  diagnosis: string;
  patientsUserId: number;
  patient: Patient;
  doctorsUserId: number;
  doctor: Doctor;
  serviceId: number;
  service: Service;
  feedbacks: Feedback[] = [];

  constructor(
    id: number,
    time: Date,
    status: AppointmentStatus,
    diagnosis: string,
    patientsUserId: number,
    patient: Patient,
    doctorsUserId: number,
    doctor: Doctor,
    serviceId: number,
    service: Service
  ) {
    this.id = id;
    this.time = time;
    this.status = status;
    this.diagnosis = diagnosis;
    this.patientsUserId = patientsUserId;
    this.patient = patient;
    this.doctorsUserId = doctorsUserId;
    this.doctor = doctor;
    this.serviceId = serviceId;
    this.service = service;
  }
}
