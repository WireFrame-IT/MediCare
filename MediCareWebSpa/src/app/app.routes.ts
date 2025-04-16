import { Routes } from '@angular/router';
import { HomePageComponent } from './components/home-page/home-page.component';
import { LoginPageComponent } from './components/login-page/login-page.component';
import { RegisterPageComponent } from './components/register-page/register-page.component';
import { DoctorRegisterPageComponent } from './components/doctor-register-page/doctor-register-page.component';
import { ServicePageComponent } from './components/service-page/service-page.component';
import { AppointmentPageComponent } from './components/appointment-page/appointment-page.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { AboutPageComponent } from './components/about-page/about-page.component';

export const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'login', component: LoginPageComponent },
  { path: 'register', component: RegisterPageComponent },
  { path: 'register-doctor', component: DoctorRegisterPageComponent },
  { path: 'services', component: ServicePageComponent },
  { path: 'appointments', component: AppointmentPageComponent },
  { path: 'admin-panel', component: AdminPanelComponent },
  { path: 'about-page', component: AboutPageComponent }
];
