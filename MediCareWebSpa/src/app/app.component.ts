import { Component, computed, OnInit } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from './services/auth.service';
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
export class AppComponent implements OnInit {
  isAdmin = computed(() => this.authService.isAdmin());
  isDoctor = computed(() => this.authService.isDoctor());
  isLoggedIn = computed(() => this.authService.isLoggedIn());
  isLoading = computed(() => this.loadingService.isLoading());
  message = computed(() => this.loadingService.message());
  errorMessage = computed(() => this.loadingService.errorMessage());

  constructor(
    private authService: AuthService,
    private loadingService: LoadingService,
  ) {}

  ngOnInit(): void {
    this.authService.retrieveCredentials();
  }

  logout = () => this.authService.logout();

  getUserNameAndSurname = (): string => this.authService.getUserNameAndSurname();
}
