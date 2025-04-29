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

@Component({
  selector: 'app-prescription-dialog',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatExpansionModule,
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
  prescriptionMedicaments: PrescriptionMedicament[] = [];
  medicaments: Medicament[] = [];

  // description: string = '';
  // prescriptionMedicaments: WritableSignal<typeof PrescriptionMedicament[]> = signal([PrescriptionMedicament]);

  constructor(
    private appointmentService: AppointmentService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<PrescriptionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public appointmentId: number
  ) {
    this.prescriptionForm = this.fb.group({
      description: ['', Validators.required],
      prescriptionMedicaments: this.fb.array([this.createMedicament()])
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
    this.appointmentService.loadPrescriptionMedicaments();

    this.subscriptions.push(this.appointmentService.medicaments$.subscribe(medicaments => this.medicaments = medicaments));
    this.subscriptions.push(this.appointmentService.prescriptionMedicaments$.subscribe(prescriptionMedicaments => {
      this.prescriptionMedicaments = prescriptionMedicaments;
      this.prescriptionForm.patchValue({ description: prescriptionMedicaments.find(x => x.prescription.appointmentId === this.appointmentId)?.prescription.description ?? '' });
    }));
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

  getFilteredPrescriptionMedicaments = (): PrescriptionMedicament[] => this.prescriptionMedicaments.filter(x => x.prescription.appointmentId === this.appointmentId);

  closeDialog = (): void => this.dialogRef.close();

  getMedicamentTypeName = (type: MedicamentType): string => MedicamentType[type];

  getMedicamentUnitName = (unit: MedicamentUnit): string => MedicamentUnit[unit];
}
