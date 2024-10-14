import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filterByType'
})
export class FilterByTypePipe implements PipeTransform {
  transform(buses: any[], type: string): any[] {
    if (!buses || !type) {
      return buses;
    }
    return buses.filter(bus => bus.type === type);
  }
}
