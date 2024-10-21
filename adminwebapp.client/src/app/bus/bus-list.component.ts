import { Component, OnInit } from '@angular/core';
import { BusService } from './bus.service';
import { Bus } from './bus.model';

@Component({
  selector: 'app-bus-list',
  templateUrl: './bus-list.component.html',  
  styleUrls: ['./bus-list.component.css']
})
export class BusListComponent implements OnInit {
  buses: Bus[] = [];
  uniqueTypes: string[] = [];
  isLoading = false;
  error: string | null = null;

  constructor(private busService: BusService) { }

  ngOnInit(): void {
    this.loaddata();
  }

  loaddata(): void {
    this.isLoading = true;
    this.error = null;
    this.busService.getBusData().subscribe(
      (businfo: Bus[]) => {
        this.buses = businfo;
        this.extractUniqueTypes();
        this.isLoading = false;        
      },
      error => {
        this.error = '加載使用者時出錯: ' + error.message;
        this.isLoading = false;        
      }
    );
  }
  extractUniqueTypes() {
    // 使用 Set 來獲取唯一的類型
    const typeSet = new Set(this.buses.map(bus => bus.type));
    this.uniqueTypes = Array.from(typeSet);
  }

  isSpecialStation(station: string): { backgroundColor: string, color: string } {
    const specialStations: { [key: string]: { backgroundColor: string, color: string } } = {
      '國際路': { backgroundColor: '#87ceeb', color: '#000000' },
      '桃園機場第二航廈': { backgroundColor: '#87ceeb', color: '#000000' },
      '機場諾富特旅館': { backgroundColor: '#87ceeb', color: '#000000' }
    };
    return specialStations[station] || { backgroundColor: '', color: '' };
  }
}
