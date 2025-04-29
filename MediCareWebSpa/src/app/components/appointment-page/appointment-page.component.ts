import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
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

@Component({
  selector: 'app-appointment-page',
  imports: [
    MatCardModule,
    MatButtonModule,
    RouterModule,
    DatePipe
  ],
  templateUrl: './appointment-page.component.html',
  styleUrl: './appointment-page.component.scss'
})
export class AppointmentPageComponent implements OnInit, OnDestroy {
  private appointmentDialogRef: MatDialogRef<AppointmentDialogComponent> | null = null;
  private prescriptionDialogRef: MatDialogRef<PrescriptionDialogComponent> | null = null;

  subscriptions: Subscription[] = [];
  appointments: Appointment[] = [];
  userPermissions: PermissionType[] = [];
  prescriptions: Prescription[] = [];
  isDoctor: boolean = false;
  isAdmin: boolean = false;
  isLoggedIn: boolean = false;

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
    private loadingService: LoadingService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.appointmentService.loadAppointments();
    this.appointmentService.loadPrescriptions();
    this.authService.loadUserPsermissions();

    this.subscriptions.push(this.appointmentService.appointments$.subscribe(appointments => this.appointments = appointments));
    this.subscriptions.push(this.authService.isDoctor$.subscribe(isDoctor => this.isDoctor = isDoctor));
    this.subscriptions.push(this.authService.isLoggedIn$.subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn));
    this.subscriptions.push(this.authService.isAdmin$.subscribe(isAdmin => this.isAdmin = isAdmin));
    this.subscriptions.push(this.authService.userPermissions$.subscribe(userPermissions => this.userPermissions = userPermissions));
    this.subscriptions.push(this.appointmentService.prescriptions$.subscribe(prescriptions => this.prescriptions = prescriptions));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
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
}
