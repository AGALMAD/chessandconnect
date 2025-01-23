import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Login } from '../../models/dto/login';
import Swal from 'sweetalert2';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-login',
  imports: [FormsModule, ReactiveFormsModule, NavbarComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  myForm: FormGroup;
  jwt: string = '';
  readonly PARAM_KEY: string = 'redirectTo';
  private redirectTo: string = null;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private api: ApiService
  ) {
    this.myForm = this.createForm();
  }

     ngOnInit(): void {
       const queryParams = this.activatedRoute.snapshot.queryParamMap;
  
      if (queryParams.has(this.PARAM_KEY)) {
        this.redirectTo = queryParams.get(this.PARAM_KEY);
      }
    }

  private createForm(): FormGroup {
    return this.formBuilder.group(
      {
        credentials: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(6)]],
        remember: [false]
      }
    );
  }

  async submit() {
    const data: Login = {
      credential: this.myForm.get('credentials')?.value,
      password: this.myForm.get('password')?.value
    }

    const result = await this.authService.login(data, this.myForm.get('remember')?.value)
    

    if (result.success) {
      Swal.fire({
        icon: 'success',
        text: 'Registro Correcto',
        showConfirmButton: false,
        animation: true,
        toast: true,
        position: 'top-right',
        timer: 1100
      });

      await this.router.navigateByUrl(this.redirectTo)
      location.reload()
      
    }
  
  }

}
