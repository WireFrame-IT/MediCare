import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { PatientRegisterRequestDTO } from '../../DTOs/request/patient-register-request.dto';
import { LoadingService } from '../../services/loading.service';
import { LoginResponseDTO } from '../../DTOs/response/login-response.dto';

@Component({
  selector: 'app-register-page',
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatDatepickerModule
  ],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss'
})
export class RegisterPageComponent {
  public registerForm: FormGroup;

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
      password: ['', [Validators.required, Validators.maxLength(256), Validators.minLength(6)]],
      pesel: ['', [Validators.required, Validators.minLength(11), Validators.maxLength(11)]],
      phoneNumber: ['', [Validators.required, Validators.maxLength(15)]],
      birthDate: ['', [Validators.required]],
    });
  }

  register(): void {
    if (this.registerForm.valid) {
      const registerRequest = new PatientRegisterRequestDTO(
        this.registerForm.value.name,
        this.registerForm.value.surname,
        this.registerForm.value.email,
        this.registerForm.value.password,
        this.registerForm.value.pesel,
        this.registerForm.value.phoneNumber,
        this.registerForm.value.birthDate,
      );

      this.loadingService.show();
      this.authService.register(registerRequest).subscribe({
        next: (response: LoginResponseDTO) => {
          this.authService.storeUserData(response.userId, response.userName, response. userSurname, response.accessToken, response.refreshToken);
          this.loadingService.hide();
          this.loadingService.showMessage('Registered.');
          this.router.navigate(['/']);
        },
        error: (error) => {
          this.loadingService.hide();
          this.loadingService.showErrorMessage(this.loadingService.extractErrorMessage(error));
        }
      });
    }
  }
}
