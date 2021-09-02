import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'category'})
export class CategoryPipe implements PipeTransform {
  transform(value: number): string {
    if (value == 2)
        return "Netzwerken";
    else if (value == 3)
        return "Organisieren";
    else
        return "Lernen";
  }
}