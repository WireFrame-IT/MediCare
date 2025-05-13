import { Component, computed, effect, OnInit } from '@angular/core';
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
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatOption, MatSelect } from '@angular/material/select';
import { FeedbackDialogComponent } from '../feedback-dialog/feedback-dialog.component';

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
  private feedbackDialogRef: MatDialogRef<FeedbackDialogComponent> | null = null;

  isDoctor = computed(() => this.authService.isDoctor());
  isAdmin = computed(() => this.authService.isAdmin());
  isLoggedIn = computed(() => this.authService.isLoggedIn());
  userPermissions = computed(() => this.authService.userPermissions());
  prescriptions = computed(() => this.appointmentService.prescriptions());
  appointments = computed(() => this.appointmentService.appointments());
  feedbacks = computed(() => this.appointmentService.feedbacks());
  specialityOptions = computed(() => this.authService.specialities().map(x => ({ label: x.name, value: x.id})));

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

  filteredAppointments: Appointment[] = [];
  search: string = '';
  selectedSpecialityId: number = 0;
  selectedStatus = 0;
  selectedSort: string = '';

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
    private loadingService: LoadingService,
    private dialog: MatDialog
  ) {
    effect(() => {
      if (this.isLoggedIn()) {
        this.appointmentService.loadAppointments();
        this.appointmentService.loadPrescriptions();
        this.authService.loadUserPermissions();
      } else {
        this.appointmentDialogRef?.close();
        this.prescriptionDialogRef?.close();
      }
    });

    effect(() => this.applyFilter());
  }

  ngOnInit(): void {
    this.authService.loadSpecialities();
    this.appointmentService.loadFeedbacks();

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

  isThisDoctor = (appointment: Appointment): boolean => {
    const userId = sessionStorage.getItem('userId');

    if (userId == null)
      return false;

    return Number(userId) === appointment.doctorsUserId;
  }

  isThisPatient = (appointment: Appointment): boolean => {
    const userId = sessionStorage.getItem('userId');

    if (userId == null)
      return false;

    return Number(userId) === appointment.patientsUserId;
  }

  isRightUser = (appointment: Appointment): boolean => this.isThisDoctor(appointment) || this.isThisPatient(appointment);

  canSeeAllAppointments = () => this.userPermissions().some(x => x === PermissionType.ViewAllAppointments);

  canAccept = (appointment: Appointment): boolean => this.isThisDoctor(appointment) && appointment.status === AppointmentStatus.New;

  canConfirm = (appointment: Appointment): boolean => this.isThisDoctor(appointment) && appointment.status === AppointmentStatus.Accepted;

  isConfirmed = (appointment: Appointment): boolean => appointment.status === AppointmentStatus.Confirmed;

  existsPrescription = (appointment: Appointment): boolean => this.prescriptions().some(x => x.appointmentId == appointment.id);

  canCancel = (appointment: Appointment): boolean => this.userPermissions().some(x => x === PermissionType.CancelAppointment)
    && this.isRightUser(appointment)
    && appointment.status !== AppointmentStatus.Canceled
    && appointment.status !== AppointmentStatus.Absent
    && appointment.status !== AppointmentStatus.Confirmed;

  openPrescriptionDialog(appointment: Appointment): void {
    this.prescriptionDialogRef = this.dialog.open(PrescriptionDialogComponent, {
      width: '700px',
      maxWidth: '700px',
      data: {
        prescription: this.prescriptions().find(x => x.appointmentId === appointment.id),
        appointmentId: appointment.id,
        doctorsUserId: appointment.doctorsUserId
      },
      autoFocus: false
    });

    this.prescriptionDialogRef.afterClosed().subscribe(() => this.appointmentService.loadPrescriptions());
  }

  openEditAppointmentDialog(appointment: Appointment): void {
    this.appointmentDialogRef = this.dialog.open(AppointmentDialogComponent, {
      width: '500px',
      data: appointment,
      autoFocus: false
    });

    this.appointmentDialogRef.afterClosed().subscribe(() => this.appointmentService.loadAppointments());
  }

  openFeedbackDialog(appointment: Appointment): void {
    this.feedbackDialogRef = this.dialog.open(FeedbackDialogComponent, {
      width: '500px',
      data: {
        feedback: this.feedbacks().find(x => x.appointmentId === appointment.id),
        appointmentId: appointment.id,
        isThisPatient: this.isThisPatient(appointment)
      },
      autoFocus: false
    });

    this.feedbackDialogRef.afterClosed().subscribe(response => {
      if (response) {
        this.appointmentService.feedbacks.update(current => {
        const index = current.findIndex(f => f.appointmentId === appointment.id);

        if (index === -1) {
          return [...current, response];
        } else {
          const updated = [...current];
          updated[index] = response;
          return updated;
        }
      });
      }
    });
  }

  applyFilter(): void {
    const normalize = (text: string): string => text.trim().replace(/\s+/g, ' ').toLocaleLowerCase();
    const matchesQuery = (...fields: string[]) => fields.some(field => normalize(field).includes(query));
    const query = normalize(this.search);

    sessionStorage.setItem('appointmentsSearch', query);
    sessionStorage.setItem('appointmentsSpecialityId', this.selectedSpecialityId.toString());
    sessionStorage.setItem('appointmentsStatus', this.selectedStatus.toString());

    this.filteredAppointments = this.appointments().filter(x => (this.selectedSpecialityId === 0 || x.service.specialityId === this.selectedSpecialityId)
      && (this.selectedStatus === 0 || x.status === this.selectedStatus));

    if (!this.isAdmin && !this.isDoctor)
      this.filteredAppointments = this.filteredAppointments.filter(x =>
        matchesQuery(x.service.name, x.service.description, `${x.doctor.user.name} ${x.doctor.user.surname}`, `${x.doctor.user.surname} ${x.doctor.user.name}`));
    else if (this.isAdmin() || (this.isDoctor() && this.canSeeAllAppointments()))
      this.filteredAppointments = this.filteredAppointments.filter(x =>
        matchesQuery(x.service.name, x.service.description, `${x.doctor.user.name} ${x.doctor.user.surname}`, `${x.doctor.user.surname} ${x.doctor.user.name}`, `${x.patient.user.name} ${x.patient.user.surname}`, `${x.patient.user.surname} ${x.patient.user.name}`));

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
