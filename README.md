# API JWT com ASP.NET Core

## Descrição

Este projeto é uma API REST desenvolvida em ASP.NET Core utilizando autenticação e autorização com JSON Web Token (JWT).

O objetivo da aplicação é demonstrar o funcionamento de autenticação baseada em tokens, permitindo acesso a rotas públicas e protegidas.

## Tecnologias Utilizadas

* .NET 8
* ASP.NET Core Minimal API
* JWT (JSON Web Token)
* C#
* Visual Studio Code

## Funcionalidades

* Login com geração de token JWT
* Endpoint público
* Endpoint protegido por autenticação
* Endpoint protegido por regra de autorização baseada em cargo (Role)

---

## Estrutura do Projeto

```text
MinhaApiJwt/
│
├── Program.cs
├── appsettings.json
├── MinhaApiJwt.csproj
└── README.md
```

---

## Configuração

No arquivo `appsettings.json`, configure os parâmetros do JWT:

```json
{
  "Jwt": {
    "Issuer": "MinhaApi",
    "Audience": "MinhaApi",
    "Key": "sua_chave_secreta_aqui_muito_segura_e_comprida"
  }
}
```

---

## Instalação

Clone o repositório:

```bash
git clone https://github.com/seu-usuario/MinhaApiJwt.git
```

Acesse a pasta do projeto:

```bash
cd MinhaApiJwt
```

Instale as dependências:

```bash
dotnet restore
```

Execute a aplicação:

```bash
dotnet run
```

---

## Endpoints

### Endpoint Público

**GET**

```http
/publico
```

Resposta:

```json
"Este endpoint é público e pode ser acessado por qualquer pessoa."
```

---

### Login

**POST**

```http
/login
```

Body:

```json
{
  "username": "professor",
  "password": "123456"
}
```

Resposta:

```json
{
  "token": "seu_token_jwt"
}
```

---

### Endpoint Protegido

**GET**

```http
/protegido
```

Necessário enviar o token JWT no cabeçalho Authorization:

```text
Bearer seu_token_jwt
```

Resposta:

```json
"Você acessou uma rota protegida!"
```

---

### Endpoint Exclusivo para Professor

**GET**

```http
/professor/aulas
```

Necessário enviar um token contendo a Role:

```text
Professor
```

Resposta:

```json
"Lista de aulas do professor"
```

---

## Testes

A aplicação pode ser testada utilizando:

* Postman
* Insomnia
* Thunder Client (VS Code)

---

## Autor

Ulisses Fernandes

Projeto acadêmico desenvolvido para a disciplina de Desenvolvimento Back-End.
