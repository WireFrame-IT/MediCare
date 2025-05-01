import { Component, effect, OnDestroy, OnInit } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from './services/auth.service';
import { Subscription } from 'rxjs';
import { LoadingService } from './services/loading.service';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    RouterModule,
    MatToolbarModule,
    MatButtonModule,
    MatProgressBarModule,
    MatTooltipModule,
    MatIcon
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit, OnDestroy{
  private effect = effect(() => {
    this.isAdmin = this.authService.isAdmin();
    this.isDoctor = this.authService.isDoctor();
    this.isLoggedIn = this.authService.isLoggedIn();
  });

  private subscriptions: Subscription[] = [];
  isAdmin: boolean = false;
  isDoctor: boolean = false;
  isLoggedIn: boolean = false;
  isLoading: boolean = false;
  message: string | null = null;
  errorMessage: string | null = null;

  constructor(
    private authService: AuthService,
    private loadingService: LoadingService,
  ) {}

  ngOnInit(): void {
    this.authService.retrieveCredentials();

    this.subscriptions.push(this.loadingService.isLoading$.subscribe(isLoading => this.isLoading = isLoading));
    this.subscriptions.push(this.loadingService.message$.subscribe(message => this.message = message));
    this.subscriptions.push(this.loadingService.errorMessage$.subscribe(errorMessage => this.errorMessage = errorMessage));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  logout = () => this.authService.logout();

  getUserNameAndSurname = (): string => this.authService.getUserNameAndSurname();
}
