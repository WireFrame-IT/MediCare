import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from './services/auth.service';
import { Subscription } from 'rxjs';
import { LoadingService } from './services/loading.service';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    RouterModule,
    MatToolbarModule,
    MatButtonModule,
    MatProgressBarModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit, OnDestroy{
  private subscriptions: Subscription[] = [];
  public isAdmin: boolean = false;
  public isLoggedIn: boolean = false;
  public isLoading: boolean = false;

  constructor(private authService: AuthService, private loadingService: LoadingService) {}

  ngOnInit(): void {
    this.subscriptions.push(this.authService.isAdmin$.subscribe(isAdmin => this.isAdmin = isAdmin));
    this.subscriptions.push(this.authService.isLoggedIn$.subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn));
    this.subscriptions.push(this.loadingService.isLoading$.subscribe(isLoading => this.isLoading = isLoading));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  logout(): void {
    this.authService.logout();
  }
}
