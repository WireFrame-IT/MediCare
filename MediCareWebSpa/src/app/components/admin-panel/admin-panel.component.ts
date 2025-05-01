import { Component, effect, OnInit } from '@angular/core';
import { AppointmentService } from '../../services/appointment.service';
import { Doctor } from '../../DTOs/models/doctor';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { EditUserDialogComponent } from '../edit-user-dialog/edit-user-dialog.component';
import { Patient } from '../../DTOs/models/patient';
import { AuthService } from '../../services/auth.service';
import { Permission } from '../../DTOs/models/permission';
import { MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RoleType } from '../../enums/role-type';
import { RolePermissionRequest } from '../../DTOs/request/role-permission-request.dto';
import { Clipboard } from '@angular/cdk/clipboard';
import { LoadingService } from '../../services/loading.service';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-admin-panel',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatCheckboxModule,
    MatIcon
  ],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.scss'
})
export class AdminPanelComponent implements OnInit {
  private dialogRef: MatDialogRef<EditUserDialogComponent> | null = null;
  private doctors: Doctor[] = [];
  private patients: Patient[] = [];
  private doctorsEffect = effect(() => this.doctors = this.appointmentService.doctors());
  private patientsEffect = effect(() => this.patients = this.appointmentService.patients());
  private permissionsEffect = effect(() => this.permissions = this.authService.permissions());


  permissions: Permission[] = [];
  displayedColumns: string[] = [];
  roleTypes = [
    { label: RoleType[RoleType.Patient], value: RoleType.Patient },
    { label: RoleType[RoleType.Doctor], value: RoleType.Doctor }
  ];

  constructor(
      private appointmentService: AppointmentService,
      private authService: AuthService,
      private loadingService: LoadingService,
      private dialog: MatDialog,
      private clipboard: Clipboard,
      private snackBar: MatSnackBar
    ) {}

  ngOnInit(): void {
    this.appointmentService.loadDoctors();
    this.appointmentService.loadPatients();
    this.authService.loadPermissions();

    this.displayedColumns = ['description', ...this.roleTypes.map(x => x.label.toLowerCase())];
  }

  getPersons = (): (Doctor | Patient)[] => [...this.doctors, ...this.patients];

  isDoctor = (person: Doctor | Patient): boolean => 'specialityId' in person;

  getSpecialityName = (person: Doctor | Patient): string => (person as Doctor).speciality.name;

  hasPermission = (permission: Permission, roleType: RoleType): boolean => permission.permissionRoles.some(p => p.role.roleType === roleType);

  copyEmail(email: string): void {
    this.clipboard.copy(email);
    this.snackBar.open('Email copied to clipboard.', 'Close', {
      duration: 2000,
    });
  }

  togglePermission(permission: Permission, roleType: RoleType, checked: boolean): void {
    this.loadingService.show();

    if (checked) {
      this.authService.addRolePermission(new RolePermissionRequest(roleType, permission.id)).subscribe({
        next: () => {
          this.authService.loadPermissions();
          this.loadingService.hide();
          this.loadingService.showMessage(`Permission to \"${permission.description}\" granted for the role of ${RoleType[roleType]}.`);
        },
        error: error => {
          this.loadingService.hide();
          this.loadingService.showErrorMessage(this.loadingService.extractErrorMessage(error));
        }
      });
    } else {
      this.authService.removeRolePermission(roleType, permission.id).subscribe({
        next: () => {
          this.authService.loadPermissions();
          this.loadingService.hide();
          this.loadingService.showMessage(`Permission to \"${permission.description}\" revoked for the role of ${RoleType[roleType]}.`);
        },
        error: error => {
          this.loadingService.hide();
          this.loadingService.showErrorMessage(this.loadingService.extractErrorMessage(error));
        }
      });
    }
  }

  isPermissionDisabled = (permission: Permission, roleType: RoleType): boolean =>  roleType === RoleType.Doctor ? permission.patientOnly : permission.doctorOnly;

  openEditUserDialog(person: Doctor | Patient): void {
    this.dialogRef = this.dialog.open(EditUserDialogComponent, {
      width: '500px',
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
