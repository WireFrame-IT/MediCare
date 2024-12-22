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
  patientId: number;
  patient: Patient;
  doctorId: number;
  doctor: Doctor;
  serviceId: number;
  service: Service;
  feedbacks: Feedback[] = [];

  constructor(
    id: number,
    time: Date,
    status: AppointmentStatus,
    diagnosis: string,
    patientId: number,
    patient: Patient,
    doctorId: number,
    doctor: Doctor,
    serviceId: number,
    service: Service
  ) {
    this.id = id;
    this.time = time;
    this.status = status;
    this.diagnosis = diagnosis;
    this.patientId = patientId;
    this.patient = patient;
    this.doctorId = doctorId;
    this.doctor = doctor;
    this.serviceId = serviceId;
    this.service = service;
  }
}
