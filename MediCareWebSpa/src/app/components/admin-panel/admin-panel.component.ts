import { Component, OnDestroy, OnInit } from '@angular/core';
import { AppointmentService } from '../../services/appointment.service';
import { Doctor } from '../../DTOs/models/doctor';
import { MatCardModule } from '@angular/material/card';
import { Subscription } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { EditUserDialogComponent } from '../edit-user-dialog/edit-user-dialog.component';

@Component({
  selector: 'app-admin-panel',
  imports: [
    MatCardModule,
    MatButtonModule
  ],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.scss'
})
export class AdminPanelComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  private dialogRef: MatDialogRef<EditUserDialogComponent> | null = null;
  public doctors: Doctor[] = [];

  constructor(
      private appointmentService: AppointmentService,
      private dialog: MatDialog
    ) {}

  ngOnInit(): void {
    this.appointmentService.loadDoctors();
    this.subscriptions.push(this.appointmentService.doctors$.subscribe(doctors => this.doctors = doctors));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  openEditUserDialog(doctor: Doctor): void {
    this.dialogRef = this.dialog.open(EditUserDialogComponent, {
      width: '600px',
      data: doctor
    });
  }
}
