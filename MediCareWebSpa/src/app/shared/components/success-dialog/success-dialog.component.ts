import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { SuccessDialogService } from '../../../services/success-dialog.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-success-dialog',
  imports: [ MatIcon ],
  templateUrl: './success-dialog.component.html',
  styleUrl: './success-dialog.component.scss'
})
export class SuccessDialogComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  public message: string = '';

  constructor(private successDialogService: SuccessDialogService) {}

  ngOnInit(): void {
    this.subscriptions.push(this.successDialogService.message$.subscribe(message => {
      this.message = message;
    }));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
