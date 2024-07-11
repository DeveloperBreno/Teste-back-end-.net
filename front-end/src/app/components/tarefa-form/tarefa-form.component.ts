import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Tarefa } from 'src/app/models/Tarefas';
import { TarefaService } from 'src/app/services/tarefa-service.service';

@Component({
  selector: 'app-tarefa-form',
  templateUrl: './tarefa-form.component.html',
  styleUrls: ['./tarefa-form.component.css']
})
export class TarefaFormComponent implements OnInit{
  @Output() onSubmit = new EventEmitter<Tarefa>();
  @Input() btnAcao!: string;
  @Input() btnTitulo!: string;
  @Input()  dadosTarefa: Tarefa | null = null;

  tarefaForm!: FormGroup;
  ativo:number = 1;


  constructor(private tarefaService : TarefaService, private router: Router) {
  }


  ngOnInit(): void {

    this.tarefaForm = new FormGroup ({
      id: new FormControl(this.dadosTarefa ? this.dadosTarefa.id : 0),
      titulo: new FormControl(this.dadosTarefa ? this.dadosTarefa.titulo : '', [Validators.required]),
      descricao: new FormControl(this.dadosTarefa ? this.dadosTarefa.descricao : '', [Validators.required]),
      status: new FormControl(this.dadosTarefa ? this.dadosTarefa.status : '',[Validators.required]),
      
      data: new FormControl(this.dadosTarefa ? this.dadosTarefa.data : '',[Validators.required]),

    });
    console.log(this.tarefaForm.value)
  }


  get nome(){
    return this.tarefaForm.get('nome')!;
  }

  submit(){

      console.log(this.tarefaForm.value)

      this.onSubmit.emit(this.tarefaForm.value);
  }

}
