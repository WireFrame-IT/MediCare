import { Injectable, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { RegisterRequestDTO } from '../DTOs/request/register-request.dto';
import { PatientRegisterRequestDTO } from '../DTOs/request/patient-register-request.dto';
import { LoginRequestDTO } from '../DTOs/request/login-request.dto';
import { RefreshResponseDTO } from '../DTOs/response/refresh-response.dto';
import { RoleType } from '../enums/role-type';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnDestroy {
  private subscriptions: Subscription[] = [];
  private _isAdmin: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  readonly apiUrl = 'https://localhost:5001/MediCareWebApi';
  isAdmin$: Observable<boolean> = this._isAdmin.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  login(loginRequest: LoginRequestDTO): Observable<any> {
    return this.http.post<LoginRequestDTO>(`${this.apiUrl}/accounts/login`, loginRequest);
  }

  register(registerRequest: PatientRegisterRequestDTO): Observable<any> {
    return this.http.post<PatientRegisterRequestDTO>(`${this.apiUrl}/accounts/register`, registerRequest);
  }

  refreshAccessToken(): Observable<RefreshResponseDTO> {
    const refreshToken = localStorage.getItem('refreshToken');
    return this.http.post<any>(`${this.apiUrl}/accounts/refresh`, { refreshToken });
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
  }

  logout(): void {
    this.subscriptions.push(this.http.post(`${this.apiUrl}/accounts/logout`, {}).subscribe({
      next: () => {
        sessionStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        this.router.navigate(['/login']);
      },
      error: err => err
    }))
  };
}
