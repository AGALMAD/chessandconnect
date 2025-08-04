import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RedirectionService } from '../../services/redirection.service';

@Component({
  selector: 'app-guard-redirection',
  standalone: true,
  imports: [],
  templateUrl: './guard-redirection.component.html',
  styleUrl: './guard-redirection.component.css'
})
export class GuardRedirectionComponent {
  readonly PARAM_KEY: string = 'redirectTo';
  private redirectTo: string = null;
  
  constructor(
    private service: RedirectionService,
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    // Vemos si tenemos que redirigir al usuario tras el inicio de sesión
    const queryParams = this.activatedRoute.snapshot.queryParamMap;

    if (queryParams.has(this.PARAM_KEY)) {
      this.redirectTo = queryParams.get(this.PARAM_KEY);
    }
  }
}