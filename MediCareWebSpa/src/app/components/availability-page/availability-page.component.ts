import { Component, effect, OnInit } from '@angular/core';
import { DoctorsAvailability } from '../../DTOs/models/doctors-availability';
import { AuthService } from '../../services/auth.service';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AvailabilityDialogComponent } from '../availability-dialog/availability-dialog.component';
import { DatePipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { LoadingService } from '../../services/loading.service';
import { DoctorsAvailabilityRequest } from '../../DTOs/request/doctors-availability-request.dto';

@Component({
  selector: 'app-availability-page',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIcon,
    DatePipe
  ],
  templateUrl: './availability-page.component.html',
  styleUrl: './availability-page.component.scss'
})
export class AvailabilityPageComponent implements OnInit {
  private dialogRef: MatDialogRef<AvailabilityDialogComponent> | null = null;
  private isAdminEffect = effect(() => this.isAdmin = this.authService.isAdmin());
  private availabilitiesEffect = effect(() => this.availabilities = this.authService.availabilities());

  availabilities: DoctorsAvailability[] = [];
  isAdmin: boolean = false;

  constructor(
    private authService: AuthService,
    private loadingService: LoadingService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.authService.loadDoctorsAvailabilities();
  }

  onRemove(availability: DoctorsAvailability): void {
    this.authService.removeDoctorsAvailability(availability.id).subscribe({
      next: () => {
        this.authService.loadDoctorsAvailabilities();
        this.loadingService.hide();
        this.loadingService.showMessage('Availability removed.');
      },
      error: error => {
        this.loadingService.hide();
        this.loadingService.showErrorMessage(this.loadingService.extractErrorMessage(error));
      }
    });
  }

  openAvailabilityDialog(availability: DoctorsAvailabilityRequest | null): void {
    this.dialogRef = this.dialog.open(AvailabilityDialogComponent, {
      width: '500px',
      data: availability,
      autoFocus: false
    });

    this.dialogRef.afterClosed().subscribe(() => {
      this.authService.loadDoctorsAvailabilities();
    });
  }
}
