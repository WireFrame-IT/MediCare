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
import { PrescriptionRequestDTO } from '../../DTOs/request/prescription-request.dto';
import { LoadingService } from '../../services/loading.service';
import { Prescription } from '../../DTOs/models/prescription';

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
    private loadingService: LoadingService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<PrescriptionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { prescription: Prescription | undefined, appointmentId: number }
  ) {
    this.prescriptionForm = this.fb.group({
      description: [ data.prescription ? data.prescription.description : '', Validators.required],
      expirationDate: [ data.prescription ? new Date(data.prescription.expirationDate) : '', Validators.required]
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
      const prescription = new PrescriptionRequestDTO(
        this.data.appointmentId,
        this.prescriptionForm.value.description,
        this.prescriptionForm.value.expirationDate
      );

      this.loadingService.show();
      this.appointmentService.savePrescription(prescription).subscribe({
        next: (prescription: Prescription) => {
          if (this.data.prescription)
            this.dialogRef.close();

          this.data.prescription = prescription;

          this.loadingService.hide();
          this.loadingService.showMessage('Prescription has been saved.');
        },
        error: (error) => {
          this.loadingService.hide();
          this.loadingService.showErrorMessage(this.loadingService.extractErrorMessage(error));
        }
      });
    }
  }

  addMedicament(): void {

  }

  removeMedicament(event: MouseEvent, prescriptionMedicament: PrescriptionMedicament): void {
    event.stopPropagation();
    this.loadingService.show();

    this.appointmentService.removePrescriptionMedicament(prescriptionMedicament.prescriptionId, prescriptionMedicament.medicamentId).subscribe({
      next: () => {
        this.data.prescription!.prescriptionMedicaments = this.data.prescription!.prescriptionMedicaments
          .filter(x => !(x.prescriptionId === prescriptionMedicament.prescriptionId && x.medicamentId === prescriptionMedicament.medicamentId))
        this.loadingService.hide();
        this.loadingService.showMessage('Medicament has been removed.');
      },
      error: error => {
        this.loadingService.hide();
        this.loadingService.showErrorMessage(this.loadingService.extractErrorMessage(error));
      }
    });;
  }

  closeDialog = (): void => this.dialogRef.close();

  getMedicamentTypeName = (type: MedicamentType): string => MedicamentType[type].replace(/([a-z])([A-Z])/g, '$1 $2');

  getMedicamentUnitName = (unit: MedicamentUnit): string => MedicamentUnit[unit].replace(/([a-z])([A-Z])/g, '$1 $2');
}
