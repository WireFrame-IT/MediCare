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

  extractErrorMessage(error: any): string | null {
    if (!error) return null;
    if (typeof error.error === 'string') {
      const match = error.error.match(/ValidationException:\s(.+?)\r\n/);
      return match ? match[1] : error.error;
    } else if (error.error && error.error.message) {
      return error.error.message;
    } else if (error.message) {
      return error.message;
    }
    return null;
  }
}
