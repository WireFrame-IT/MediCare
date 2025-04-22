import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  private readonly DURATION_IN_MILLISECNDS = 3000;

  private _isLoading = new BehaviorSubject<boolean>(false);
  private _messageSubject = new BehaviorSubject<string | null>(null);
  private _errorMessageSubject = new BehaviorSubject<string | null>(null);
  private _showingMessage = new BehaviorSubject<boolean>(false);
  private messageQueue: { message: string, error: boolean }[] = [];

  isLoading$ = this._isLoading.asObservable();
  message$ = this._messageSubject.asObservable();
  errorMessage$ = this._errorMessageSubject.asObservable();
  showingMessage$ = this._showingMessage.asObservable();

  show() {
    this._isLoading.next(true);
  }

  hide() {
    this._isLoading.next(false);

    if (!this._showingMessage.value)
      this.displayNextMessage();
  }

  private displayNextMessage() {
    if (this.messageQueue.length > 0) {
      this._showingMessage.next(true);
      const nextMessage = this.messageQueue.shift()!;

      if (nextMessage.error)
        this._errorMessageSubject.next(nextMessage.message);
      else
        this._messageSubject.next(nextMessage.message);

      setTimeout(() => {
        this._messageSubject.next(null);
        this._errorMessageSubject.next(null);
        this._showingMessage.next(false);
        this.displayNextMessage();
      }, this.DURATION_IN_MILLISECNDS);
    }
  }

  showMessage(message: string) {
    if (this._isLoading.value || this._showingMessage.value) {
      this.messageQueue.push({message: message, error: false});
    } else {
      this._showingMessage.next(true);
      this._messageSubject.next(message);
      setTimeout(() => {
        this._messageSubject.next(null);
        this._showingMessage.next(false);
        this.displayNextMessage();
      }, this.DURATION_IN_MILLISECNDS);
    }
  }

  showErrorMessage(message: string) {
    if (this._isLoading.value || this._showingMessage.value) {
      this.messageQueue.push({message: message, error: true});
    } else {
      this._showingMessage.next(true);
      this._errorMessageSubject.next(message);
      setTimeout(() => {
        this._errorMessageSubject.next(null);
        this._showingMessage.next(false);
        this.displayNextMessage();
      }, this.DURATION_IN_MILLISECNDS);
    }
  }

  extractErrorMessage(error: any): string {
    if (error?.status === 0) {
      return 'Could not connect to the server. Please check your connection or try again later.';
    }

    if (error?.status === 401) {
      return 'You are not authorized. Please log in and try again.';
    }

    if (typeof error?.error === 'string') {
      return error.error;
    }

    if (typeof error?.error?.message === 'string') {
      return error.error.message;
    }

    if (typeof error?.message === 'string') {
      return error.message;
    }

    return 'Something went wrong. Please try again later.';
  }
}
