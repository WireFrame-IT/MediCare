import { Injectable } from "@angular/core";
import { BehaviorSubject, catchError, Observable } from "rxjs";
import { Service } from "../DTOs/models/service";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { LoadingService } from "./loading.service";

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  private _services: BehaviorSubject<Service[]> = new BehaviorSubject<Service[]>([]);
  services$: Observable<Service[]> = this._services.asObservable();

  readonly apiUrl = 'https://localhost:5001/MediCareWebApi/appointments';

  constructor(
      private http: HttpClient,
      private router: Router,
      private loadingService: LoadingService,
    ) {}

  loadSpecialities(): void {
    this.http.get(`${this.apiUrl}/services`).pipe(catchError(error => {
      console.error(error);
      return [];
    })).subscribe(services => this._services.next(services as Service[]));
  }
}
