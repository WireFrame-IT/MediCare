import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, catchError, Observable } from 'rxjs';
import { PatientRegisterRequestDTO } from '../DTOs/request/patient-register-request.dto';
import { LoginRequestDTO } from '../DTOs/request/login-request.dto';
import { RefreshResponseDTO } from '../DTOs/response/refresh-response.dto';
import { RoleType } from '../enums/role-type';
import { Router } from '@angular/router';
import { LoadingService } from './loading.service';
import { Speciality } from '../DTOs/models/speciality';
import { DoctorRegisterRequestDTO } from '../DTOs/request/doctor-register-request.dto';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private _isAdmin: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _isDoctor: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _isLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _specialities: BehaviorSubject<Speciality[]> = new BehaviorSubject<Speciality[]>([]);

  isAdmin$: Observable<boolean> = this._isAdmin.asObservable();
  isDoctor$: Observable<boolean> = this._isDoctor.asObservable();
  isLoggedIn$: Observable<boolean> = this._isLoggedIn.asObservable();
  specialities$: Observable<Speciality[]> = this._specialities.asObservable();

  readonly apiUrl = 'https://localhost:5001/MediCareWebApi/accounts';

  constructor(
    private http: HttpClient,
    private router: Router,
    private loadingService: LoadingService
  ) {}

  login(loginRequest: LoginRequestDTO): Observable<any> {
    return this.http.post<LoginRequestDTO>(`${this.apiUrl}/login`, loginRequest);
  }

  register(registerRequest: PatientRegisterRequestDTO): Observable<any> {
    return this.http.post<PatientRegisterRequestDTO>(`${this.apiUrl}/register`, registerRequest);
  }

  registerDoctor(registerRequest: DoctorRegisterRequestDTO): Observable<any> {
    return this.http.post<DoctorRegisterRequestDTO>(`${this.apiUrl}/register-doctor`, registerRequest);
  }

  refreshAccessToken(): Observable<RefreshResponseDTO> {
    const refreshToken = localStorage.getItem('refreshToken');
    return this.http.post<any>(`${this.apiUrl}/refresh`, { refreshToken });
  }

  getAccessToken(): string | null {
    return sessionStorage.getItem('accessToken');
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }

  getRoleType(): RoleType | null {
    const roleType = Number(sessionStorage.getItem('roleType'));
    if (!isNaN(roleType) && roleType in RoleType) {
      return roleType as RoleType;
    }
    return null;
  }

  storeUserData(accessToken: string, refreshToken: string, roleType?: RoleType): void {
    sessionStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
    sessionStorage.setItem('roleType', roleType?.toString() ?? RoleType.Patient.toString());
    this._isAdmin.next(roleType === RoleType.Admin);
    this._isDoctor.next(roleType === RoleType.Doctor);
    this._isLoggedIn.next(true);
  }

  loadSpecialities(): void {
    this.http.get(`${this.apiUrl}/specialities`)
    .pipe(catchError(error => {
      console.error(error);
      return [];
    }))
    .subscribe(specialities => this._specialities.next(specialities as Speciality[]));
  }

  logout(): void {
    this.loadingService.show();
    this.http.post(`${this.apiUrl}/logout`, {}).subscribe({
      next: () => {
        this._isAdmin.next(false);
        this._isDoctor.next(false);
        this._isLoggedIn.next(false);
        sessionStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        sessionStorage.removeItem('roleType');
        this.router.navigate(['/login']);
      },
      error: err => {
        this.loadingService.hide();
        return err;
      },
      complete: () => {
        this.loadingService.hide();
      }
    })
  };
}
