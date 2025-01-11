import { Component } from '@angular/core';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  
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
