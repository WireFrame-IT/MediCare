import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatInputModule } from '@angular/material/input';
import { AppointmentService } from '../../services/appointment.service';
import { Subscription } from 'rxjs';
import { Medicament } from '../../DTOs/models/medicament';
import { PrescriptionMedicament } from '../../DTOs/models/prescription-medicament';
import { MedicamentType } from '../../enums/medicament-type';
import { MedicamentUnit } from '../../enums/medicament-unit';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-prescription-dialog',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatIconModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './prescription-dialog.component.html',
  styleUrl: './prescription-dialog.component.scss'
})
export class PrescriptionDialogComponent implements OnInit, OnDestroy {
  prescriptionForm: FormGroup;
  subscriptions: Subscription[] = [];
  medicaments: Medicament[] = [];
  isDoctor: boolean = false;
  minDate: Date = new Date();

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<PrescriptionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public prescriptionMedicaments: PrescriptionMedicament[]
  ) {
    this.prescriptionForm = this.fb.group({
      description: [ prescriptionMedicaments.length ? prescriptionMedicaments[0].prescription.description : '', Validators.required],
      expirationDate: [ prescriptionMedicaments.length ? new Date(prescriptionMedicaments[0].prescription.expirationDate) : '', Validators.required]
    });
  }

  private createMedicament(): FormGroup {
    return this.fb.group({
      medicamentId: ['', Validators.required],
      dosage: ['', Validators.required],
      quantity: ['', [Validators.required]],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.appointmentService.loadMedicaments();

    this.subscriptions.push(this.authService.isDoctor$.subscribe(isDoctor => this.isDoctor = isDoctor));
    this.subscriptions.push(this.appointmentService.medicaments$.subscribe(medicaments => this.medicaments = medicaments));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  onSubmit(): void {
    if (this.prescriptionForm.valid) {

    }
  }

  addMedicament(): void {

  }

  removeMedicament(event: MouseEvent, prescriptionMedicament: PrescriptionMedicament): void {
    event.stopPropagation();

  }

  closeDialog = (): void => this.dialogRef.close();

  getMedicamentTypeName = (type: MedicamentType): string => MedicamentType[type];

  getMedicamentUnitName = (unit: MedicamentUnit): string => MedicamentUnit[unit];
}
