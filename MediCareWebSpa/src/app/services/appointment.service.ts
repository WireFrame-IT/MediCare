import { Injectable } from "@angular/core";
import { BehaviorSubject, catchError, Observable } from "rxjs";
import { Service } from "../DTOs/models/service";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { LoadingService } from "./loading.service";
import { Doctor } from "../DTOs/models/doctor";
import { Appointment } from "../DTOs/models/appointment";
import { AppointmentRequestDTO } from "../DTOs/request/appointment-request.dto";

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  private _services: BehaviorSubject<Service[]> = new BehaviorSubject<Service[]>([]);
  private _doctors: BehaviorSubject<Doctor[]> = new BehaviorSubject<Doctor[]>([]);
  services$: Observable<Service[]> = this._services.asObservable();
  doctors$: Observable<Doctor[]> = this._doctors.asObservable();

  readonly apiUrl = 'https://localhost:5001/MediCareWebApi/appointments';

  constructor(
      private http: HttpClient,
      private router: Router,
      private loadingService: LoadingService,
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

  saveAppointment(appointmentData: AppointmentRequestDTO): Observable<Appointment> {
    return this.http.post<Appointment>(`${this.apiUrl}`, appointmentData);
  }
}
