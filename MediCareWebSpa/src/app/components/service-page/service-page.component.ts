import { Component, effect, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppointmentService } from '../../services/appointment.service';
import { Service } from '../../DTOs/models/service';
import { ICON_MAP } from '../../shared/icon-map';
import { MatCard } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AppointmentDialogComponent } from '../appointment-dialog/appointment-dialog.component';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { Appointment } from '../../DTOs/models/appointment';
import { AppointmentStatus } from '../../enums/appointment-status';
import { MatInputModule } from '@angular/material/input';
import { MatOption, MatSelect } from '@angular/material/select';

@Component({
  selector: 'app-service-page',
  imports: [
    MatCard,
    MatIcon,
    FormsModule,
    MatInputModule,
    MatSelect,
    MatOption
  ],
  templateUrl: './service-page.component.html',
  styleUrl: './service-page.component.scss'
})
export class ServicePageComponent implements OnInit {
  private dialogRef: MatDialogRef<AppointmentDialogComponent> | null = null;

  private servicesEffect = effect(() => {
    this.services = this.appointmentService.services();
    this.applyFilter();
  });

  private isDoctorEffect = effect(() => this.isDoctor = this.authService.isDoctor());

  private isLoggedInEffect = effect(() => {
    this.isLoggedIn = this.authService.isLoggedIn();

    if(!this.isLoggedIn)
      this.dialogRef?.close();
  });

  private specialitiesEffect = effect(() => this.specialityOptions = this.authService.specialities().map(x => ({ label: x.name, value: x.id})));

  specialityOptions: { label: string, value: number}[] = [];
  readonly sortOptions = [
    { label: 'Name', value: 'name' },
    { label: 'Speciality', value: 'speciality' },
    { label: 'Duration', value: 'duration' },
    { label: 'Price', value: 'price' }
  ];

  services: Service[] = [];
  filteredServices: Service[] = [];
  isLoggedIn: boolean = false;
  isDoctor: boolean = false;
  search: string = '';
  selectedSpecialityId: number = 0;
  selectedSort: string = '';

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.selectedSort = sessionStorage.getItem('servicesSortBy') ?? '';
    this.search = sessionStorage.getItem('servicesSearch') ?? '';
    this.selectedSpecialityId = Number(sessionStorage.getItem('servicesSpecialityId')) ?? 0;

    this.appointmentService.loadServices();
    this.authService.loadSpecialities();
  }

  openAppointmentDialog(service: Service): void {
    if (!this.isLoggedIn) {
      this.router.navigate(['/login']);
      return;
    }

    if (this.isDoctor)
      return;

    this.dialogRef = this.dialog.open(AppointmentDialogComponent, {
      width: '500px',
      data: { service: service, serviceId: service.id, status: AppointmentStatus.New } as Appointment,
      autoFocus: false
    });

    this.dialogRef.afterClosed().subscribe(() => {
      this.appointmentService.loadAppointments();
    });
  }

  applyFilter(): void {
    const query = this.search.toLowerCase();
    sessionStorage.setItem('servicesSearch', query);
    sessionStorage.setItem('servicesSpecialityId', this.selectedSpecialityId.toString());
    this.filteredServices = this.services.filter(service =>
      (this.selectedSpecialityId === 0 || service.specialityId === this.selectedSpecialityId) && (service.name.toLowerCase().includes(query) || service.description.toLocaleLowerCase().includes(query)));
    this.onSortChange();
  }

  onSortChange(): void {
    sessionStorage.setItem('servicesSortBy', this.selectedSort);
    this.filteredServices = [...this.filteredServices].sort((a, b) => {
      switch(this.selectedSort) {
        case 'name':
          return a.name.localeCompare(b.name);
        case 'speciality':
          return a.speciality.name.localeCompare(b.speciality.name);
        case 'duration':
          return a.durationMinutes - b .durationMinutes;
        case 'price':
          return a.price - b.price;
        default:
          return 0;
      }
    });
  }

  getIcon = (name: string): string => ICON_MAP[name]

  getDuration = (minutes: number): string => `${Math.floor(minutes / 60).toString().padStart(2, '0')}:${(minutes % 60).toString().padStart(2, '0')}`;
}
