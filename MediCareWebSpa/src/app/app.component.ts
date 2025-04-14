import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from './services/auth.service';
import { Subscription } from 'rxjs';
import { LoadingService } from './services/loading.service';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { SuccessDialogComponent } from './shared/components/success-dialog/success-dialog.component';
import { ErrorMessageComponent } from './shared/components/error-message/error-message.component';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    RouterModule,
    MatToolbarModule,
    MatButtonModule,
    MatProgressBarModule,
    SuccessDialogComponent,
    ErrorMessageComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit, OnDestroy{
  private subscriptions: Subscription[] = [];
  isAdmin: boolean = false;
  isLoggedIn: boolean = false;
  isLoading: boolean = false;
  errorMessage: string | null = null;

  constructor(
    private authService: AuthService,
    private loadingService: LoadingService,
  ) {}

  ngOnInit(): void {
    this.authService.retrieveCredentials();
    this.subscriptions.push(this.authService.isAdmin$.subscribe(isAdmin => this.isAdmin = isAdmin));
    this.subscriptions.push(this.authService.isLoggedIn$.subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn));
    this.subscriptions.push(this.loadingService.isLoading$.subscribe(isLoading => this.isLoading = isLoading));
    this.subscriptions.push(this.loadingService.errorMessage$.subscribe(msg => this.errorMessage = msg));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  logout(): void {
    this.authService.logout();
    this.loadingService.showMessage('Logged out successfully.');
  }
}
