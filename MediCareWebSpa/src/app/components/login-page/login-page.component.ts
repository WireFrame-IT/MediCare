import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { LoginResponseDTO } from '../../DTOs/response/login-response.dto';
import { ErrorHandlerService } from '../../services/error-handler.service';
import { ErrorMessageComponent } from '../../shared/components/error-message/error-message.component';
import { Subscription } from 'rxjs';
import { LoadingService } from '../../services/loading.service';
import { SuccessDialogService } from '../../services/success-dialog.service';

@Component({
  selector: 'app-login-page',
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    ErrorMessageComponent
  ],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss'
})
export class LoginPageComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  public loginForm: FormGroup;
  public errorMessage: string | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private errorHandlerService: ErrorHandlerService,
    private loadingService: LoadingService,
    private successDialogService: SuccessDialogService
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  ngOnInit(): void {
    this.subscriptions.push(this.errorHandlerService.errorMessage$.subscribe(msg => this.errorMessage = msg));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  login() {
    if (this.loginForm.valid) {
      const loginRequest = { ...this.loginForm.value };
      this.loadingService.show();

      this.authService.login(loginRequest).subscribe({
        next: (response: LoginResponseDTO) => {
          this.errorHandlerService.clearErrorMessage();
          this.successDialogService.showMessage('Logged in successfully.');
          this.authService.storeUserData(response.accessToken, response.refreshToken, response.roleType);
          this.router.navigate(['/']);
        },
        error: (error) => {
          this.errorHandlerService.setErrorMessage(this.errorHandlerService.extractErrorMessage(error));
          console.error(error);
          this.loadingService.hide();
        },
        complete: () => {
          this.loadingService.hide();
        }
      });
    }
    else {
      this.errorHandlerService.setErrorMessage('Please fill in all fields correctly.');
    }
  }
}
