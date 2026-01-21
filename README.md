# FiapCloudGames Users API

API construída em **.NET 8** para gerenciamento de usuários.

**Obs**: Explicação de Infra em https://github.com/joseeduardoleite/fcg-orchestration

## 📦 Tecnologias & Ferramentas

- .NET 8
- C#
- ASP.NET Core Web API
- MassTransit
- RabbitMQ
- Docker
- Kubernetes
- FluentValidation
- AutoMapper
- Moq + xUnit (para testes unitários)
- Asp.Versioning (API versioning)

## 🚀 Funcionalidades

- CRUD de usuários
- Validação de DTOs usando FluentValidation
- Controle de acesso via claims e roles (`Admin`, `Usuario`)
- API versioning


## 🐳 Docker

Esta API possui suporte a containerização via Docker.

### Build da imagem

Na raiz do projeto:

```bash
docker build -t fcg-users-api .
```
Se quiser, pode executar o container localmente
```bash
docker run -d -p 5000:80 --name fcg-users-api fcg-users-api
```
A API ficará disponível em http://localhost:5000/swagger

## ☸ Kubernetes

Esta API faz parte da arquitetura de microserviços do projeto FiapCloudGames - 2 fase.

Orquestrada com Kubernetes e comunicação assíncrona via RabbitMQ.

Os manifests desta API estão localizados na pasta:
```bash
/k8s
```
Para realizar o deploy individual desta API no cluster:
```bash
kubectl apply -f k8s/
```

## 🔧 Setup

1. Clone o repositório:

```bash
git clone https://github.com/joseeduardoleite/fcg-users-api.git
```

2. Restaure os pacotes:
```bash
dotnet restore
```

3. Build do projeto:
```bash
dotnet build
```

## 🏃‍♂️ Executar a API
```bash
dotnet run --project FiapCloudGames.Users.Api
```
-> Os testes incluem validação de DTOs usando FluentValidation, mocks de serviços e controllers.

## Atenção
- Deve ser realizado login através da rota de login, com o usuario sugerido, que é o admin.
- Após login, pegar o token gerado e colocar no authorize pelo swagger.

## ⚡ Validação (FluentValidation)
- Todos os DTOs possuem validadores implementados.
```csharp
builder.Services.AddValidatorsFromAssemblyContaining<UsuarioValidator>();
```
- Se um DTO for inválido, a API retorna 400 Bad Request com detalhes do erro.

## 🔄 Mapping (AutoMapper)

- AutoMapper é usado para converter entre Entities e DTOs.

- Perfis são carregados automaticamente via DI.

Exemplo de mapping:

```csharp
CreateMap<Usuario, UsuarioDto>()
    .ReverseMap();
```

## 👮 Controle de acesso

- Role `Admin`: acesso total a todos os endpoints.

- Role `Usuario`: acesso restrito (ex.: apenas ao próprio recurso).

- Métodos que requerem admin possuem `[Authorize(Roles = "Admin")]`.

## 📝 Endpoints
### Usuários

### GET
`/v1/usuarios`

- Admin apenas

- Retorna todos os usuários

#### Response 200 OK:
```json
[
  {
    "id": "b6aefc4f-1e0f-4e2f-9f2f-8a3d6f8b6e72",
    "nome": "Eduardo",
    "email": "eduardo@exemplo.com",
    "role": "Admin"
  }
]
```

### GET
`/v1/usuarios/{id}`

- Admin ou proprietário

- Retorna um usuário específico

### Response 200 OK:
```json
{
  "id": "b6aefc4f-1e0f-4e2f-9f2f-8a3d6f8b6e72",
  "nome": "Eduardo",
  "email": "eduardo@exemplo.com",
  "role": "Admin"
}
```

### POST
`/v1/usuarios`

- Cria um usuário

### Request:
```json
{
  "nome": "Francisco",
  "email": "francisco@exemplo.com",
  "senha": "Senha123!",
  "role": "Usuario"
}
```


### Response 201 Created:
```json
{
  "id": "3f0a1d2c-5d0f-4a2e-9f2b-123456789abc",
  "nome": "Francisco",
  "email": "francisco@exemplo.com",
  "role": "Usuario"
}
```