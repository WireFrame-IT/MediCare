import { Component } from '@angular/core';
import { AppointmentService } from '../../services/appointment.service';
import { Subscription } from 'rxjs';
import { Service } from '../../DTOs/models/service';
import { ICON_MAP } from '../../shared/icon-map';
import { MatCard } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { AppointmentDialogComponent } from '../appointment-dialog/appointment-dialog.component';

@Component({
  selector: 'app-service-page',
  imports: [
    MatCard,
    MatIcon
  ],
  templateUrl: './service-page.component.html',
  styleUrl: './service-page.component.scss'
})
export class ServicePageComponent {
private subscriptions: Subscription[] = [];
  services: Service[] = [];

  constructor(
    private appointmentService: AppointmentService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.appointmentService.loadServices();
    this.subscriptions.push(this.appointmentService.services$.subscribe(services => this.services = services));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscriotion => subscriotion.unsubscribe());
  }

  openAppointmentDialog(service: Service): void {
    this.dialog.open(AppointmentDialogComponent, {
      width: '600px',
      data: service
    });
  }

  getIcon(name: string): string {
    return ICON_MAP[name];
  }
}
