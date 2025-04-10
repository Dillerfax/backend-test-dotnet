# API de Gerenciamento de Estacionamento - Teste FCamara

API REST desenvolvida como parte do teste para a vaga de Desenvolvedor Back-end .NET na FCamara. O objetivo é gerenciar estabelecimentos (estacionamentos), veículos e controlar a entrada e saída dos mesmos.

## Descrição do Projeto

Esta API permite realizar as seguintes operações:

* **Gerenciamento de Estabelecimentos:** CRUD (Criar, Ler, Atualizar, Deletar) para os dados dos estacionamentos, incluindo nome, CNPJ, endereço, telefone e capacidade de vagas para carros e motos.
* **Gerenciamento de Veículos:** CRUD (Criar, Ler, Atualizar, Deletar) para os dados dos veículos, incluindo marca, modelo, cor, placa e tipo (Carro ou Moto).
* **Controle de Estacionamento:** Registrar a entrada e a saída de veículos em um estabelecimento específico, validando a disponibilidade de vagas e prevenindo registros duplicados.

## Tecnologias Utilizadas

* **Framework:** .NET 8.0 (ou a versão que você utilizou: 5.0, 6.0, 7.0)
* **Linguagem:** C#
* **Arquitetura:** API RESTful com ASP.NET Core Web API
* **Persistência de Dados:** Entity Framework Core 8 (ou a versão correspondente)
* **Banco de Dados:** Microsoft SQL Server
* **Documentação da API:** Swagger (via Swashbuckle.AspNetCore)

## Pré-requisitos

Antes de começar, garanta que você tenha instalado:

* [.NET SDK](https://dotnet.microsoft.com/download) (versão 8.0 ou a utilizada no projeto)
* [Microsoft SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) (Express, Developer ou outra edição) ou LocalDB (geralmente instalado com o Visual Studio).
* [Git](https://git-scm.com/downloads)
* (Opcional) [Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/vs/) ou [Visual Studio Code](https://code.visualstudio.com/)
* (Opcional) [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/pt-br/sql/ssms/download-sql-server-management-studio-ssms) ou [Azure Data Studio](https://docs.microsoft.com/pt-br/sql/azure-data-studio/download-azure-data-studio) para gerenciar o banco de dados.

## Configuração e Instalação

1.  **Clonar o Repositório:**
    ```bash
    git clone <URL_DO_SEU_REPOSITORIO_FORK>
    cd <NOME_DA_PASTA_DO_PROJETO>
    ```

2.  **Configurar a String de Conexão:**
    * Abra o arquivo `appsettings.Development.json` (ou `appsettings.json`).
    * Localize a seção `ConnectionStrings`.
    * Altere o valor de `DefaultConnection` para apontar para a sua instância do SQL Server.

    **Exemplo para SQL Server LocalDB (comum com Visual Studio):**
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EstacionamentoDbFc;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
    }
    ```

    **Exemplo para SQL Server Express:**
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=.\\SQLEXPRESS;Database=EstacionamentoDbFc;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
    }
    ```
    *(Ajuste `Server=` e `Database=` conforme sua configuração).*

3.  **Aplicar as Migrações do Entity Framework Core:**
    * Abra um terminal ou PowerShell na pasta raiz do projeto da API (onde está o arquivo `.csproj`).
    * Execute o comando para criar/atualizar o banco de dados e suas tabelas:
        ```bash
        dotnet ef database update
        ```
    * *Alternativa (Visual Studio):* Abra o Console do Gerenciador de Pacotes (View -> Other Windows -> Package Manager Console) e execute `Update-Database`.

## Executando a Aplicação

1.  **Via Linha de Comando:**
    * Navegue até a pasta do projeto da API.
    * Execute o comando:
        ```bash
        dotnet run
        ```

2.  **Via Visual Studio:**
    * Abra o arquivo de solução (`.sln`) no Visual Studio.
    * Pressione `F5` ou clique no botão de "Play" (geralmente com o nome do projeto).

A API estará disponível no endereço indicado no console (ex: `https://localhost:7123` ou `http://localhost:5123`).

## Usando a API (Endpoints)

A maneira mais fácil de explorar e interagir com a API é através da interface do **Swagger UI**, geralmente disponível em `/swagger` na URL base da aplicação (ex: `https://localhost:7123/swagger`).

### Estabelecimentos (`/api/estabelecimentos`)

* **`GET /api/estabelecimentos`**: Retorna a lista de todos os estabelecimentos.
* **`GET /api/estabelecimentos/{id}`**: Retorna os detalhes de um estabelecimento específico pelo seu ID.
* **`POST /api/estabelecimentos`**: Cria um novo estabelecimento.
    * *Request Body (Exemplo):*
        ```json
        {
          "nome": "Estacionamento Novo",
          "cnpj": "98765432000111",
          "endereco": "Av. Secundária, 456",
          "telefone": "11988887777",
          "quantidadeVagasMotos": 5,
          "quantidadeVagasCarros": 15
        }
        ```
    * *Success Response:* `201 Created` com o objeto criado e cabeçalho `Location`.
* **`PUT /api/estabelecimentos/{id}`**: Atualiza um estabelecimento existente. Requer o ID na URL e o objeto completo no corpo da requisição (incluindo o ID correspondente).
    * *Success Response:* `204 No Content`.
* **`DELETE /api/estabelecimentos/{id}`**: Remove um estabelecimento pelo seu ID.
    * *Success Response:* `204 No Content`.

### Veículos (`/api/veiculos`)

* **`GET /api/veiculos`**: Retorna a lista de todos os veículos.
* **`GET /api/veiculos/{id}`**: Retorna os detalhes de um veículo específico pelo seu ID.
* **`POST /api/veiculos`**: Cria um novo veículo.
    * *Request Body (Exemplo):*
        ```json
        {
          "marca": "Honda",
          "modelo": "Civic",
          "cor": "Preto",
          "placa": "BRA2E19",
          "tipo": 0 // 0 para Carro, 1 para Moto
        }
        ```
    * *Success Response:* `201 Created` com o objeto criado e cabeçalho `Location`.
* **`PUT /api/veiculos/{id}`**: Atualiza um veículo existente.
    * *Success Response:* `204 No Content`.
* **`DELETE /api/veiculos/{id}`**: Remove um veículo pelo seu ID.
    * *Success Response:* `204 No Content`.

### Controle de Estacionamento (Dentro de `/api/estabelecimentos`)

* **`POST /api/estabelecimentos/{idEstabelecimento}/entrada`**: Registra a entrada de um veículo em um estabelecimento.
    * *Parâmetros:* `idEstabelecimento` na URL.
    * *Request Body (Exemplo):*
        ```json
        {
          "placa": "BRA2E19"
        }
        ```
    * *Success Response:* `201 Created` com o registro da entrada.
    * *Error Responses:* `404 Not Found` (estabelecimento ou veículo não existe), `409 Conflict` (veículo já estacionado, sem vagas).
* **`POST /api/estabelecimentos/{idEstabelecimento}/saida`**: Registra a saída de um veículo de um estabelecimento.
    * *Parâmetros:* `idEstabelecimento` na URL.
    * *Request Body (Exemplo):*
        ```json
        {
          "placa": "BRA2E19"
        }
        ```
    * *Success Response:* `200 OK` com o registro atualizado (incluindo `horaSaida`).
    * *Error Responses:* `404 Not Found` (estabelecimento, veículo ou registro de entrada ativo não encontrado).
* **`GET /api/estabelecimentos/{idEstabelecimento}/registros`**: Lista todos os registros de entrada/saída para um estabelecimento específico.
* **`GET /api/estabelecimentos/{idEstabelecimento}/registros/{idRegistro}`**: Retorna um registro de estacionamento específico.

*(Fim do Modelo)*