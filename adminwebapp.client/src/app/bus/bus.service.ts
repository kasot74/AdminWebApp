import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Bus } from './bus.model';

@Injectable({
  providedIn: 'root'
})
export class BusService {
  private apiUrl = './api/businfo';

  constructor(private http: HttpClient) { }

  getBusData(): Observable<Bus[]> {
    return this.http.get<Bus[]>(this.apiUrl);
  }
}
