import { Component, computed, effect, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppointmentService } from '../../services/appointment.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { Service } from '../../DTOs/models/service';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatTimepickerModule, MatTimepickerOption } from '@angular/material/timepicker';
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

  timeOptions: MatTimepickerOption<Date>[] = [
    { value: new Date(0, 0, 0, 8, 0), label: '08:00' },
    { value: new Date(0, 0, 0, 8, 15), label: '08:15' },
    { value: new Date(0, 0, 0, 8, 30), label: '08:30' },
    { value: new Date(0, 0, 0, 8, 45), label: '08:45' },
    { value: new Date(0, 0, 0, 9, 0), label: '09:00' },
    { value: new Date(0, 0, 0, 9, 15), label: '09:15' },
    { value: new Date(0, 0, 0, 9, 30), label: '09:30' },
    { value: new Date(0, 0, 0, 9, 45), label: '09:45' },
    { value: new Date(0, 0, 0, 10, 0), label: '10:00' },
    { value: new Date(0, 0, 0, 10, 15), label: '10:15' },
    { value: new Date(0, 0, 0, 10, 30), label: '10:30' },
    { value: new Date(0, 0, 0, 10, 45), label: '10:45' },
    { value: new Date(0, 0, 0, 11, 0), label: '11:00' },
    { value: new Date(0, 0, 0, 11, 15), label: '11:15' },
    { value: new Date(0, 0, 0, 11, 30), label: '11:30' },
    { value: new Date(0, 0, 0, 11, 45), label: '11:45' },
    { value: new Date(0, 0, 0, 12, 0), label: '12:00' },
    { value: new Date(0, 0, 0, 12, 15), label: '12:15' },
    { value: new Date(0, 0, 0, 12, 30), label: '12:30' },
    { value: new Date(0, 0, 0, 12, 45), label: '12:45' },
    { value: new Date(0, 0, 0, 13, 0), label: '13:00' },
    { value: new Date(0, 0, 0, 13, 15), label: '13:15' },
    { value: new Date(0, 0, 0, 13, 30), label: '13:30' },
    { value: new Date(0, 0, 0, 13, 45), label: '13:45' },
    { value: new Date(0, 0, 0, 14, 0), label: '14:00' },
    { value: new Date(0, 0, 0, 14, 15), label: '14:15' },
    { value: new Date(0, 0, 0, 14, 30), label: '14:30' },
    { value: new Date(0, 0, 0, 14, 45), label: '14:45' },
    { value: new Date(0, 0, 0, 15, 0), label: '15:00' },
    { value: new Date(0, 0, 0, 15, 15), label: '15:15' },
    { value: new Date(0, 0, 0, 15, 30), label: '15:30' },
    { value: new Date(0, 0, 0, 15, 45), label: '15:45' }
  ];
  availableTimeOptions: MatTimepickerOption<Date>[] = [];

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

  onServiceChange(event: MatSelectChange) {
    this.doctorsBySpeciality = this.doctors().filter(x => x.specialityId === this.findSpecialityIdByServiceId(event.value));

    this.onDoctorChange();
  }

  onDoctorChange() {
    this.appointmentForm.patchValue({ date: null, time: null });
    this.availableTimeOptions = [];
  }

  onDateChange(selectedDate: Date | null): void {
    this.availableTimeOptions = [];
    if (selectedDate == null)
      return;

    const selectedDoctorId = this.appointmentForm.value.doctorsUserId;
    const selectedService = this.services().find(x => x.id === this.appointmentForm.value.serviceId)
    const relevantDoctors = this.doctors().filter(doctor => selectedDoctorId ? doctor.userId === selectedDoctorId : doctor.specialityId === selectedService?.specialityId);

    const updatedTimeOptions: MatTimepickerOption<Date>[] = this.timeOptions.map(opt => ({
      label: opt.label,
      value: new Date(
        selectedDate.getFullYear(),
        selectedDate.getMonth(),
        selectedDate.getDate(),
        opt.value.getHours(),
        opt.value.getMinutes()
      )
    }));

    this.availableTimeOptions = updatedTimeOptions.filter(option =>
      relevantDoctors.some(doctor => {
        const optionStart = option.value;

        const optionEnd = new Date(option.value.getTime());
        optionEnd.setMinutes(optionEnd.getMinutes() + (selectedService?.durationMinutes ?? 0));

        const endOfWork = new Date(option.value);
        endOfWork.setHours(16, 0, 0, 0);

        return doctor.doctorsAvailabilities.some(availability =>
          new Date(availability.from) <= optionStart && optionEnd <= endOfWork && (availability.to == null || optionEnd <= new Date(availability.to)))
          && doctor.unavailableTerms.every(term => optionEnd <= new Date(term.start) || optionStart >= new Date(term.end))
    }));
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
          this.loadingService.showMessage(this.isDoctor() ? 'Diagnoza została zapisana.' : 'Wizyta została zapisana.');
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
