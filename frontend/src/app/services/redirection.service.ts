import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RedirectionService {

  constructor() { 
    console.log("Se crea servicio auth"); 
  }
}