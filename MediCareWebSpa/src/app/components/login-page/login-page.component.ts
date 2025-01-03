import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { LoginResponseDTO } from '../../DTOs/response/login-response.dto';
import { Subscription } from 'rxjs';
import { LoadingService } from '../../services/loading.service';

@Component({
  selector: 'app-login-page',
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule
  ],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss'
})
export class LoginPageComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  public loginForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private loadingService: LoadingService,
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  ngOnInit(): void { }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  login() {
    if (this.loginForm.valid) {
      const loginRequest = { ...this.loginForm.value };
      this.loadingService.show();

      this.authService.login(loginRequest).subscribe({
        next: (response: LoginResponseDTO) => {
          this.loadingService.clearErrorMessage();
          this.loadingService.showMessage('Logged in successfully');
          this.authService.storeUserData(response.accessToken, response.refreshToken, response.roleType);
          this.router.navigate(['/']);
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
    else {
      this.loadingService.setErrorMessage('Please fill in all fields correctly');
    }
  }
}
