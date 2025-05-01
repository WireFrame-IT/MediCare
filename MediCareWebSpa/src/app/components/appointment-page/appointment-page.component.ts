import { Component, effect, OnInit } from '@angular/core';
import { Appointment } from '../../DTOs/models/appointment';
import { AppointmentService } from '../../services/appointment.service';
import { MatCardModule } from '@angular/material/card';
import { DatePipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../services/auth.service';
import { RouterModule } from '@angular/router';
import { AppointmentStatus } from '../../enums/appointment-status';
import { AppointmentDialogComponent } from '../appointment-dialog/appointment-dialog.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { PermissionType } from '../../enums/permission-type';
import { LoadingService } from '../../services/loading.service';
import { PrescriptionDialogComponent } from '../prescription-dialog/prescription-dialog.component';
import { Prescription } from '../../DTOs/models/prescription';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatOption, MatSelect } from '@angular/material/select';

@Component({
  selector: 'app-appointment-page',
  imports: [
    MatCardModule,
    FormsModule,
    MatInputModule,
    MatSelect,
    MatOption,
    MatButtonModule,
    RouterModule,
    DatePipe
  ],
  templateUrl: './appointment-page.component.html',
  styleUrl: './appointment-page.component.scss'
})
export class AppointmentPageComponent implements OnInit {
  private appointmentDialogRef: MatDialogRef<AppointmentDialogComponent> | null = null;
  private prescriptionDialogRef: MatDialogRef<PrescriptionDialogComponent> | null = null;

  private userPermissionsEffect = effect(() => this.userPermissions = this.authService.userPermissions());
  private prescriptionsEffect = effect(() => this.prescriptions = this.appointmentService.prescriptions());
  private specialitiesEffect = effect(() => this.specialityOptions = this.authService.specialities().map(x => ({ label: x.name, value: x.id})));
  private isAdminEffect = effect(() => this.isAdmin = this.authService.isAdmin());
  private isDoctorEffect = effect(() => this.isDoctor = this.authService.isDoctor());

  private isLoggedInEffect = effect(() => {
    this.isLoggedIn = this.authService.isLoggedIn();

    if (this.isLoggedIn) {
      this.appointmentService.loadAppointments();
      this.appointmentService.loadPrescriptions();
      this.authService.loadUserPsermissions();
    }
  });

  private appointmentsEffect = effect(() => {
    this.appointments = this.appointmentService.appointments();
    this.applyFilter();
  });

  readonly sortOptions = [
    { label: 'Date', value: 'date' },
    { label: 'Service', value: 'service' },
    { label: 'Speciality', value: 'speciality' },
    { label: 'Status', value: 'status' },
    { label: 'Doctor', value: 'doctor' },
    { label: 'Patient', value: 'patient' }
  ];
  readonly statusOptions = Object.keys(AppointmentStatus).filter(key => isNaN(Number(key))).map(key => ({
    label: key,
    value: AppointmentStatus[key as keyof typeof AppointmentStatus]
  }));
  specialityOptions: { label: string, value: number}[] = [];

  appointments: Appointment[] = [];
  filteredAppointments: Appointment[] = [];
  userPermissions: PermissionType[] = [];
  prescriptions: Prescription[] = [];
  isDoctor: boolean = false;
  isAdmin: boolean = false;
  isLoggedIn: boolean = false;
  search: string = '';
  selectedSpecialityId: number = 0;
  selectedStatus = 0;
  selectedSort: string = '';

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
    private loadingService: LoadingService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.authService.loadSpecialities();

    this.search = sessionStorage.getItem('appointmentsSearch') ?? '';
    this.selectedSpecialityId = Number(sessionStorage.getItem('appointmentsSpecialityId')) ?? 0;
    this.selectedStatus = Number(sessionStorage.getItem('appointmentsStatus')) ?? 0;
    this.selectedSort = sessionStorage.getItem('appointmentsSortBy') ?? '';
  }

  onCancel(appointment: Appointment): void {
    this.appointmentService.cancelAppointment(appointment.id).subscribe({
      next: () => {
        this.loadingService.hide();
        appointment.status = AppointmentStatus.Canceled;
        this.loadingService.showMessage('Appointment canceled.');
      },
      error: (error) => {
        this.loadingService.hide();
        this.loadingService.showErrorMessage(this.loadingService.extractErrorMessage(error));
      }
    });
  }

  onAccept(appointment: Appointment): void {
    this.loadingService.show();

    this.appointmentService.acceptAppointment(appointment.id).subscribe({
      next: () => {
        this.loadingService.hide();
        appointment.status = AppointmentStatus.Accepted;
        this.loadingService.showMessage('Appointment accepted.');
      },
      error: (error) => {
        this.loadingService.hide();
        this.loadingService.showErrorMessage(this.loadingService.extractErrorMessage(error));
      }
    });
  }

  onConfirm(appointment: Appointment): void {
    this.loadingService.show();

    this.appointmentService.confirmAppointment(appointment.id).subscribe({
      next: () => {
        this.loadingService.hide();
        appointment.status = AppointmentStatus.Confirmed;
        this.loadingService.showMessage('Appointment confirmed.');
      },
      error: (error) => {
        this.loadingService.hide();
        this.loadingService.showErrorMessage(this.loadingService.extractErrorMessage(error));
      }
    });
  }

  getStatusClass(appointment: Appointment): string {
    switch (appointment.status) {
      case AppointmentStatus.Accepted:
        return 'status-accepted';
      case AppointmentStatus.Confirmed:
        return 'status-confirmed';
      case AppointmentStatus.Canceled:
        return 'status-canceled';
      case AppointmentStatus.Absent:
        return 'status-absent';
    }
    return '';
  }

  getStatus = (appointment: Appointment): string => AppointmentStatus[appointment.status];

  isRightUser = (appointment: Appointment): boolean => {
    const userId = sessionStorage.getItem('userId');

    if (userId == null)
      return false;

    return Number(userId) === appointment.doctorsUserId || Number(userId) === appointment.patientsUserId;
  };

  canSeeAllAppointments = () => this.userPermissions.some(x => x === PermissionType.ViewAllAppointments);

  canAccept = (appointment: Appointment): boolean => this.isRightUser(appointment) && appointment.status === AppointmentStatus.New;

  canConfirm = (appointment: Appointment): boolean => this.isRightUser(appointment) && appointment.status === AppointmentStatus.Accepted;

  isConfirmed = (appointment: Appointment): boolean => appointment.status === AppointmentStatus.Confirmed;

  existsPrescription = (appointment: Appointment): boolean => this.prescriptions.some(x => x.appointmentId == appointment.id);

  canCancel = (appointment: Appointment): boolean => this.userPermissions.some(x => x === PermissionType.CancelAppointment)
    && this.isRightUser(appointment)
    && appointment.status !== AppointmentStatus.Canceled
    && appointment.status !== AppointmentStatus.Absent
    && appointment.status !== AppointmentStatus.Confirmed;

  openPrescriptionDialog(appointment: Appointment): void {
    this.prescriptionDialogRef = this.dialog.open(PrescriptionDialogComponent, {
      width: '700px',
      maxWidth: '700px',
      data: {
        prescription: this.prescriptions.find(x => x.appointmentId === appointment.id),
        appointmentId: appointment.id
      }
    });

    this.prescriptionDialogRef.afterClosed().subscribe(() => {
      this.appointmentService.loadPrescriptions();
    });
  }

  openEditAppointmentDialog(appointment: Appointment): void {
    this.appointmentDialogRef = this.dialog.open(AppointmentDialogComponent, {
      width: '500px',
      data: appointment
    });

    this.appointmentDialogRef.afterClosed().subscribe(() => {
      this.appointmentService.loadAppointments();
    });
  }

  applyFilter(): void {
    const query = this.search.toLowerCase();
    sessionStorage.setItem('appointmentsSearch', query);
    sessionStorage.setItem('appointmentsSpecialityId', this.selectedSpecialityId.toString());
    sessionStorage.setItem('appointmentsStatus', this.selectedStatus.toString());
    this.filteredAppointments = this.appointments.filter(appointment => (this.selectedSpecialityId === 0 || appointment.service.specialityId === this.selectedSpecialityId)
      && (this.selectedStatus === 0 || appointment.status === this.selectedStatus)
      && (appointment.service.name.toLowerCase().includes(query) || appointment.service.description.toLocaleLowerCase().includes(query)
      || appointment.doctor.user.name.toLocaleLowerCase().includes(query) ||  appointment.doctor.user.surname.toLocaleLowerCase().includes(query)
      || appointment.patient.user.name.toLocaleLowerCase().includes(query) ||  appointment.patient.user.surname.toLocaleLowerCase().includes(query)));
    this.onSortChange();
  }

  onSortChange(): void {
    sessionStorage.setItem('appointmentsSortBy', this.selectedSort);
    this.filteredAppointments = [...this.filteredAppointments].sort((a, b) => {
      switch(this.selectedSort) {
        case 'date':
          return new Date(b.time).getTime() - new Date(a.time).getTime();
        case 'service':
          return a.service.name.localeCompare(b.service.name);
        case 'speciality':
          return a.service.speciality.name.localeCompare(b.service.speciality.name);
        case 'status':
          return a.status - b.status;
        case 'doctor':
          return this.compareUsers(a.doctor.user, b.doctor.user);
        case 'patient':
          return this.compareUsers(a.patient.user, b.patient.user);
        default:
          return 0;
      }
    });
  }

  private compareUsers(userA: { name: string, surname: string }, userB: { name: string, surname: string }): number {
    const nameCompare = userA.name.localeCompare(userB.name);
    if (nameCompare !== 0)
      return nameCompare;

    return userA.surname.localeCompare(userB.surname);
  }
}
