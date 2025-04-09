import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { LoadingService } from '../../services/loading.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Doctor } from '../../DTOs/models/doctor';
import { Subscription } from 'rxjs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { UserRequestDTO } from '../../DTOs/request/user-request.dto';
import { Speciality } from '../../DTOs/models/speciality';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { Patient } from '../../DTOs/models/patient';

@Component({
  selector: 'app-edit-user-dialog',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatOptionModule,
    ReactiveFormsModule
  ],
  templateUrl: './edit-user-dialog.component.html',
  styleUrl: './edit-user-dialog.component.scss'
})
export class EditUserDialogComponent implements OnInit, OnDestroy {
  userForm: FormGroup;
  specialities: Speciality[] = [];
  isDoctor: boolean;
  private subscriptions: Subscription[] = [];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private loadingService: LoadingService,
    public dialogRef: MatDialogRef<EditUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Doctor | Patient
  ) {
    if ('specialityId' in data) {
      this.isDoctor = true;
      this.userForm = this.fb.group({
        specialityId: [data.specialityId, Validators.required]
      });
    } else {
      this.isDoctor = false;
      this.userForm = this.fb.group({

      });
    }
  }

  ngOnInit(): void {
    this.authService.loadSpecialities();
    this.subscriptions.push(this.authService.specialities$.subscribe(specialities => this.specialities = specialities));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  getFullName(): string {
    return `${this.data.user.name} ${this.data.user.surname}`;
  }

  onSubmit() {
    if (this.userForm.valid) {
      const user = new UserRequestDTO (
        this.userForm.value.speciality
      );
      this.loadingService.show();
      this.dialogRef.close();
      this.authService.saveUser(user).subscribe({
        next: (response: boolean) => {
          this.loadingService.clearErrorMessage();
          this.loadingService.showMessage('User saved successfully');
        },
        error: (error) => {
          this.loadingService.setErrorMessage(this.loadingService.extractErrorMessage(error));
          console.error(error);
          this.loadingService.hide();
        },
        complete: () => {
          this.loadingService.hide();
        }
      });
    }
  }
}
