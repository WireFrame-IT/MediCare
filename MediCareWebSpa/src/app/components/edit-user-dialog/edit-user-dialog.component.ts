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
import { MatDatepickerModule } from '@angular/material/datepicker';

@Component({
  selector: 'app-edit-user-dialog',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatOptionModule,
    MatDatepickerModule,
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
    const base = {
      name: [data.user.name, [Validators.required, Validators.maxLength(50)]],
      surname: [data.user.surname, [Validators.required, Validators.maxLength(50)]],
      email: [data.user.email, [Validators.required, Validators.email, Validators.maxLength(50)]],
      newPassword: ['', [Validators.maxLength(256)]],
      pesel: [data.user.pesel, [Validators.required, Validators.minLength(11), Validators.maxLength(11)]],
      phoneNumber: [data.user.phoneNumber, [Validators.required, Validators.maxLength(15)]],
    };

    if ('specialityId' in data) {
      this.isDoctor = true;
      this.userForm = this.fb.group({
        ...base,
        specialityId: [data.specialityId, [Validators.required]]
      });
    } else {
      this.isDoctor = false;
      this.userForm = this.fb.group({
        ...base,
        birthDate: [data.birthDate, [Validators.required]]
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

  onSubmit() {
    if (this.userForm.valid) {
      const user = new UserRequestDTO (
        this.userForm.value.name,
        this.userForm.value.surName,
        this.userForm.value.email,
        this.userForm.value.newPassword,
        this.userForm.value.pesel,
        this.userForm.value.phoneNumber
      );

      if (this.isDoctor) {
        user.specialityId = this.userForm.value.speciality;
      } else {
        user.birthDate = this.userForm.value.birthDate;
      }

      this.loadingService.show();
      this.authService.saveUser(user).subscribe({
        next: (response: boolean) => {
          this.loadingService.clearErrorMessage();
          this.dialogRef.close();
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
