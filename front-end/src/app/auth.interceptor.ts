import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private cookieService: CookieService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.cookieService.get('token');
    if (token) {
      const authReq = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });

      return next.handle(authReq).pipe(
        catchError(error => {
          if (error.status === 401) {
            // Redireciona para a tela de login se o status da resposta for 401 (Unauthorized)
            this.router.navigate(['/login']);
          }
          return throwError(error);
        })
      );
    } else {
      // Redireciona para a tela de login se não houver token
      this.router.navigate(['/login']);
    }

    return next.handle(req);
  }
}
