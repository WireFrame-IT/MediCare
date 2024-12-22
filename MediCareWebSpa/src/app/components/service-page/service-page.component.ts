import { Component } from '@angular/core';
import { AppointmentService } from '../../services/appointment.service';
import { Subscription } from 'rxjs';
import { Service } from '../../DTOs/models/service';
import { ICON_MAP } from '../../shared/icon-map';
import { NgFor } from '@angular/common';
import { MatCard } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-service-page',
  imports: [
    NgFor,
    MatCard,
    MatIcon
  ],
  templateUrl: './service-page.component.html',
  styleUrl: './service-page.component.scss'
})
export class ServicePageComponent {
private subscriptions: Subscription[] = [];
  services: Service[] = [];

  constructor(private appointmentService: AppointmentService) {}

  ngOnInit(): void {
    this.appointmentService.loadSpecialities();
    this.subscriptions.push(this.appointmentService.services$.subscribe(services => this.services = services));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscriotion => subscriotion.unsubscribe());
  }

  getIcon(name: string): string {
    return ICON_MAP[name];
  }
}
