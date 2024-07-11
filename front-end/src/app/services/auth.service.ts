import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.baseApiUrl}`;

  constructor(private http: HttpClient) { }

  login(credentials: { 
		email: string, 
		senha: string,
		celular: string, 
		userName: string, 
		nascimento: string }): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(`${this.apiUrl}/v1/User/Token`, credentials);
  }
}
