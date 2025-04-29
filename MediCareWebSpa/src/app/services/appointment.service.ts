import { Injectable } from "@angular/core";
import { BehaviorSubject, catchError, Observable, map } from "rxjs";
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

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  private _services: BehaviorSubject<Service[]> = new BehaviorSubject<Service[]>([]);
  private _doctors: BehaviorSubject<Doctor[]> = new BehaviorSubject<Doctor[]>([]);
  private _reducedDoctors: BehaviorSubject<ReducedDoctor[]> = new BehaviorSubject<ReducedDoctor[]>([]);
  private _patients: BehaviorSubject<Patient[]> = new BehaviorSubject<Patient[]>([]);
  private _medicaments: BehaviorSubject<Medicament[]> = new BehaviorSubject<Medicament[]>([]);
  private _prescriptions: BehaviorSubject<Prescription[]> = new BehaviorSubject<Prescription[]>([]);
  private _appointments: BehaviorSubject<Appointment[]> = new BehaviorSubject<Appointment[]>([]);

  services$: Observable<Service[]> = this._services.asObservable();
  doctors$: Observable<Doctor[]> = this._doctors.asObservable();
  reducedDoctors$: Observable<ReducedDoctor[]> = this._reducedDoctors.asObservable();
  patients$: Observable<Patient[]> = this._patients.asObservable();
  medicaments$: Observable<Medicament[]> = this._medicaments.asObservable();
  prescriptions$: Observable<Prescription[]> = this._prescriptions.asObservable();
  appointments$: Observable<Appointment[]> = this._appointments.asObservable().pipe(
    map((appointments: Appointment[]) => appointments.slice().sort((a: Appointment, b: Appointment) => new Date(a.time).getTime() - new Date(b.time).getTime()))
  );

  readonly apiUrl = 'https://localhost:5001/MediCareWebApi/appointments';

  constructor(
      private http: HttpClient
    ) {}

  loadServices(): void {
    this.http.get(`${this.apiUrl}/services`).pipe(catchError(error => {
      console.error(error);
      return [];
    })).subscribe(services => this._services.next(services as Service[]));
  }

  loadDoctors(): void {
    this.http.get<Doctor[]>(`${this.apiUrl}/doctors`).pipe(catchError(() => [])).subscribe(doctors => this._doctors.next(doctors as Doctor[]));
  }

  loadReducedDoctors(): void {
    this.http.get<Doctor[]>(`${this.apiUrl}/reduced-doctors`).pipe(catchError(() => [])).subscribe(reducedDoctors => this._reducedDoctors.next(reducedDoctors as ReducedDoctor[]));
  }

  loadPatients(): void {
    this.http.get<Patient[]>(`${this.apiUrl}/patients`).pipe(catchError(() => [])).subscribe(patients => this._patients.next(patients as Patient[]));
  }

  loadMedicaments(): void {
    this.http.get<Medicament[]>(`${this.apiUrl}/medicaments`).pipe(catchError(() => [])).subscribe(medicaments => this._medicaments.next(medicaments as Medicament[]));
  }

  loadPrescriptions(): void {
    this.http.get<Prescription[]>(`${this.apiUrl}/prescriptions`).pipe(catchError(() => [])).subscribe(prescriptions => this._prescriptions.next(prescriptions as Prescription[]));
  }

  loadAppointments(): void {
    this.http.get<Appointment[]>(`${this.apiUrl}`).pipe(catchError(() => [])).subscribe(appointments => this._appointments.next(appointments as Appointment[]));
  }

  saveAppointment(appointmentData: AppointmentRequestDTO): Observable<Appointment> {
    return this.http.post<Appointment>(`${this.apiUrl}`, appointmentData);
  }

  savePrescription(prescription: PrescriptionRequestDTO): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/prescription`, prescription);
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
}
