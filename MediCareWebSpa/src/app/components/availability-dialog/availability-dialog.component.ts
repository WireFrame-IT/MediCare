import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTimepickerModule } from '@angular/material/timepicker';
import { DoctorsAvailabilityRequest } from '../../DTOs/request/doctors-availability-request.dto';
import { LoadingService } from '../../services/loading.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-availability-dialog',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatTimepickerModule,
    ReactiveFormsModule
  ],
  templateUrl: './availability-dialog.component.html',
  styleUrl: './availability-dialog.component.scss'
})
export class AvailabilityDialogComponent {
  availabilityForm: FormGroup;
  minDate: Date = new Date();

  constructor(
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private authService: AuthService,
    public dialogRef: MatDialogRef<AvailabilityDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DoctorsAvailabilityRequest
  ) {
    this.availabilityForm = this.fb.group({
      dateFrom: [data?.from ? new Date(data.from) : '', Validators.required],
      timeFrom: [data?.from ? new Date(data.from) : '', Validators.required],
      dateTo: [data?.to ? new Date(data.to) : '', Validators.required],
      timeTo: [data?.to ? new Date(data.to) : '', Validators.required]
    });
  }

   onSubmit() {
    if (this.availabilityForm.valid) {
      const availability = new DoctorsAvailabilityRequest (
        this.data?.id,
        this.availabilityForm.value.dateFrom,
        this.availabilityForm.value.dateTo
      );

      availability.from.setHours(this.availabilityForm.value.timeFrom.getHours(), this.availabilityForm.value.timeFrom.getMinutes());
      availability.to.setHours(this.availabilityForm.value.timeTo.getHours(), this.availabilityForm.value.timeTo.getMinutes());

      this.loadingService.show();
      this.authService.saveDoctorsAvailability(availability).subscribe({
        next: () => {
          this.dialogRef.close();
          this.loadingService.hide();
          this.loadingService.showMessage('Availability has been saved successfully.');
        },
        error: (error) => {
          this.loadingService.hide();
          this.loadingService.showErrorMessage(this.loadingService.extractErrorMessage(error));
        }
      });
    }
  }

  closeDialog = (): void => this.dialogRef.close();

  dateFilter(d: Date | null): boolean {
    const day = (d || new Date()).getDay();
    return day !== 0 && day !== 6;
  };

  getMinDateTo = () => this.availabilityForm.value.dateFrom;
}
