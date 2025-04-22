import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, catchError, finalize, Observable } from 'rxjs';
import { PatientRegisterRequestDTO } from '../DTOs/request/patient-register-request.dto';
import { LoginRequestDTO } from '../DTOs/request/login-request.dto';
import { RefreshResponseDTO } from '../DTOs/response/refresh-response.dto';
import { RoleType } from '../enums/role-type';
import { Router } from '@angular/router';
import { LoadingService } from './loading.service';
import { Speciality } from '../DTOs/models/speciality';
import { UserRegisterRequestDTO } from '../DTOs/request/user-register-request.dto';
import { RefreshRequestDTO } from '../DTOs/request/refresh-request.dto';
import { UserRequestDTO } from '../DTOs/request/user-request.dto';
import { Doctor } from '../DTOs/models/doctor';
import { Patient } from '../DTOs/models/patient';
import { Permission } from '../DTOs/models/permission';
import { RolePermissionRequest } from '../DTOs/request/role-permission-request.dto';
import { LoginResponseDTO } from '../DTOs/response/login-response.dto';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private _isAdmin: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _isDoctor: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _isLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _specialities: BehaviorSubject<Speciality[]> = new BehaviorSubject<Speciality[]>([]);
  private _permissions: BehaviorSubject<Permission[]> = new BehaviorSubject<Permission[]>([]);

  isAdmin$: Observable<boolean> = this._isAdmin.asObservable();
  isDoctor$: Observable<boolean> = this._isDoctor.asObservable();
  isLoggedIn$: Observable<boolean> = this._isLoggedIn.asObservable();
  specialities$: Observable<Speciality[]> = this._specialities.asObservable();
  permissions$: Observable<Permission[]> = this._permissions.asObservable();

  readonly apiUrl = 'https://localhost:5001/MediCareWebApi/accounts';

  constructor(
    private http: HttpClient,
    private router: Router,
    private loadingService: LoadingService
  ) {}

  login(loginRequest: LoginRequestDTO): Observable<LoginResponseDTO> {
    return this.http.post<LoginResponseDTO>(`${this.apiUrl}/login`, loginRequest);
  }

  register(registerRequest: PatientRegisterRequestDTO): Observable<LoginResponseDTO> {
    return this.http.post<LoginResponseDTO>(`${this.apiUrl}/register`, registerRequest);
  }

  registerUser(registerRequest: UserRegisterRequestDTO): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/register-user`, registerRequest);
  }

  refreshAccessToken(): Observable<RefreshResponseDTO> {
    const refreshRequestDTO = new RefreshRequestDTO(this.getRefreshToken() ?? '');
    return this.http.post<RefreshResponseDTO>(`${this.apiUrl}/refresh`, refreshRequestDTO);
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

  saveUser(user: UserRequestDTO): Observable<Doctor | Patient> {
    return this.http.post<Doctor | Patient>(`${this.apiUrl}/user`, user);
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
      .pipe(catchError(error => []))
      .subscribe(specialities => this._specialities.next(specialities as Speciality[]));
  }

  loadPermissions(): void {
    this.http.get(`${this.apiUrl}/permissions`)
      .pipe(catchError(error => []))
      .subscribe(permissions => this._permissions.next(permissions as Permission[]));
  }

  addRolePermission(rolePermission: RolePermissionRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/role-permission`, rolePermission);
  }

  removeRolePermission(roleType: RoleType, permissionId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/role-permission`, { params: { roleType, permissionId }});
  }

  cleanCredentials(): void {
    this._isAdmin.next(false);
    this._isDoctor.next(false);
    this._isLoggedIn.next(false);
    sessionStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    sessionStorage.removeItem('roleType');
  }

  retrieveCredentials(): void {
    const accessToken = sessionStorage.getItem('accessToken');
    if (!accessToken)
      return;

    this._isLoggedIn.next(true);
    const roleType = this.getRoleType();
    if (roleType) {
      this._isAdmin.next(roleType === RoleType.Admin);
      this._isDoctor.next(roleType === RoleType.Doctor);
    }
  }

  logout(): void {
    this.loadingService.show();
    this.http.post(`${this.apiUrl}/logout`, {})
    .pipe(finalize(() => {
      this.cleanCredentials();
      localStorage.removeItem('services_filter');
      localStorage.removeItem('services_sort_by');
      this.router.navigate(['/login']);
    }))
    .subscribe({
      next: () => {
        this.loadingService.hide();
        this.loadingService.showMessage('Logged out.');
      },
      error: err => {
        this.loadingService.hide();
        this.loadingService.showErrorMessage(err);
      }
    })
  };
}
