import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-banned-view',
  imports: [],
  templateUrl: './banned-view.component.html',
  styleUrl: './banned-view.component.css'
})
export class BannedViewComponent {

  constructor(public authService :AuthService){}

  
  submitAppeal(event: Event) {
    event.preventDefault(); // Evita el refresco del formulario para poder mostrar el mensaje de apelación enviada

    const form = event.target as HTMLFormElement;

    Swal.fire({
      title: '✅ Apelación enviada',
      text: 'Tu solicitud ha sido enviada. Nos pondremos en contacto contigo pronto.',
      icon: 'success',
      confirmButtonColor: '#8b5a2b'
    }).then(() => {
      form.reset(); 
    });
  }
}
