import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SuccessDialogService {
  private _messageSubject = new BehaviorSubject<string>('');
  public message$ = this._messageSubject.asObservable();

  showMessage(message: string) {
    this._messageSubject.next(message);
    setTimeout(() => this._messageSubject.next(''), 3000);
  }
}
