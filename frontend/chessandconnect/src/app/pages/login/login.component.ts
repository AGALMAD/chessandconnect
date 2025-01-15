import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Login } from '../../models/dto/login';

@Component({
  selector: 'app-login',
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  myForm: FormGroup;
  jwt: string = '';
  //remember: boolean = false;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {
    this.myForm = this.createForm();
  }

/*   ngOnInit(): void {
    const queryParams = this.activatedRoute.snapshot.queryParamMap;

    if (queryParams.has(this.PARAM_KEY)) {
      this.redirectTo = queryParams.get(this.PARAM_KEY);
    }
  } */

  private createForm(): FormGroup {
    return this.formBuilder.group(
      {
        credentials: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(6)]]
      }
    );
  }

  async submit(){
    const data: Login = {
      credentials: this.myForm.get('credentials')?.value,
      password: this.myForm.get('credentials')?.value
    }

    const result = await this.authService.login(data);

    // sin esto "await" el location.reload() se recarga antes que el 
    // router cambia de pagina y no funciona 
    //await this.router.navigateByUrl(this.redirectTo)
  }

}
