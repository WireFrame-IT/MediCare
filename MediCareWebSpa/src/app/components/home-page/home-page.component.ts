import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { AppointmentService } from '../../services/appointment.service';
import { Service } from '../../DTOs/models/service';
import { Subscription } from 'rxjs';
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
export class HomePageComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  services: Service[] = [];

  constructor(private appointmentService: AppointmentService) {}

  ngOnInit(): void {
    this.appointmentService.loadServices();
    this.subscriptions.push(this.appointmentService.services$.subscribe(services => this.services = this.getRandomServices(services, 6)));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscriotion => subscriotion.unsubscribe());
  }

  getIcon(name: string): string {
    return ICON_MAP[name];
  }

  getImageUrls(count: number): any {
    const urls = []
    for (let i = 1; i <= count; i += 2) {
      urls.push({
        first: `assets/medical-facility/${i}.svg`,
        second: `assets/medical-facility/${i + 1}.svg`
      });
    }
    return urls;
  }

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
