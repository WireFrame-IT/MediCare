import { Component, computed, Inject, OnInit } from '@angular/core';
import { MedicamentUnit } from '../../enums/medicament-unit';
import { MedicamentType } from '../../enums/medicament-type';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatOptionModule } from '@angular/material/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AppointmentService } from '../../services/appointment.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { LoadingService } from '../../services/loading.service';
import { PrescriptionMedicamentRequestDTO } from '../../DTOs/request/prescription-medicament-request.dto';
import { PrescriptionMedicament } from '../../DTOs/models/prescription-medicament';

@Component({
  selector: 'app-medicament-dialog',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatOptionModule,
    ReactiveFormsModule
  ],
  templateUrl: './medicament-dialog.component.html',
  styleUrl: './medicament-dialog.component.scss'
})
export class MedicamentDialogComponent implements OnInit {
  medicaments = computed(() => this.appointmentService.medicaments());

  medicamentUnitOptions: { label: string, value: MedicamentUnit }[] = [];
  medicamentForm: FormGroup;

  constructor(
    private appointmentService: AppointmentService,
    private loadingService: LoadingService,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<MedicamentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public appointmentId: number
  ) {
    this.medicamentForm = this.fb.group({
      medicamentId: ['', Validators.required],
      quantity: [1, Validators.required],
      medicamentUnit: ['', Validators.required],
      dosage: ['', Validators.required],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.appointmentService.loadMedicaments();

    this.medicamentUnitOptions = Object.values(MedicamentUnit).filter(value => typeof value === 'number').map((value) => ({
      label: this.appointmentService.getMedicamentUnitName(value as MedicamentUnit),
      value: value
    }));
  }

  onSubmit(): void {
    if (this.medicamentForm.valid) {
      const prescriptionMedicament = new PrescriptionMedicamentRequestDTO (
        this.medicamentForm.value.dosage,
        this.medicamentForm.value.quantity,
        this.appointmentId,
        this.medicamentForm.value.medicamentUnit,
        this.medicamentForm.value.medicamentId,
        this.medicamentForm.value.notes
      );

      this.loadingService.show();
      this.appointmentService.savePrescriptionMedicament(prescriptionMedicament).subscribe({
        next: (response: PrescriptionMedicament) => {
          this.dialogRef.close(response);
          this.loadingService.hide();
          this.loadingService.showMessage('Medicament has been added.');
        },
        error: (error) => {
          this.loadingService.hide();
          this.loadingService.showErrorMessage(this.loadingService.extractErrorMessage(error));
        }
      });
    }
  }

  getMedicamentTypeName = (type: MedicamentType): string => this.appointmentService.getMedicamentTypeName(type);

  closeDialog = (): void => this.dialogRef.close();
}
