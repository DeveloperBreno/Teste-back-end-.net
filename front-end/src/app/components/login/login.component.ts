import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from'src/app/services/auth.service';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginFormComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private router: Router, private cookieService: CookieService) { }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      email: ['', Validators.required],
      senha: ['', Validators.required]
    });
  }

  submit() {
    if (this.loginForm.valid) {
      const credentials = this.loginForm.value;
      this.authService.login(credentials)
        .subscribe(
          response => {
            // Lógica de sucesso
            console.log('Login bem-sucedido:', response);
            // Armazene o token JWT retornado, se houver
            this.cookieService.set('token', response.toString());
            // Redirecione para a página inicial ou faça outras ações necessárias
            this.router.navigate(['/']);
          },
          error => {
            // Lógica de erro
            console.error('Erro de login:', error);
            // Exiba uma mensagem de erro ou faça outras ações necessárias
          }
        );
    }
  }
  
}
