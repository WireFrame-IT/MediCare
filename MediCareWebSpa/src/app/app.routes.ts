import { Routes } from '@angular/router';
import { HomePageComponent } from './components/home-page/home-page.component';
import { LoginPageComponent } from './components/login-page/login-page.component';
import { RegisterPageComponent } from './components/register-page/register-page.component';
import { UserRegisterPageComponent } from './components/user-register-page/user-register-page.component';
import { ServicePageComponent } from './components/service-page/service-page.component';
import { AppointmentPageComponent } from './components/appointment-page/appointment-page.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { AboutPageComponent } from './components/about-page/about-page.component';
import { AvailabilityPageComponent } from './components/availability-page/availability-page.component';
import { authGuard } from './shared/guards/auth.guard';

export const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'login', component: LoginPageComponent },
  { path: 'register', component: RegisterPageComponent },
  { path: 'register-user', component: UserRegisterPageComponent },
  { path: 'services', component: ServicePageComponent },
  { path: 'appointments', component: AppointmentPageComponent },
  { path: 'admin-panel', component: AdminPanelComponent },
  { path: 'about-page', component: AboutPageComponent },
  { path: 'availability-page', component: AvailabilityPageComponent },
  { path: 'appointments', canActivate: [authGuard], loadComponent: () => import('./components/appointment-page/appointment-page.component').then(x => x.AppointmentPageComponent)}
];
