import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'pipeTimer'
})
export class PipeTimerPipe implements PipeTransform {

  transform(value: number): string {
    if (isNaN(value) || value < 0) return '00:00';

    const minutes = Math.floor(value / 60);
    const seconds = value % 60;

    return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
  }

}
