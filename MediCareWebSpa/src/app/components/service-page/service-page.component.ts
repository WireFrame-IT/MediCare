import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppointmentService } from '../../services/appointment.service';
import { Subscription } from 'rxjs';
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
export class ServicePageComponent {
  private subscriptions: Subscription[] = [];
  private dialogRef: MatDialogRef<AppointmentDialogComponent> | null = null;

  services: Service[] = [];
  filteredServices: Service[] = [];
  isLoggedIn: boolean = false;
  filter: string = '';
  selectedSort: string = '';

  sortOptions = [
    { label: 'Name', value: 'name' },
    { label: 'Duration', value: 'duration' },
    { label: 'Price', value: 'price' }
  ];

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.appointmentService.loadServices();
    this.subscriptions.push(this.appointmentService.services$.subscribe(services => {
      this.services = services;
      this.applyFilter();
    }));

    this.subscriptions.push(this.authService.isLoggedIn$.subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
      if(!this.isLoggedIn && this.dialogRef)
        this.dialogRef.close();
    }));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscriotion => subscriotion.unsubscribe());
    if(this.dialogRef)
      this.dialogRef.close();
  }

  openAppointmentDialog(service: Service): void {
    if (!this.isLoggedIn) {
      this.router.navigate(['/login']);
      return;
    }

    this.dialogRef = this.dialog.open(AppointmentDialogComponent, {
      width: '600px',
      data: { service: service, serviceId: service.id, status: AppointmentStatus.New } as Appointment
    });

    this.dialogRef.afterClosed().subscribe(appointment => {
      this.appointmentService.addAppointment(appointment);
    });
  }

  applyFilter(): void {
    const query = this.filter.toLowerCase();
    this.filteredServices = this.services.filter(service => service.name.toLowerCase().includes(query) || service.description.toLocaleLowerCase().includes(query));
    this.onSortChange();
  }

  onSortChange(): void {
    this.filteredServices = [...this.filteredServices].sort((a, b) => {
      switch(this.selectedSort) {
        case 'name':
          return a.name.localeCompare(b.name);
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
