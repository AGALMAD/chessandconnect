import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { GameService } from '../../services/game.service';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/dto/user';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-chess-result',
  imports: [],
  templateUrl: './chess-result.component.html',
  styleUrl: './chess-result.component.css'
})
export class ChessResultComponent {

  public baseUrl = environment.apiUrl;

  public winner: User;
  public looser: User;

  constructor(
    public dialogRef: MatDialogRef<ChessResultComponent>,
    public authService : AuthService,
    @Inject(MAT_DIALOG_DATA) public data: any 
  ){
    this.winner = data.winner
    this.looser = data.looser
  }

  closeModal() {
    this.dialogRef.close();
  }
}
