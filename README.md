## Para iniciar o sistema localmente siga as instruções
Para fazer as configurações você precisará instalar o docker em sua maquina.

Crie um container docker com o serviço do sql server (opcional caso já tenha algum serviço de Sql Server)

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SuaSenhaAqui" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server

Crie um container com o serviço de filas rabbitmq (ou utilize um existente)

faça um pull do rabbitmq
docker pull rabbitmq:3.11-management

inicie o container
docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_PASS="DcpmQP3pFjNFrRJFNDwTRNwdAIKlvlHMCNrsC67Ijzc" rabbitmq:3.11-management

Faça update database
No visual studio, selecione o webApi como startup project (projeto de inicialização)

No Package Manager Console, selecione o projeto Infraestrutura e execute esse comando:

(mas apenas execute esse comando caso a pasta migrations esteja vazia)
Add-Migration Criacao -Context Contexto

Para atualizar no banco:
Update-Database -Context Contexto

Você pode repetir esse processo caso crie ou altere as entidades/tabelas do banco, assim podemos controlar as atualizações de banco em nossa camada de infraestrutura.

Agora é só iniciar o projeto webApi.

As atualizações de usuários são feitas por um worker na pasta "Workers", que lê da fila para inserir no banco de dados. Então você pode executar o projeto "CreateAndEditUser", via terminal com o comando "dotnet run" dentro da pasta do projeto "CreateAndEditUser".

O Worker que eu criei utiliza uma estratégia de fila de erro para cada fila. Por exemplo:
Temos a fila "CriarOuAtualizarUsuario". Caso dê algum erro ao processar o item, o próprio worker joga esse item em uma outra fila, por exemplo: "FilaDeErroParaCriarOuAtualizarUsuario". Assim, o programador/equipe pode analisar o cenário e fazer as correções, tudo isso sem perder a informação.

Em resumo, podemos criar uma fila e um worker (Consumer) para cada atualização de entidade no banco de dados, garantindo a manutenção no sistema e garantindo as falhas.
Nesse projeto, caso dê algum erro em operações de insert, update ou delete, o worker joga para uma outra fila indicando que houve falha ao tentar processar o objeto. Essa mesma lógica pode ser aplicada para a entidade "Tarefa" e, caso o sistema venha a crescer, podemos ter mais workers. Todos os workers "Consumers" fazem a importação das DLLs da aplicação, mantendo o domínio do negócio independente da implementação do sistema ou solução técnica.

Obs.: Não foi feito um worker para as operações da entidade "Tarefa", mas caso seja feito, será igual ao worker de "User".

Atenção: lembre-se de atualizar os app.settings de cada projeto, inclusive o projeto de testes.

Esse projeto pode ganhar a funcionalidade de atualização em tempo real com o SignalR, basta criar as classes de Hub e fazer suas funcionalidades em tempo real no cliente e no servidor.

A ideia do projeto back-end é focar no domínio do negócio e escalar a aplicação com micro serviços, utilizando o conceito do DDD no coração da aplicação, enquanto o front-end é focado em fazer autenticações e operações CRUD.

Atenção: se você quer rodar esse projeto em produção, adicione algum log para o back-end, por exemplo o Graylog ou alguma tática com banco não relacional.
