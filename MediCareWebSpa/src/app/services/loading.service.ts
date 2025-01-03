import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  private _isLoading = new BehaviorSubject<boolean>(false);
  private _messageSubject = new BehaviorSubject<string | null>(null);
  private _errorMessageSubject = new BehaviorSubject<string | null>(null);
  private errorTimeoutId: ReturnType<typeof setTimeout> | null = null;

  isLoading$ = this._isLoading.asObservable();
  message$ = this._messageSubject.asObservable();
  errorMessage$ = this._errorMessageSubject.asObservable();

  show() {
    this._isLoading.next(true);
  }

  hide() {
    this._isLoading.next(false);
  }

  showMessage(message: string) {
    this._messageSubject.next(message);
    setTimeout(() => this._messageSubject.next(null), 5000);
  }

  setErrorMessage(message: string) {
    if (this.errorTimeoutId) {
      clearTimeout(this.errorTimeoutId);
    }

    this._errorMessageSubject.next(message);
    this.errorTimeoutId = setTimeout(() => {
      this._errorMessageSubject.next(null);
      this.errorTimeoutId = null;
    }, 5000);
  }

  clearErrorMessage() {
    if (this.errorTimeoutId) {
      clearTimeout(this.errorTimeoutId);
      this.errorTimeoutId = null;
    }

    this._errorMessageSubject.next(null);
  }

  extractErrorMessage(error: any): string {
    if (error?.error && typeof error.error === 'string') {
      return error.error;
    }
    if (error?.error && error?.error?.message) {
      return error.error.message;
    }
    if (error?.message) {
      return error.message;
    }
    return 'Something went wrong.';
  }
}
