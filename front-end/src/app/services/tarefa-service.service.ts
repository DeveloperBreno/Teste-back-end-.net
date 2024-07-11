import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Tarefa } from '../models/Tarefas';
import { Response } from '../models/Response';

@Injectable({
  providedIn: 'root'
})
export class TarefaService {

  private apiUrl = `${environment.baseApiUrl}/Tarefa`

  constructor(private http: HttpClient) { }

  GetTarefas() : Observable<Response<Tarefa[]>> {
      return this.http.get<Response<Tarefa[]>>(`${this.apiUrl}/List`);
  }

  GetTarefa(id: number) : Observable<Response<Tarefa>> {
    return this.http.get<Response<Tarefa>>(`${this.apiUrl}/${id}`);
  }

  CreateTarefa(tarefa: Tarefa) : Observable<Response<Tarefa[]>> {
    return this.http.post<Response<Tarefa[]>>(`${this.apiUrl}/Create`, tarefa);
  }

  EditTarefa(tarefa : Tarefa) : Observable<Response<Tarefa[]>> {
      return this.http.put<Response<Tarefa[]>>(`${this.apiUrl}`, tarefa);
  }

  ExcluirTarefa(id: number) : Observable<Response<Tarefa[]>>{
    return this.http.delete<Response<Tarefa[]>>(`${this.apiUrl}/${id}`)
  }
}
