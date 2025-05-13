import { Component, computed, effect, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatInputModule } from '@angular/material/input';
import { AppointmentService } from '../../services/appointment.service';
import { PrescriptionMedicament } from '../../DTOs/models/prescription-medicament';
import { MedicamentType } from '../../enums/medicament-type';
import { MedicamentUnit } from '../../enums/medicament-unit';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { AuthService } from '../../services/auth.service';
import { PrescriptionRequestDTO } from '../../DTOs/request/prescription-request.dto';
import { LoadingService } from '../../services/loading.service';
import { Prescription } from '../../DTOs/models/prescription';
import { MedicamentDialogComponent } from '../medicament-dialog/medicament-dialog.component';

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
export class PrescriptionDialogComponent {
  private medicamentDialogRef: MatDialogRef<MedicamentDialogComponent> | null = null;

  isDoctor = computed(() => this.authService.isDoctor());

  prescriptionForm: FormGroup;
  minDate: Date = new Date();

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
    private loadingService: LoadingService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private dialogRef: MatDialogRef<PrescriptionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { prescription: Prescription | undefined, appointmentId: number }
  ) {
    this.prescriptionForm = this.fb.group({
      description: [ data.prescription ? data.prescription.description : '', Validators.required],
      expirationDate: [ data.prescription ? new Date(data.prescription.expirationDate) : '', Validators.required]
    });

    effect(() => {
      if(!this.authService.isLoggedIn())
        this.medicamentDialogRef?.close();
    });
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

  openAddMedicamentDialog(appointmentId: number): void {
    this.medicamentDialogRef = this.dialog.open(MedicamentDialogComponent, {
      width: '500px',
      data: appointmentId,
      autoFocus: false
    });

    this.medicamentDialogRef.afterClosed().subscribe((prescriptionMedicament) => {
      if (prescriptionMedicament)
        this.data.prescription?.prescriptionMedicaments.push(prescriptionMedicament);
    });
  }

  removeMedicament(event: MouseEvent, prescriptionMedicament: PrescriptionMedicament): void {
    event.stopPropagation();
    this.loadingService.show();

    this.appointmentService.removePrescriptionMedicament(prescriptionMedicament.prescriptionAppointmentId, prescriptionMedicament.medicamentId).subscribe({
      next: () => {
        this.data.prescription!.prescriptionMedicaments = this.data.prescription!.prescriptionMedicaments
          .filter(x => !(x.prescriptionAppointmentId === prescriptionMedicament.prescriptionAppointmentId && x.medicamentId === prescriptionMedicament.medicamentId))
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

  getMedicamentTypeName = (type: MedicamentType): string => this.appointmentService.getMedicamentTypeName(type);

  getMedicamentUnitName = (unit: MedicamentUnit): string => this.appointmentService.getMedicamentUnitName(unit);
}
