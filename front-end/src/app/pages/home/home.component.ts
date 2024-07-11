import { Component, OnInit } from '@angular/core';
import { TarefaService } from 'src/app/services/tarefa-service.service';
import { Tarefa } from '../../models/Tarefas';
import { ExcluirComponent } from '../../components/excluir/excluir.component'
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { disableDebugTools } from '@angular/platform-browser';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{

  tarefas: Tarefa[] = [];
  tarefasGeral: Tarefa[] = [];
  columnsToDisplay = ['titulo', 'status', 'data', 'Ações', 'Excluir'];

  constructor(private tarefaService : TarefaService, public matDialog: MatDialog, private router: Router) { }


  ngOnInit(): void {
    this.tarefaService.GetTarefas().subscribe(
      (data) => {
        const dados = data.result; 
       console.log(dados);
       dados.map((item) => {
         item.data = new Date(item.data!).toLocaleDateString('pt-BE');
       });

      this.tarefasGeral = dados;
      this.tarefas = dados;
      },
      (error) => {
        console.error('Erro ao obter tarefas:', error);
        // Redirecionar para a tela de login
        this.router.navigate(['/login']);
      }
    );
  }
  

  search(event : Event){
    const target = event.target as HTMLInputElement;
    const value = target.value.toLowerCase();

    this.tarefas = this.tarefasGeral.filter(tarefa => {
      return tarefa.titulo.toLowerCase().includes(value);
    })
  }

  openDialog(id : number){
    this.matDialog.open(ExcluirComponent,{
      width: '350px',
      height: '350px',
      data: {
        id: id
      }
    })
  }


}



