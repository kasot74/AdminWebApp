import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authUrl = 'https://herry537.sytes.net/Account'; // 替換為您的 API URL

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.authUrl}/Login`, { username, password })
      .pipe(
        tap(response => {
          // 假設後端設置了 Cookie，這裡不需要額外處理
          console.log('User logged in');
        })
      );
  }

  logout(): Observable<any> {
    return this.http.post<any>(`${this.authUrl}/Logout`, {})
      .pipe(
        tap(() => {
          console.log('User logged out');
        })
      );
  }

  isAuthenticated(): Observable<boolean> {
    // 這裡可以調用一個 API 來檢查用戶是否已經認證
    return this.http.get<boolean>(`${this.authUrl}/IsAuthenticated`);
  }
}