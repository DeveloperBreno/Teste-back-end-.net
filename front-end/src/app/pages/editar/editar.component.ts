import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Tarefa } from 'src/app/models/Tarefas';
import { TarefaService } from 'src/app/services/tarefa-service.service';

@Component({
  selector: 'app-editar',
  templateUrl: './editar.component.html',
  styleUrls: ['./editar.component.css']
})
export class EditarComponent implements OnInit{

  btnAcao = "Editar";
  btnTitulo = "Editar Tarefa!";
  tarefa!: Tarefa;

  constructor(private tarefaService : TarefaService, private router :Router,  private route : ActivatedRoute) {
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.tarefaService.GetTarefa(id).subscribe((data) => {
        this.tarefa = data.result;
        this.tarefa.data = this.tarefa.data?.substring(0, 16);
    });
  }

  async editTarefa(tarefa : Tarefa){

      this.tarefaService.EditTarefa(tarefa).subscribe(data => {
        this.router.navigate(['/']);
      });

  }

}
