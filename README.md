## Para iniciar o sistema localmente siga as instruções 

Para fazer as configurações você precisará instalar o docker em sua maquina.

Crie um container docekr com o serviço do sql server (opcional caso já tenha algum serviço de Sql Server)

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SuaSenhaAqui" && 
  -p 1433:1433 --name sqlserver &&
  -d mcr.microsoft.com/mssql/server:latest

Crie um container com o serviço de filas rabbitmq (ou utilize um existente)

faça um pull do rabbitmq
docker pull rabbitmq:3.11-management

inicie o container
docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_PASS="DcpmQP3pFjNFrRJFNDwTRNwdAIKlvlHMCNrsC67Ijzc" rabbitmq:3.11-management

## Faça update database
No visual studio, seleciona o webApi como startap up project (projeto de inicialização)

no Package Manager Console selecione o projeto Infraestrutura e execute esse comando:

(mas apenas execute esse comando caso a pasta migrations esteja vazia)
Add-Migration Criacao -Context Contexto 

para atualizar no banco:
Update-Database -Context Contexto

Pode repedir esse processo caso crie ou altera as entidades / tabelas do banco, assim podemos controlar as atualizações de banco em nossa camada de infraestrutura.

Agora é só iniciar o projeto webApi

As atualizações de usuários são feitas por um worker na pasta "Workers", que lê da fila para inserir no banco de dados então você pode executar o projeto "CreateAndEditUser", voce pode pode execuar via terminal com o comando "dotner run" dentro da pasta do projeto "CreateAndEditUser".

O Worker que eu criei utilizei uma estrategia de fila de erro para cada fila, por exemplo:
Temos a fila "CriarOuAtualizarUsuario" caso dê algum erro ao processar o item o propio worker joga esse item em uma outra fila, por exemplo: "FilaDeErroParaCriarOuAtualizarUsuario", assim o programador / equipe podem analizar o cenario e fazer as coreções, isso tudo sem perder a informação.

Em resumo podemos criar uma fila e um worker (Consumer) para cada atualização de entidade no banco de dados, assim garantindo uma manutenção no sistema e garantindo as falhas.
Nesse projeto caso dê algum erro em operações de insert, update ou delete o worker joga para uma outra fila indicando que teve falha ao tentar processar o obj essa mesma logica pode ser aplicada para a entidade "Tarefa" e caso o sistema venha a crescer podemos ter mais workers, todos os worker "Consumers" faz a importação das DLLs da aplicação sendo assim mantendo o domínio do negócio independente da implementação do sistema ou solução técnica.

Obs. Não foi feito um worker para as operações da entidade "Tarefa", mas caso for feita será igual ao worker de "User".

Atenção lembre-se de atualizar os app.settings de cada projeto até mesmo o projeto de testes.

Esse projeto pode ganhar a funcionalidade de atualização em tempo real com o SignalR, basta criar as classes de Hub e fazer suas funcionalidades em tempo real clients and  server.


A ideia do projeto back-end é focar no domínio do negocio e escalar a aplicação com micros serviços, utilizando o conceito do DDD no coração da aplicação, enquanto o front-end é focado em fazer autenticações e operações CRUD.

Atenção: se você quer rodar esse projeto em produção, adicione algum log para o back-end por exemplo o graylog ou alguma tatica com banco nção relacional.

