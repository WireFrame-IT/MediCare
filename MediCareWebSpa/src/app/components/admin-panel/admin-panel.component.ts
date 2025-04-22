import { Component, OnDestroy, OnInit } from '@angular/core';
import { AppointmentService } from '../../services/appointment.service';
import { Doctor } from '../../DTOs/models/doctor';
import { MatCardModule } from '@angular/material/card';
import { Subscription } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { EditUserDialogComponent } from '../edit-user-dialog/edit-user-dialog.component';
import { Patient } from '../../DTOs/models/patient';
import { AuthService } from '../../services/auth.service';
import { Permission } from '../../DTOs/models/permission';
import { MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { RoleType } from '../../enums/role-type';

@Component({
  selector: 'app-admin-panel',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatCheckboxModule
  ],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.scss'
})
export class AdminPanelComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  private dialogRef: MatDialogRef<EditUserDialogComponent> | null = null;
  private doctors: Doctor[] = [];
  private patients: Patient[] = [];

  permissions: Permission[] = [];
  displayedColumns: string[] = [];
  roleTypes = [
    { label: RoleType[RoleType.Patient], value: RoleType.Patient },
    { label: RoleType[RoleType.Doctor], value: RoleType.Doctor }
  ];

  constructor(
      private appointmentService: AppointmentService,
      private authService: AuthService,
      private dialog: MatDialog
    ) {}

  ngOnInit(): void {
    this.appointmentService.loadDoctors();
    this.appointmentService.loadPatients();
    this.authService.loadPermissions();

    this.subscriptions.push(this.appointmentService.doctors$.subscribe(doctors => this.doctors = doctors));
    this.subscriptions.push(this.appointmentService.patients$.subscribe(patients => this.patients = patients));
    this.subscriptions.push(this.authService.permissions$.subscribe(permissions => this.permissions = permissions));

    this.displayedColumns = ['description', ...this.roleTypes.map(x => x.label.toLowerCase())];
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  getPersons = (): (Doctor | Patient)[] => [...this.doctors, ...this.patients];

  isDoctor = (person: Doctor | Patient): boolean => 'specialityId' in person;

  getSpecialityName = (person: Doctor | Patient): string => (person as Doctor).speciality.name;

  hasPermission = (permission: Permission, roleType: RoleType): boolean => permission.permissionRoles.some(p => p.role.roleType === roleType);

  togglePermission(permission: Permission, role: RoleType, checked: boolean): void {
    // if (checked) {
    //   permission.permissionRoles.push(new RolePermission(role));
    // } else {
    //   permission.permissionRoles = permission.permissionRoles.filter(p => p.role !== role);
    // }
  }

  openEditUserDialog(person: Doctor | Patient): void {
    this.dialogRef = this.dialog.open(EditUserDialogComponent, {
      width: '600px',
      data: person
    });

    this.dialogRef.afterClosed().subscribe((updatedPerson: Doctor | Patient) => {
      if (updatedPerson) {
        const index = this.isDoctor(person)
          ? this.doctors.findIndex(x => x.userId === updatedPerson.userId)
          : this.patients.findIndex(x => x.userId === updatedPerson.userId);

        if (index !== -1) {
          this.isDoctor(person)
            ? this.doctors[index] = updatedPerson as Doctor
            : this.patients[index] = updatedPerson as Patient;
        }
      }
    });
  }
}
