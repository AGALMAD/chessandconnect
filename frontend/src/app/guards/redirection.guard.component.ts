import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { tap } from 'rxjs';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const redirectionGuard: CanActivateFn = async (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {

  // Inyectamos servicios
  const authService = inject(AuthService);
  const router = inject(Router);

  // Verificar si el usuario está logueado
  if (!authService.loged()) {
    router.navigate(['login'], { queryParams: { redirectTo: state.url } });
    return false;
  }

  // Obtiene los datos del usuario y verifica si está baneado 
  await authService.getCurrentUser();
  
  if (authService.currentUser?.banned) {
    router.navigate(['banned']);
    return false;
  }

  return true;
};