import { Component, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Subscription } from 'rxjs';
import { DoctorRegisterRequestDTO } from '../../DTOs/request/doctor-register-request.dto';
import { Speciality } from '../../DTOs/models/speciality';
import { LoadingService } from '../../services/loading.service';

@Component({
  selector: 'app-doctor-register-page',
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatSelectModule
  ],
  templateUrl: './doctor-register-page.component.html',
  styleUrl: './doctor-register-page.component.scss'
})
export class DoctorRegisterPageComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  public registerForm: FormGroup;
  public specialities: Speciality[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private loadingService: LoadingService
  ) {
    this.registerForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      surname: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],
      password: ['', [Validators.required, Validators.maxLength(256)]],
      pesel: ['', [Validators.required, Validators.minLength(11), Validators.maxLength(11)]],
      phoneNumber: ['', [Validators.required, Validators.maxLength(15)]],
      employmentDate: ['', [Validators.required]],
      specialityId: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.subscriptions.push(this.authService.specialities$.subscribe(specialities => this.specialities = specialities));
    this.authService.loadSpecialities();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  register() {
    if (this.registerForm.valid) {
        const registerRequest = new DoctorRegisterRequestDTO(
          this.registerForm.value.name,
          this.registerForm.value.surname,
          this.registerForm.value.email,
          this.registerForm.value.password,
          this.registerForm.value.pesel,
          this.registerForm.value.phoneNumber,
          this.registerForm.value.employmentDate,
          this.registerForm.value.specialityId
        );

        this.authService.registerDoctor(registerRequest).subscribe({
          next: () => {
            this.loadingService.clearErrorMessage();
            this.loadingService.showMessage('The doctor has been registered successfully');
            this.router.navigate(['/']);
          },
          error: (error) => {
            this.loadingService.setErrorMessage(this.loadingService.extractErrorMessage(error));
            console.error(error);
          }
        });
    } else {
      this.loadingService.setErrorMessage('Please fill in all fields correctly');
    }
  }
}
