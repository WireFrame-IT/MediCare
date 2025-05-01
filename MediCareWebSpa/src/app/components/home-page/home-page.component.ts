import { Component, effect, OnInit } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { AppointmentService } from '../../services/appointment.service';
import { Service } from '../../DTOs/models/service';
import { ICON_MAP } from '../../shared/icon-map';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-home-page',
  imports: [
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatGridListModule,
    RouterModule
  ],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent implements OnInit {
  private servicesEffect = effect(() => this.services = this.getRandomServices(this.appointmentService.services(), 6));

  services: Service[] = [];

  constructor(private appointmentService: AppointmentService) {}

  ngOnInit(): void {
    this.appointmentService.loadServices();
  }

  getIcon = (name: string): string => ICON_MAP[name];

  private getRandomServices(services: Service[], count: number): Service[] {
    const result = [];
    const copy = services.slice();
    while (result.length < count && copy.length > 0) {
      const randomIndex = Math.floor(Math.random() * copy.length);
      result.push(copy.splice(randomIndex, 1)[0]);
    }
    return result;
  }
}
