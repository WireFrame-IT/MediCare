import { Component, OnDestroy } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { LoginRequestDTO } from '../../DTOs/request/login-request.dto';
import { LoginResponseDTO } from '../../DTOs/response/login-response.dto';
import { ErrorHandlerService } from '../../services/error-handler.service';
import { ErrorMessageComponent } from '../../shared/error-message/error-message.component';
import { AsyncPipe } from '@angular/common';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule, MatInputModule, MatButtonModule, MatFormFieldModule, ErrorMessageComponent, AsyncPipe],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss'
})
export class LoginPageComponent implements OnDestroy {
  private subscriptions: Subscription[] = [];
  public loginForm: FormGroup;

  constructor(private fb: FormBuilder, private router: Router, private authService: AuthService, public errorHandlerService: ErrorHandlerService) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  login() {
    if (this.loginForm.valid) {
      const loginRequest = new LoginRequestDTO(
        this.loginForm.value.email,
        this.loginForm.value.password,
      );

      this.subscriptions.push(this.authService.login(loginRequest).subscribe({
        next: (response: LoginResponseDTO) => {
          this.errorHandlerService.clearErrorMessage();
          this.authService.storeUserData(response.accessToken, response.refreshToken, response.roleType);
          this.router.navigate(['/']);
        },
        error: (error) => {
          this.errorHandlerService.setErrorMessage('Something went wrong, please try again.');
          console.error(error);
        }
      }));
    }
    else {
      this.errorHandlerService.setErrorMessage('Please fill in all fields correctly.');
    }
  }
}
