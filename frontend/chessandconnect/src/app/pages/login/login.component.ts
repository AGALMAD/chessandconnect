import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NgIf } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [FormsModule, ReactiveFormsModule, NgIf],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  myForm: FormGroup;

  constructor(
    //private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {
    this.myForm = this.createForm();
  }

  private createForm(): FormGroup {
    return this.formBuilder.group(
      {
        name: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(6)]]
      }
    );
  }

}
