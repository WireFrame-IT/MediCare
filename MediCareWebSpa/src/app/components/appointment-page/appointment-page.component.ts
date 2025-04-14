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
  private dialogRef: MatDialogRef<AppointmentDialogComponent> | null = null;
  subscriptions: Subscription[] = [];
  appointments: Appointment[] = [];
  isDoctor: boolean = false;
  isAdmin: boolean = false;
  isLoggedIn: boolean = false;

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.appointmentService.loadAppointments();
    this.subscriptions.push(this.appointmentService.appointments$.subscribe(appointments => this.appointments = appointments));
    this.subscriptions.push(this.authService.isDoctor$.subscribe(isDoctor => this.isDoctor = isDoctor));
    this.subscriptions.push(this.authService.isLoggedIn$.subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn));
    this.subscriptions.push(this.authService.isAdmin$.subscribe(isAdmin => this.isAdmin = isAdmin));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  onCancel(appointment: Appointment): void {
    this.appointmentService.cancelAppointment(appointment.id).subscribe(response => {
      appointment.status = AppointmentStatus.Canceled;
    });
  }

  onAccept(appointment: Appointment): void {
    this.appointmentService.acceptAppointment(appointment.id).subscribe(response => {
      appointment.status = AppointmentStatus.Accepted;
    });
  }

  getStatus(appointment: Appointment): string {
    return AppointmentStatus[appointment.status];
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

  canCancel(appointment: Appointment): boolean {
    return appointment.status !== AppointmentStatus.Canceled
      && appointment.status !== AppointmentStatus.Absent
      && appointment.status !== AppointmentStatus.Confirmed;
  }

  canAccept(appointment: Appointment): boolean {
    return appointment.status === AppointmentStatus.New;
  }

  openEditAppointmentDialog(appointment: Appointment): void {
    this.dialogRef = this.dialog.open(AppointmentDialogComponent, {
      width: '600px',
      data: appointment
    });
  }
}
