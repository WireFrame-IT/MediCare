import { Component } from '@angular/core';
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
  isLoggedIn: boolean = false;
  private dialogRef: MatDialogRef<AppointmentDialogComponent> | null = null;

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.appointmentService.loadServices();
    this.subscriptions.push(this.appointmentService.services$.subscribe(services => this.services = services));
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
      data: service
    });
  }

  getIcon(name: string): string {
    return ICON_MAP[name];
  }
}
