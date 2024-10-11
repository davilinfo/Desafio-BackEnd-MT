## Access application
user: admin@davinetlive.onmicrosoft.com
password: Mottu2021

user: user@davinetlive.onmicrosoft.com
password: Mottu2021

## Postman json
- Arquivo está localizado na pasta SistemaMottu-desafio e na pasta IntegrationTests
- No arquivo json existe authorization request configurado nas pastas locacao, motos, entregadores (conteúdos internos herdam de tais pastas configuração de authorize)
- Pasta motos deve utilizar token de usuário admin
- Pastas entregadores e locacao devem utilizar token de usuário user
- Quando tokens forem gerados e salvos no postman em tais pastas então requests estarão autorizadas

## IPFS (Interplanetary file system)
Imagem cnh é gravada no IPFS, assim apenas identificador da imagem é salvo em banco de dados

## Redis installation in docker (cluster better option)
docker run --name redis -p 6379:6379 -d redis

## RabbitMQ installation version latest 4+
docker run --name rabbitmq -p 5672:5672 -d rabbitmq

## MongoDB installation in docker (cluster better option) (após instalar pode acessar docker exec -it mongodb mongo)
docker run --name mongodb -p 27017:27017 -d mongo:latest

## Postgres installation in docker (cluster better option)
docker run --name manutencaomoto -p 5432:5432 -e POSTGRES_USER=moto -e POSTGRES_PASSWORD=moto -e POSTGRES_DB=manutencaomoto -d postgres

### Criar tabelas (Executar no cmd na pasta da solução)
dotnet ef database update 20241009124749_initial --project Persistence -s SistemaManutencaoMotos -c Context --verbose
### ou (Executar no Gerenciador de pacotes projeto Persistence o comando entity framework tool)
Update-Database

### Connection string de alta performance
"Host=localhost;Port=5432;Database=manutencaomoto;Username=moto;Password=moto;Pooling=true;MinPoolSize=10;MaxPoolSize=100;No Reset On Close=true;"

- Pooling=true: Habilita o uso de pool de conexões, o que melhora a performance ao reutilizar conexões existentes.
- MinPoolSize=10: Define o número mínimo de conexões no pool.
- MaxPoolSize=100: Define o número máximo de conexões no pool.
- No Reset On Close=true: Evita o reset da conexão ao fechá-la, o que pode melhorar a performance em cenários de conexões de curta duração1.
