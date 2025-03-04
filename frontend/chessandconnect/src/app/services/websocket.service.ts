import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { environment } from '../../environments/environment';
import { ApiService } from './api.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class WebsocketService {

  constructor(
    private api: ApiService,
    private router: Router
  ) { }

  // Eventos
  connected = new Subject<void>();
  messageReceived = new Subject<any>();
  disconnected = new Subject<void>();

  private onConnected() {
    console.log('Socket connected');
    this.connected.next();
  }

  private onMessageReceived(message: string) {
    this.messageReceived.next(message);
  }

  private onError(error: any) {
    console.error('Error:', error);
    this.disconnectRxjs()

    Swal.fire({
      title: '<i class="fa-solid fa-chess-board"></i> ¡Error de conexión!',
      toast: true,
      position: 'top-end',
      timer: 5000,
      timerProgressBar: true,
      background: '#301e16',
      color: '#E8D5B5',
      customClass: {
        popup: 'rounded-lg shadow-lg',
        title: 'font-bold text-lg',
        confirmButton: 'bg-[#CBA77B] hover:bg-[#A68556] text-[#301e16] font-medium py-2 px-4 rounded-lg',
        cancelButton: 'bg-[#CBA77B] hover:bg-[#A68556] text-[#301e16] font-medium py-2 px-4 rounded-lg',
        timerProgressBar: 'bg-[#E8D5B5]'
      }
    }).then(() => {
      this.disconnectRxjs()
      this.onDisconnected();
    });
  }

  private onDisconnected() {
    console.log('WebSocket connection closed');
    this.disconnected.next();
    this.router.navigate(['/']);
  }

  // ============ Usando Rxjs =============

  rxjsSocket: WebSocketSubject<string>;

  isConnectedRxjs() {
    return this.rxjsSocket && !this.rxjsSocket.closed;
  }

  async connectRxjs() {

    if (!this.isConnectedRxjs() && this.api.jwt) {

      console.log("conectado")

      this.rxjsSocket = webSocket({
        url: environment.socketUrl + "?jwt=" + this.api.jwt,

        // Evento de apertura de conexión
        openObserver: {
          next: () => this.onConnected()
        },

        // La versión de Rxjs está configurada por defecto para manejar JSON
        // Si queremos manejar cadenas de texto en crudo debemos configurarlo
        serializer: (value: string) => value,
        deserializer: (event: MessageEvent) => event.data
      });

      this.rxjsSocket.subscribe({
        // Evento de mensaje recibido
        next: (message: string) => this.onMessageReceived(message),

        // Evento de error generado
        error: (error) => this.onError(error),

        // Evento de cierre de conexión
        complete: () => this.onDisconnected()
      });
    }
  }

  sendRxjs(message: string) {
    this.rxjsSocket.next(message);
  }

  disconnectRxjs() {
    this.rxjsSocket.complete();
    this.rxjsSocket = null;
    console.log("Desconectado")
  }
}