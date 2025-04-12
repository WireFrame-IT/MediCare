import { Injectable } from "@angular/core";
import { BehaviorSubject, catchError, Observable } from "rxjs";
import { Service } from "../DTOs/models/service";
import { HttpClient } from "@angular/common/http";
import { Doctor } from "../DTOs/models/doctor";
import { Appointment } from "../DTOs/models/appointment";
import { AppointmentRequestDTO } from "../DTOs/request/appointment-request.dto";
import { Patient } from "../DTOs/models/patient";

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  private _services: BehaviorSubject<Service[]> = new BehaviorSubject<Service[]>([]);
  private _doctors: BehaviorSubject<Doctor[]> = new BehaviorSubject<Doctor[]>([]);
  private _patients: BehaviorSubject<Patient[]> = new BehaviorSubject<Patient[]>([]);
  private _appointments: BehaviorSubject<Appointment[]> = new BehaviorSubject<Appointment[]>([]);
  services$: Observable<Service[]> = this._services.asObservable();
  doctors$: Observable<Doctor[]> = this._doctors.asObservable();
  patients$: Observable<Patient[]> = this._patients.asObservable();
  appointments$: Observable<Appointment[]> = this._appointments.asObservable();

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
    this.http.get<Doctor[]>(`${this.apiUrl}/doctors`).pipe(catchError(error => {
      console.error(error);
      return [];
    })).subscribe(doctors => this._doctors.next(doctors as Doctor[]));
  }

  loadPatients(): void {
    this.http.get<Patient[]>(`${this.apiUrl}/patients`).pipe(catchError(error => {
      console.error(error);
      return [];
    })).subscribe(patients => this._patients.next(patients as Patient[]));
  }

  loadAppointments(): void {
    this.http.get<Appointment[]>(`${this.apiUrl}`).pipe(catchError(error => {
      console.error(error);
      return [];
    })).subscribe(appointments => this._appointments.next(appointments as Appointment[]));
  }

  saveAppointment(appointmentData: AppointmentRequestDTO): Observable<Appointment> {
    return this.http.post<Appointment>(`${this.apiUrl}`, appointmentData);
  }

  acceptAppointment(id: number): Observable<null> {
    return this.http.post<null>(`${this.apiUrl}/accept?id=${id}`, null);
  }

  cancelAppointment(id: number): Observable<null> {
    return this.http.post<null>(`${this.apiUrl}/cancel?id=${id}`, null);
  }
}
