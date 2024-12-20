import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ErrorHandlerService {
  private _errorMessageSubject = new BehaviorSubject<string | null>(null);
  errorMessage$ = this._errorMessageSubject.asObservable();

  constructor() {}

  setErrorMessage(message: string) {
    this._errorMessageSubject.next(message);
  }

  clearErrorMessage() {
    this._errorMessageSubject.next(null);
  }
}
