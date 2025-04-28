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
import { PermissionType } from '../enums/permission-type';
import { DoctorsAvailability } from '../DTOs/models/doctors-availability';
import { DoctorsAvailabilityRequest } from '../DTOs/request/doctors-availability-request.dto';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private _isAdmin: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _isDoctor: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _isLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _specialities: BehaviorSubject<Speciality[]> = new BehaviorSubject<Speciality[]>([]);
  private _permissions: BehaviorSubject<Permission[]> = new BehaviorSubject<Permission[]>([]);
  private _userPermissions: BehaviorSubject<PermissionType[]> = new BehaviorSubject<PermissionType[]>([]);
  private _availabilities: BehaviorSubject<DoctorsAvailability[]> = new BehaviorSubject<DoctorsAvailability[]>([]);

  isAdmin$: Observable<boolean> = this._isAdmin.asObservable();
  isDoctor$: Observable<boolean> = this._isDoctor.asObservable();
  isLoggedIn$: Observable<boolean> = this._isLoggedIn.asObservable();
  specialities$: Observable<Speciality[]> = this._specialities.asObservable();
  permissions$: Observable<Permission[]> = this._permissions.asObservable();
  userPermissions$: Observable<PermissionType[]> = this._userPermissions.asObservable();
  availabilities$: Observable<DoctorsAvailability[]> = this._availabilities.asObservable();

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
    return sessionStorage.getItem('refreshToken');
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

  storeUserData(userId: number, userName: string, userSurname: string, accessToken: string, refreshToken: string, roleType?: RoleType): void {
    sessionStorage.setItem('userId', userId.toString());
    sessionStorage.setItem('userName', userName);
    sessionStorage.setItem('userSurname', userSurname);
    sessionStorage.setItem('roleType', roleType?.toString() ?? RoleType.Patient.toString());
    sessionStorage.setItem('accessToken', accessToken);
    sessionStorage.setItem('refreshToken', refreshToken);
    this._isAdmin.next(roleType === RoleType.Admin);
    this._isDoctor.next(roleType === RoleType.Doctor);
    this._isLoggedIn.next(true);
  }

  loadSpecialities(): void {
    this.http.get(`${this.apiUrl}/specialities`)
      .pipe(catchError(() => []))
      .subscribe(specialities => this._specialities.next(specialities as Speciality[]));
  }

  loadPermissions(): void {
    this.http.get(`${this.apiUrl}/permissions`)
      .pipe(catchError(() => []))
      .subscribe(permissions => this._permissions.next(permissions as Permission[]));
  }

  loadUserPsermissions(): void {
    this.http.get(`${this.apiUrl}/user-permissions`)
      .pipe(catchError(() => []))
      .subscribe(userPermissions => this._userPermissions.next(userPermissions as PermissionType[]));
  }

  loadDoctorsAvailabilities(): void {
    this.http.get(`${this.apiUrl}/availabilities`)
      .pipe(catchError(() => []))
      .subscribe(availabilities => this._availabilities.next(availabilities as DoctorsAvailability[]));
  }

  addRolePermission(rolePermission: RolePermissionRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/role-permission`, rolePermission);
  }

  removeRolePermission(roleType: RoleType, permissionId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/role-permission`, { params: { roleType, permissionId }});
  }

  removeDoctorsAvailability(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/availability`, { params: { id }});
  }

  saveDoctorsAvailability(availability: DoctorsAvailabilityRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/availability`, availability);
  }

  cleanCredentials(): void {
    this._isAdmin.next(false);
    this._isDoctor.next(false);
    this._isLoggedIn.next(false);
    sessionStorage.removeItem('accessToken');
    sessionStorage.removeItem('refreshToken');
    sessionStorage.removeItem('userId');
    sessionStorage.removeItem('userName');
    sessionStorage.removeItem('userSurname');
    sessionStorage.removeItem('roleType');
    sessionStorage.removeItem('servicesFilter');
    sessionStorage.removeItem('servicesSortBy');
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

  getUserNameAndSurname(): string {
    const name = sessionStorage.getItem('userName');
    const surname = sessionStorage.getItem('userSurname');
    return name && surname ? `${name} ${surname}` : '';
  }

  logout(): void {
    this.loadingService.show();
    this.http.post(`${this.apiUrl}/logout`, null)
    .pipe(finalize(() => {
      this.cleanCredentials();
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
