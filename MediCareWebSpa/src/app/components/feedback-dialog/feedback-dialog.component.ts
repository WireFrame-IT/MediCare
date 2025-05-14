import { Component, Inject } from '@angular/core';
import { Feedback } from '../../DTOs/models/feedback';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppointmentService } from '../../services/appointment.service';
import { LoadingService } from '../../services/loading.service';
import { AuthService } from '../../services/auth.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { FeedbackRequestDTO } from '../../DTOs/request/feedback-request.dto';

@Component({
  selector: 'app-feedback-dialog',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './feedback-dialog.component.html',
  styleUrl: './feedback-dialog.component.scss'
})
export class FeedbackDialogComponent {
  feedbackForm: FormGroup;

  stars: number[] = [1, 2, 3, 4, 5];
  rating: number = 0;
  hovered: number = 0;

  constructor(
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private authService: AuthService,
    private appointmentService: AppointmentService,
    public dialogRef: MatDialogRef<FeedbackDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      feedback: Feedback | undefined,
      appointmentId: number,
      isThisPatient: boolean
    }
  ) {
    this.rating = data.feedback?.rate ?? 0;

    this.feedbackForm = this.fb.group({
      rate: [ data.feedback?.rate || '', Validators.required],
      description: [ data.feedback?.description || '', Validators.required]
    });
  }

  setHovered = (value: number) => this.hovered = value;

  clearHovered = () => this.hovered = 0;

  closeDialog = (): void => this.dialogRef.close();

  setRating(value: number) {
    this.rating = value;
    this.feedbackForm.get('rate')?.setValue(value);
  }

  onSubmit(): void {
    if (this.feedbackForm.valid) {
      const feedbackDTO = new FeedbackRequestDTO(
        this.feedbackForm.value.rate,
        this.feedbackForm.value.description,
        this.data.appointmentId
      );

      this.loadingService.show();
      this.appointmentService.saveFeedback(feedbackDTO).subscribe({
        next: (feedback: Feedback) => {
          this.dialogRef.close(feedback);
          this.loadingService.hide();
          this.loadingService.showMessage('Opinia została zapisana.');
        },
        error: (error) => {
          this.loadingService.hide();
          this.loadingService.showErrorMessage(this.loadingService.extractErrorMessage(error));
        }
      });
    }
  }
}
