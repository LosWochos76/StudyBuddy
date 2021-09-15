import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'requesttype'})
export class RequestTypePipe implements PipeTransform {
  transform(value: number): string {
    if (value == 1)
        return "Freundschaftsanfrage";
    else if (value == 2)
        return "Herausforderungsbestätigung";
    else
        return "";
  }
}