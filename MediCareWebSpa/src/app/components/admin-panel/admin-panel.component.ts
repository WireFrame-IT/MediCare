import { Component, OnDestroy, OnInit } from '@angular/core';
import { AppointmentService } from '../../services/appointment.service';
import { Doctor } from '../../DTOs/models/doctor';
import { MatCardModule } from '@angular/material/card';
import { Subscription } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { EditUserDialogComponent } from '../edit-user-dialog/edit-user-dialog.component';
import { Patient } from '../../DTOs/models/patient';

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
  private doctors: Doctor[] = [];
  private patients: Patient[] = [];

  constructor(
      private appointmentService: AppointmentService,
      private dialog: MatDialog
    ) {}

  ngOnInit(): void {
    this.appointmentService.loadDoctors();
    this.appointmentService.loadPatients();
    this.subscriptions.push(this.appointmentService.doctors$.subscribe(doctors => this.doctors = doctors));
    this.subscriptions.push(this.appointmentService.patients$.subscribe(patients => this.patients = patients));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  getPersons(): (Doctor | Patient)[] {
    return [...this.doctors, ...this.patients];
  }

  isDoctor(person: Doctor | Patient): boolean {
    return 'specialityId' in person;
  }

  getSpecialityName(person: Doctor | Patient): string {
    return (person as Doctor).speciality.name;
  }

  openEditUserDialog(person: Doctor | Patient): void {
    this.dialogRef = this.dialog.open(EditUserDialogComponent, {
      width: '600px',
      data: person
    });
  }
}
