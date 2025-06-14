import { Injectable, signal } from "@angular/core";
import { catchError, Observable, of } from "rxjs";
import { Service } from "../DTOs/models/service";
import { HttpClient } from "@angular/common/http";
import { Doctor } from "../DTOs/models/doctor";
import { Appointment } from "../DTOs/models/appointment";
import { AppointmentRequestDTO } from "../DTOs/request/appointment-request.dto";
import { Patient } from "../DTOs/models/patient";
import { ReducedDoctor } from "../DTOs/models/reduced-doctor";
import { Medicament } from "../DTOs/models/medicament";
import { PrescriptionRequestDTO } from "../DTOs/request/prescription-request.dto";
import { Prescription } from "../DTOs/models/prescription";
import { MedicamentType } from "../enums/medicament-type";
import { MedicamentUnit } from "../enums/medicament-unit";
import { PrescriptionMedicamentRequestDTO } from "../DTOs/request/prescription-medicament-request.dto";
import { PrescriptionMedicament } from "../DTOs/models/prescription-medicament";
import { Feedback } from "../DTOs/models/feedback";
import { FeedbackRequestDTO } from "../DTOs/request/feedback-request.dto";
import { MEDICAMENT_TYPE_MAP } from "../shared/maps/medicament-type-map";
import { MEDICAMENT_UNIT_MAP } from "../shared/maps/medicament-unit-map";

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  private readonly apiUrl = 'https://medicare20250528195544-hucfcyf3dgatf6bn.northeurope-01.azurewebsites.net/MediCareWebApi/appointments';

  services = signal<Service[]>([]);
  doctors = signal<Doctor[]>([]);
  reducedDoctors = signal<ReducedDoctor[]>([]);
  patients = signal<Patient[]>([]);
  medicaments = signal<Medicament[]>([]);
  prescriptions = signal<Prescription[]>([]);
  appointments = signal<Appointment[]>([]);
  feedbacks = signal<Feedback[]>([]);

  constructor(private http: HttpClient) {}

  loadServices(): void {
    this.http.get<Service[]>(`${this.apiUrl}/services`)
      .pipe(catchError(() => of([])))
      .subscribe(services => this.services.set(services as Service[]));
  }

  loadDoctors(): void {
    this.http.get<Doctor[]>(`${this.apiUrl}/doctors`)
      .pipe(catchError(() => of([])))
      .subscribe(doctors => this.doctors.set(doctors as Doctor[]));
  }

  loadReducedDoctors(): void {
    this.http.get<Doctor[]>(`${this.apiUrl}/reduced-doctors`)
      .pipe(catchError(() => of([])))
      .subscribe(reducedDoctors => this.reducedDoctors.set(reducedDoctors as ReducedDoctor[]));
  }

  loadPatients(): void {
    this.http.get<Patient[]>(`${this.apiUrl}/patients`)
      .pipe(catchError(() => of([])))
      .subscribe(patients => this.patients.set(patients as Patient[]));
  }

  loadMedicaments(): void {
    this.http.get<Medicament[]>(`${this.apiUrl}/medicaments`)
      .pipe(catchError(() => of([])))
      .subscribe(medicaments => this.medicaments.set(medicaments as Medicament[]));
  }

  loadPrescriptions(): void {
    this.http.get<Prescription[]>(`${this.apiUrl}/prescriptions`)
      .pipe(catchError(() => of([])))
      .subscribe(prescriptions => this.prescriptions.set(prescriptions as Prescription[]));
  }

  loadAppointments(): void {
    this.http.get<Appointment[]>(`${this.apiUrl}`)
      .pipe(catchError(() => of([])))
      .subscribe(appointments => this.appointments.set(appointments as Appointment[]));
  }

  loadFeedbacks(): void {
    this.http.get<Feedback[]>(`${this.apiUrl}/feedbacks`)
      .pipe(catchError(() => of([])))
      .subscribe(feedbacks => this.feedbacks.set(feedbacks as Feedback[]))
  }

  removePrescriptionMedicament(prescriptionAppointmentId: number, medicamentId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/prescription-medicament`, { params: { prescriptionAppointmentId, medicamentId }});
  }

  saveAppointment(appointmentData: AppointmentRequestDTO): Observable<Appointment> {
    return this.http.post<Appointment>(`${this.apiUrl}`, appointmentData);
  }

  savePrescription(prescription: PrescriptionRequestDTO): Observable<Prescription> {
    return this.http.post<Prescription>(`${this.apiUrl}/prescription`, prescription);
  }

  savePrescriptionMedicament(prescriptionMedicament: PrescriptionMedicamentRequestDTO): Observable<PrescriptionMedicament> {
    return this.http.post<PrescriptionMedicament>(`${this.apiUrl}/prescription-medicament`, prescriptionMedicament);
  }

  acceptAppointment(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/accept?id=${id}`, null);
  }

  confirmAppointment(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/confirm?id=${id}`, null);
  }

  cancelAppointment(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/cancel?id=${id}`, null);
  }

  saveFeedback(feedback: FeedbackRequestDTO): Observable<Feedback> {
    return this.http.post<Feedback>(`${this.apiUrl}/feedback`, feedback);
  }

  getMedicamentTypeName = (type: MedicamentType): string => MEDICAMENT_TYPE_MAP[type];

  getMedicamentUnitName = (unit: MedicamentUnit): string => MEDICAMENT_UNIT_MAP[unit];
}
