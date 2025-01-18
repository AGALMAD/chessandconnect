import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { tap } from 'rxjs';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const redirectionGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot, 
  state: RouterStateSnapshot) => {

    // Inyectamos servicios
    const authService = inject(AuthService);
    const router = inject(Router);

    // Opción sin observable
    if (!authService.loged()) {
      // Navegamos al login indicando que después redireccione a donde queríamos ir en un principio
      router.navigate(['login'], { queryParams: { redirectTo: state.url }});
    }

    return authService.loged();
};