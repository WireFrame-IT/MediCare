import { Component, computed, effect, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppointmentService } from '../../services/appointment.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { Service } from '../../DTOs/models/service';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatTimepickerModule } from '@angular/material/timepicker';
import { provideNativeDateAdapter } from '@angular/material/core';
import { AuthService } from '../../services/auth.service';
import { Appointment } from '../../DTOs/models/appointment';
import { AppointmentRequestDTO } from '../../DTOs/request/appointment-request.dto';
import { LoadingService } from '../../services/loading.service';
import { AppointmentStatus } from '../../enums/appointment-status';
import { PermissionType } from '../../enums/permission-type';
import { ReducedDoctor } from '../../DTOs/models/reduced-doctor';

@Component({
  selector: 'app-appointment-dialog',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatOptionModule,
    MatDatepickerModule,
    MatTimepickerModule,
    ReactiveFormsModule
  ],
  providers: [ provideNativeDateAdapter() ],
  templateUrl: './appointment-dialog.component.html',
  styleUrl: './appointment-dialog.component.scss'
})
export class AppointmentDialogComponent implements OnInit {
  isDoctor = computed(() => this.authService.isDoctor());
  isAdmin = computed(() => this.authService.isAdmin());
  services = computed(() => this.appointmentService.services());
  doctors = computed(() => this.appointmentService.reducedDoctors());
  userPermissions = computed(() => this.authService.userPermissions());

  appointmentForm: FormGroup;
  servicesBySpeciality: Service[] = [];
  doctorsBySpeciality: ReducedDoctor[] = [];
  minDate: Date = new Date();
  statusEnum = Object.entries(AppointmentStatus).filter(([key, value]) => !isNaN(Number(value))).map(([key, value]) => ({ key, value }));

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private appointmentService: AppointmentService,
    private loadingService: LoadingService,
    public dialogRef: MatDialogRef<AppointmentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Appointment
  ) {
    this.appointmentForm = this.fb.group({
      serviceId: [data?.serviceId || ''],
      doctorsUserId: [data?.doctorsUserId ||  0],
      date: [data?.time ? new Date(data.time) : '', Validators.required],
      time: [data?.time ? new Date(data.time) : '', Validators.required],
      status: [data?.status ?? AppointmentStatus.New, Validators.required],
      diagnosis: [data?.diagnosis || '']
    });

    effect(() => this.servicesBySpeciality = this.services().filter(x => x.specialityId == this.data.service.specialityId));
    effect(() => this.doctorsBySpeciality = this.doctors().filter(x => x.specialityId === this.data?.service?.specialityId));
  }

  ngOnInit(): void {
    this.authService.loadUserPermissions();
    this.appointmentService.loadServices();
    this.appointmentService.loadReducedDoctors();
  }

  onServiceChange(event: any) {
    this.doctorsBySpeciality = this.doctors().filter(x => x.specialityId === this.findSpecialityIdByServiceId(event.value));
  }

  hasChooseDoctorPermission = (): boolean => this.userPermissions().some(x => x === PermissionType.ChooseDoctor);

  closeDialog = (): void => this.dialogRef.close();

  onSubmit(): void {
    if (this.appointmentForm.valid) {
      const appointment = new AppointmentRequestDTO (
        this.data.id ?? 0,
        this.appointmentForm.value.date,
        this.appointmentForm.value.status,
        this.appointmentForm.value.diagnosis,
        this.appointmentForm.value.doctorsUserId,
        this.appointmentForm.value.serviceId
      );

      appointment.time.setHours(this.appointmentForm.value.time.getHours(), this.appointmentForm.value.time.getMinutes());

      this.loadingService.show();
      this.appointmentService.saveAppointment(appointment).subscribe({
        next: () => {
          this.dialogRef.close();
          this.loadingService.hide();
          this.loadingService.showMessage(this.isDoctor() ? 'Diagnosis has been saved.' : 'Appointment has been saved.');
        },
        error: (error) => {
          this.loadingService.hide();
          this.loadingService.showErrorMessage(this.loadingService.extractErrorMessage(error));
        }
      });
    }
  }

  findSpecialityIdByServiceId(serviceId: number): number | null {
    const specialityId = this.services().find(service => service.id === serviceId)?.specialityId;
    if (specialityId)
      return specialityId
    return null;
  }

  dateFilter(d: Date | null): boolean {
    const day = (d || new Date()).getDay();
    return day !== 0 && day !== 6;
  };
}
