import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  private readonly DURATION_IN_MILLISECNDS = 2000;

  isLoading = signal<boolean>(false);
  message = signal<string | null>(null);
  errorMessage = signal<string | null>(null);
  showingMessage = signal<boolean>(false);

  private messageQueue: { message: string, error: boolean }[] = [];

  show() {
    this.isLoading.set(true);
  }

  hide() {
    this.isLoading.set(false);

    if (!this.showingMessage())
      this.displayNextMessage();
  }

  private displayNextMessage() {
    if (this.messageQueue.length > 0) {
      this.showingMessage.set(true);
      const nextMessage = this.messageQueue.shift()!;

      if (nextMessage.error)
        this.errorMessage.set(nextMessage.message);
      else
        this.message.set(nextMessage.message);

      setTimeout(() => {
        this.message.set(null)
        this.errorMessage.set(null);
        this.showingMessage.set(false);
        this.displayNextMessage();
      }, this.DURATION_IN_MILLISECNDS);
    }
  }

  showMessage(message: string) {
    if (this.isLoading() || this.showingMessage()) {
      this.messageQueue.push({message: message, error: false});
    } else {
      this.showingMessage.set(true);
      this.message.set(message);
      setTimeout(() => {
        this.message.set(null);
        this.showingMessage.set(false);
        this.displayNextMessage();
      }, this.DURATION_IN_MILLISECNDS);
    }
  }

  showErrorMessage(message: string) {
    if (this.isLoading() || this.showingMessage()) {
      this.messageQueue.push({message: message, error: true});
    } else {
      this.showingMessage.set(true);
      this.errorMessage.set(message);
      setTimeout(() => {
        this.errorMessage.set(null);
        this.showingMessage.set(false);
        this.displayNextMessage();
      }, this.DURATION_IN_MILLISECNDS);
    }
  }

  extractErrorMessage(error: any): string {
    if (error?.status === 0) {
      return 'Could not connect to the server. Please check your connection or try again later.';
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
