# Feedback - Avaliação Geral

## Front End
### Navegação

* **Pontos Positivos:**
  - Será avaliado na entrega final

* **Pontos Negativos:**
  - Será avaliado na entrega final

### Design

* **Pontos Positivos:**
  - Será avaliado na entrega final

* **Pontos Negativos:**
  - Será avaliado na entrega final

### Funcionalidade

* **Pontos Positivos:**
  - Funcionalidades de CRUD de Produtos e Categorias estão presentes.
  - As ações estão protegidas por autenticação via Cookie no MVC.
  - Utilização correta de ASP.NET Identity no processo de login.

## Back End
### Arquitetura

* **Pontos Negativos:**
  - Projeto dividido em 5 camadas, excedendo o numero de camadas necessárias para este tipo de projeto:
    - `MLV.MVC` (interface MVC)
    - `MLV.Api` (Web API)
    - `MLV.Core` (compartilhado: models, dtos, entre outros)
    - `MLV.Business` (regras e serviços)
    - `MLV.Infra` (Entity Framework e persistência)
  - Foi implementada uma arquitetura com CQRS desnecessária para o tipo de desafio
  - `MLV.Core` atua como um "dump" de entidades e DTOs, mas com pouca separação conceitual entre domínio e compartilhamento.

  - Como foi orientado, guarde seu arsenal técnico para o momento certo, "matar uma formiga com um canhão" é visto como uma má prática.

### Funcionalidade

* **Pontos Positivos:**
  - A API expõe endpoints RESTful conforme os requisitos do escopo.
  - API com autenticação JWT implementada corretamente.
  - Proteção dos endpoints sensíveis.

* **Pontos Negativos:**
  - Não há controle para impedir que um usuário altere produtos de outro.

### Modelagem

* **Pontos Positivos:**
  - Entidades `Produto`, `Categoria` e `Vendedor` implementadas.
  - Vínculo entre `Produto` e `Vendedor` por meio do `UserId` do ASP.NET Identity.
  - Validações básicas estão presentes no modelo.

## Projeto
### Organização

* **Pontos Positivos:**
  - Boa organização de pastas em `src/`, com `.sln` na raiz.
  - Separação física entre API e MVC evita confusão de responsabilidades.

### Documentação
* **Pontos Positivos:**
  - `README.md` existente e razoavelmente bem escrito.
  - Swagger está presente para documentar a API.

### Instalação
* **Pontos Positivos:**
  - Projeto está configurado para SQLite no `appsettings.Development.json`, seguindo o escopo corretamente.
  - Migrations e seed estão prontos para uso.