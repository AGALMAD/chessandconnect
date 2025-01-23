import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Register } from '../../models/dto/register';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { NavbarComponent } from '../../components/navbar/navbar.component';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, NavbarComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  myForm: FormGroup;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router,
  ) {
    this.myForm = this.createForm();
  }

  image: File
  registerHints: Boolean = false;

  private createForm(): FormGroup {
    return this.formBuilder.group(
      {
        nickname: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
        remember: [false]
      },
      { validators: this.passwordMatchValidator }
    );
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPasswordControl = form.get('confirmPassword');
    const confirmPassword = confirmPasswordControl?.value;

    if (password !== confirmPassword) {
      confirmPasswordControl.setErrors({ mismatch: true });
    }
  }

  async submit() {
    const authData: Register = {
      username: this.myForm.get('nickname').value,
      email: this.myForm.get('email').value,
      password: this.myForm.get('password').value,
      image: this.image
    };


    console.log(authData)
    if (this.myForm.valid) {
      const result = await this.authService.register(authData, this.myForm.get('remember')?.value);
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
        this.router.navigate(['']);
      }
    } else {
      // El formulario no es válido
      this.registerHints = true;
      Swal.fire({
        icon: 'error',
        text: 'Registro erroneo.',
        showConfirmButton: false,
        animation: true,
        toast: true,
        position: 'top-right',
        timer: 1100
      });
    }
  }

  onFileSelected(event: any) {
    const image = event.target.files[0] as File; // Here we use only the first file (single file)
    this.image = image
    console.log(this.image)
  }
}