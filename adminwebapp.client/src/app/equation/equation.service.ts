import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Equation } from './equation.model';

@Injectable({
  providedIn: 'root'
})
export class EquationService {
  private apiUrl = './api/equation';

  constructor(private http: HttpClient) { }

  getData(): Observable<Equation[]> {
    return this.http.get<Equation[]>(this.apiUrl)
  }

  create(user: Equation): Observable<Equation> {
    return this.http.post<Equation>(this.apiUrl, user);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
