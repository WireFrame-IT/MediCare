import { Component, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { PatientRegisterRequestDTO } from '../../DTOs/request/patient-register-request.dto';
import { RefreshResponseDTO } from '../../DTOs/response/refresh-response.dto';
import { Subscription } from 'rxjs';
import { ErrorHandlerService } from '../../services/error-handler.service';
import { ErrorMessageComponent } from '../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-register-page',
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    ErrorMessageComponent,
    MatDatepickerModule
  ],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss'
})
export class RegisterPageComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  public registerForm: FormGroup;
  public errorMessage: string | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private errorHandlerService: ErrorHandlerService
  ) {
    this.registerForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      surname: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],
      password: ['', [Validators.required, Validators.maxLength(256)]],
      pesel: ['', [Validators.required, Validators.minLength(11), Validators.maxLength(11)]],
      phoneNumber: ['', [Validators.required, Validators.maxLength(15)]],
      birthDate: ['', [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.subscriptions.push(this.errorHandlerService.errorMessage$.subscribe(msg => this.errorMessage = msg));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  register() {
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

      this.authService.register(registerRequest).subscribe({
        next: (response: RefreshResponseDTO) => {
          this.errorHandlerService.clearErrorMessage();
          this.authService.storeUserData(response.accessToken, response.refreshToken);
          this.router.navigate(['/']);
        },
        error: (error) => {
          this.errorHandlerService.setErrorMessage(this.errorHandlerService.extractErrorMessage(error));
          console.error(error);
        }
      });
    } else {
      this.errorHandlerService.setErrorMessage('Please fill in all fields correctly.');
    }
  }
}
