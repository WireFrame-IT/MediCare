import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgFor } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { AppointmentService } from '../../services/appointment.service';
import { Service } from '../../DTOs/models/service';
import { Subscription } from 'rxjs';
import { ICON_MAP } from '../../shared/icon-map';

@Component({
  selector: 'app-home-page',
  imports: [
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatGridListModule,
    NgFor
  ],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent implements OnInit, OnDestroy {
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
