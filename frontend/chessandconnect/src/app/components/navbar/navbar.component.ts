import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit {


  ngOnInit(): void {
    const checkbox = document.getElementById('check') as HTMLInputElement; 
  
    //Si la pantalla es más pequeña que 1350px, se cierra la barra de navegación 
    if (checkbox && window.matchMedia('(max-width: 1350px)').matches) { 
      checkbox.checked = true; 
      this.onToggle({ target: checkbox } as unknown as Event);
    }
  }
  



  
  onToggle(event: Event) {
    const checkbox = event.target as HTMLInputElement;
    const nav = document.getElementById('nav');
    const icon = document.getElementById('menu-icon');

    if (checkbox.checked) {

      nav?.classList.add('hidden-nav');
      icon?.classList.add('rotate-icon');

    } else {

      nav?.classList.remove('hidden-nav');
      icon?.classList.remove('rotate-icon');
    }
  }
}
