# Microservico

Este repositório contém um microserviço simples escrito em ASP.NET Core (.NET 8). O objetivo deste README é explicar de forma didática, passo a passo e para um público leigo, o que o projeto faz, quais recursos usa e como executar e testar a aplicação no seu computador.

Sumário
- Visão geral
- Recursos principais (explicados de forma simples)
- Tecnologias usadas
- Como configurar (pré-requisitos)
- Como executar localmente
- Como testar a API (exemplos)
- Estrutura do código e responsabilidades dos arquivos
- Como trabalhar com o banco de dados (migrations)
- Dicas e próximos passos

Visão geral
-----------

Este microserviço expõe uma API (um conjunto de endereços de internet) para gerenciar um catálogo de produtos. Com a API você pode listar produtos, consultar um produto específico, criar, atualizar e excluir produtos.

Recursos principais (explicados de forma simples)
-----------------------------------------------
- Endpoints HTTP (API): pontos que outros sistemas ou você, com ferramentas como o navegador ou o curl, podem chamar para obter ou manipular dados.
- Banco de dados relacional (SQL Server): onde os dados do catálogo são guardados de forma estruturada.
- Entity Framework Core: biblioteca que facilita trabalhar com banco de dados usando código C# em vez de escrever SQL manualmente.
- Swagger (Swashbuckle): gera automaticamente uma página web com documentação interativa da API para testar os endpoints sem precisar de outras ferramentas.

Tecnologias usadas
------------------
- .NET 8 / ASP.NET Core Web API: plataforma e framework que executam a aplicação e entregam a API.
- Microsoft.EntityFrameworkCore.SqlServer: provedor que permite ao Entity Framework Core comunicar-se com o SQL Server.
- Microsoft.EntityFrameworkCore.Tools: ferramentas para criar e aplicar alterações no banco (migrations).
- Swashbuckle.AspNetCore (Swagger): cria a documentação interativa da API.

Como configurar (pré-requisitos)
--------------------------------
Antes de executar a aplicação, você precisa ter no computador:

1. .NET 8 SDK instalado. Você pode baixar do site oficial da Microsoft (procure por ".NET SDK").
2. Um servidor SQL Server acessível. Para um ambiente de desenvolvimento você pode usar:
   - SQL Server Express / LocalDB (Windows), ou
   - SQL Server em um container Docker, ou
   - Uma instância remota já existente.
3. (Opcional, mas recomendado para gerenciar migrations) a ferramenta dotnet-ef instalada globalmente:
   dotnet tool install --global dotnet-ef

Como executar localmente
------------------------
1. Abra um terminal (PowerShell no Windows) na pasta do projeto (onde está o arquivo .csproj).
2. Restaurar pacotes e compilar:
   - dotnet restore
   - dotnet build
3. Configurar a cadeia de conexão (connection string) com o banco de dados.
   - Crie ou edite o arquivo appsettings.Development.json (ou appsettings.json) e adicione uma seção ConnectionStrings com a chave DefaultConnection.
     Exemplo (substitua os valores pelo seu servidor e credenciais):

     {
       "ConnectionStrings": {
         "DefaultConnection": "Server=localhost;Database=CatalogDb;User Id=sa;Password=SuaSenha;TrustServerCertificate=True;"
       }
     }

   - A aplicação lê a connection string com o nome "DefaultConnection".
4. Aplicar migrations (criar as tabelas no banco):
   - dotnet ef database update
   Se preferir, a aplicação pode ser configurada para aplicar migrations automaticamente ao iniciar, mas aqui usamos o comando manual para maior controle.
5. Executar a aplicação:
   - dotnet run
6. A aplicação iniciará e, normalmente, estará disponível em https://localhost:5001 (ou outra porta mostrada no terminal).

Como testar a API (exemplos)
---------------------------
Ao executar em modo Development, a aplicação expõe a interface do Swagger para testar os endpoints:

- Abra no navegador: https://localhost:5001/swagger

Endpoints principais (rotas)
- GET  /api/catalog/items           => Lista todos os produtos
- GET  /api/catalog/items/{id}      => Retorna produto por id
- POST /api/catalog/items           => Cria um novo produto (envie dados em JSON)
- PUT  /api/catalog/items/{id}      => Atualiza um produto
- DELETE /api/catalog/items/{id}    => Remove um produto

Exemplo de requisição para criar um item (JSON):

{
  "name": "Camiseta Exemplo",
  "description": "Camiseta de algodão",
  "price": 49.90,
  "pictureFileName": "camiseta.jpg",
  "pictureUri": "https://exemplo.com/imagens/camiseta.jpg",
  "catalogTypeId": 1,
  "catalogBrandId": 1,
  "availableStock": 100,
  "restockThreshold": 10,
  "maxStockThreshold": 200,
  "onReorder": false
}

Você pode usar o curl para testar (exemplo de GET):

curl https://localhost:5001/api/catalog/items --insecure

(--insecure é usado quando o certificado de desenvolvimento não é confiável localmente)

Estrutura do código e responsabilidades dos arquivos
--------------------------------------------------
- Program.cs: ponto de entrada da aplicação. Configura serviços (como o Entity Framework e o Swagger) e define como a aplicação responde a requisições.
- Data/CatalogContext.cs: classe que representa a conexão com o banco (DbContext). Contém DbSet<T> que mapeiam para as tabelas do banco.
- Models/: classes que representam os dados (por exemplo, CatalogItem, CatalogBrand, CatalogType). Cada instância dessas classes corresponde a uma linha nas tabelas do banco.
- Controllers/CatalogController.cs: contém os endpoints da API para manipular o catálogo (listar, criar, atualizar, excluir).
- Migrations/: pasta gerada pelo Entity Framework que guarda o histórico das mudanças na estrutura do banco (não editar manualmente).

Como trabalhar com o banco de dados (migrations)
----------------------------------------------
O Entity Framework usa "migrations" para controlar as alterações na estrutura do banco (por exemplo, criar tabelas, adicionar colunas). Fluxo básico:

1. Fazer alterações nas classes do Models (ex.: adicionar propriedade).
2. Criar nova migration:
   - dotnet ef migrations add NomeDaMigration
3. Aplicar no banco:
   - dotnet ef database update

Dica: verifique o arquivo de snapshot em Migrations para entender a estrutura atual do banco.

Dicas e próximos passos
-----------------------
- Para um ambiente de produção, use uma instância de banco de dados segura, variáveis de ambiente para secrets e configure logging/monitoramento.
- Para facilitar testes locais, considere usar um container Docker com SQL Server.
- Dependendo das necessidades, pode-se adicionar autenticação, validação de entrada (DTOs) e paginação nos endpoints de listagem.

Contato
-------
Este README foi gerado para explicar o projeto de forma didática. Se precisar de detalhes técnicos adicionais (scripts, exemplos de Dockerfile, ou orientação para deploy), peça instruções específicas.

