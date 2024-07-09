## para iniciar o sistema localmente siga as instruções 

Para fazer as configurações você precisará instalar o docker em sua maquina.



Crie um container docekr com o serviço do sql server


docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SuaSenhaAqui" && 
  -p 1433:1433 --name sqlserver &&
  -d mcr.microsoft.com/mssql/server:latest



  Crie um container com o serviço de filas rabbitqm

  docker pull rabbitmq:3.11-management

docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_PASS="DcpmQP3pFjNFrRJFNDwTRNwdAIKlvlHMCNrsC67Ijzc" rabbitmq:3.11-management



## Faça update database

no visual studio, seleciona o webApi como startap up project (projeto de inicialização)

no package Manager Console selecione o projeto Infraestrutura e execute esse comando:

Add-Migration Criacao -Context Contexto 

para atualizar no banco:
Update-Database -Context Contexto


agora é só iniciar o projeto webApi

As atualizações de usuários são feitas por um worker na pasta "Workers", que lê da fila para inserir no banco de dados então você pode executar o projeto "CreateAndEditUser"
