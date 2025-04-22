import { Component, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Subscription } from 'rxjs';
import { UserRegisterRequestDTO } from '../../DTOs/request/user-register-request.dto';
import { Speciality } from '../../DTOs/models/speciality';
import { LoadingService } from '../../services/loading.service';
import { RoleType } from '../../enums/role-type';

@Component({
  selector: 'app-user-register-page',
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatSelectModule
  ],
  templateUrl: './user-register-page.component.html',
  styleUrl: './user-register-page.component.scss'
})
export class UserRegisterPageComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];

  registerForm!: FormGroup;
  specialities: Speciality[] = [];
  isDoctor: boolean = false;
  roleTypes = [
    { label: RoleType[RoleType.Patient], value: RoleType.Patient },
    { label: RoleType[RoleType.Doctor], value: RoleType.Doctor }
  ];

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private loadingService: LoadingService
  ) {
    this.buildForm(RoleType.Patient);
  }

  ngOnInit(): void {
    this.subscriptions.push(this.authService.specialities$.subscribe(specialities => this.specialities = specialities));
    this.authService.loadSpecialities();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  private buildForm(selectedRoleType: RoleType): void {
    const base = {
      name: ['', [Validators.required, Validators.maxLength(50)]],
      surname: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],
      password: ['', [Validators.required, Validators.maxLength(256)]],
      pesel: ['', [Validators.required, Validators.minLength(11), Validators.maxLength(11)]],
      phoneNumber: ['', [Validators.required, Validators.maxLength(15)]],
      roleType: [selectedRoleType || '', Validators.required]
    };

    this.registerForm = this.formBuilder.group(
      this.isDoctor
        ? {
            ...base,
            employmentDate: ['', [Validators.required]],
            specialityId: ['', [Validators.required]]
          }
        : {
            ...base,
            birthDate: ['', [Validators.required]],
          }
    );
  }

  onRoleTypeChange(event: MatSelectChange): void {
    this.isDoctor = event.value === RoleType.Doctor;
    this.buildForm(event.value);
  }

  register() {
    if (this.registerForm.valid) {
      const registerRequest = new UserRegisterRequestDTO(
        this.registerForm.value.name,
        this.registerForm.value.surname,
        this.registerForm.value.email,
        this.registerForm.value.password,
        this.registerForm.value.pesel,
        this.registerForm.value.phoneNumber,
        this.registerForm.value.roleType,
        this.isDoctor ? this.registerForm.value.employmentDate : null,
        this.isDoctor ? this.registerForm.value.specialityId : null,
        this.isDoctor ? null : this.registerForm.value.birthDate
      );

      this.loadingService.show();
      this.authService.registerUser(registerRequest).subscribe({
        next: () => {
          this.loadingService.hide();
          this.loadingService.showMessage('The user has been registered successfully.');
          this.router.navigate(['/']);
        },
        error: (error) => {
          this.loadingService.hide();
          this.loadingService.showErrorMessage(this.loadingService.extractErrorMessage(error));
        }
      });
    }
  }
}
