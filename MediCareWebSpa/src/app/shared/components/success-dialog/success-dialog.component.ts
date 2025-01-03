import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { Subscription } from 'rxjs';
import { LoadingService } from '../../../services/loading.service';

@Component({
  selector: 'app-success-dialog',
  imports: [ MatIcon ],
  templateUrl: './success-dialog.component.html',
  styleUrl: './success-dialog.component.scss'
})
export class SuccessDialogComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  public message: string | null = null;

  constructor(private loadingService: LoadingService) {}

  ngOnInit(): void {
    this.subscriptions.push(this.loadingService.message$.subscribe(message => {
      this.message = message;
    }));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
