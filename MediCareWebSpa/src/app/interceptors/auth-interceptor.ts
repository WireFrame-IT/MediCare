import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, switchMap, filter, take } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { RefreshResponseDTO } from '../DTOs/response/refresh-response.dto';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
  private ignoreUrls: string[] = [];

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    this.ignoreUrls = [
      `${this.authService.apiUrl}/accounts/login`,
      `${this.authService.apiUrl}/accounts/register`
    ]
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.ignoreUrls.includes(request.url)) {
      return next.handle(request);
    }

    const accessToken = this.authService.getAccessToken();
    if (accessToken) {
      request = this.addAuthorizationHeader(request, accessToken);
    }

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          return this.handleAuthError(request, next);
        } else {
          return throwError(() => error);
        }
      })
    );
  }

  private addAuthorizationHeader(request: HttpRequest<any>, accessToken: string): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`,
      },
    });
  }

  private handleAuthError(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.authService.refreshAccessToken().pipe(
        switchMap((newTokens: RefreshResponseDTO) => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(newTokens.refreshToken);
          return next.handle(this.addAuthorizationHeader(request, newTokens.accessToken));
        }),
        catchError((err) => {
          this.isRefreshing = false;
          this.authService.logout();
          this.router.navigate(['/login']);
          return throwError(() => err);
        })
      );
    }

    return this.refreshTokenSubject.pipe(
      filter((token) => token != null),
      take(1),
      switchMap(() => next.handle(this.addAuthorizationHeader(request, this.authService.getAccessToken()!)))
    );
  }
}
