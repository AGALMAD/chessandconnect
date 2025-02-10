import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-chess',
  imports: [CommonModule],
  templateUrl: './chess.component.html',
  styleUrl: './chess.component.css'
})
export class ChessComponent {
  letters: string[] = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'];
  rows: number[] = [8, 7, 6, 5, 4, 3, 2, 1];
  cells: string[] = [];

  rowsReverse: number[] = [1, 2, 3, 4, 5, 6, 7, 8];
  lettersReverse: string[] = ['H', 'G', 'F', 'E', 'D', 'C', 'B', 'A'];

}
