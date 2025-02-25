import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const cookieService = inject(CookieService);
  const authToken = cookieService.get('Authorization');

  if (req.urlWithParams.includes('addAuth=true')) {
    const authReq = req.clone({
      setHeaders: {
        Authorization: authToken,
      },
    });

    return next(authReq);
  }

  return next(req);
};
