
# Sistema de Academia - Jiu-Jitsu 🥋

Este projeto é um sistema de gerenciamento para academias de jiu-jítsu, desenvolvido com a arquitetura MVC em C# utilizando .NET e SQL Server. O sistema permite o cadastro e a gestão de alunos, instrutores, aulas e pagamentos.
## Funcionalidades 🛠️

- Cadastro de Alunos
- Cadastro de Instrutores
- Gestão de Aulas
- Controle de Pagamentos
- Visualização de histórico de aulas e pagamentos

## Tecnologias Utilizadas 💻
- C#
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- HTML/CSS/JavaScript (para o front-end)
## Como Executar o Projeto ⚙

Clone o repositório:

```bash
  git clone https://github.com/seu-usuario/Academia-JiuJitsu.git
```

Navegue até o diretório do projeto:

```bash
  cd Parioca-Fight
```

Configure o banco de dados no arquivo appsettings.json:

```bash
"ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=NOME_BANCO;User Id=SEU_USUARIO;Password=SUA_SENHA;"
}
```
Execute as migrações para criar o banco de dados

```bash
  dotnet ef database update
```

Execute o projeto:

```bash
dotnet run
```
# Estrutura do Banco de Dados 🧱🧱

![Estrutura do Banco de Dados](https://raw.githubusercontent.com/Chris7ianp/Academia-JiuJitsu/refs/heads/master/PariocaFight/wwwroot/imagens/estrutura-bd.jpg?token=GHSAT0AAAAAACYU7NGPS37YD4DMOIRQWFCAZYFXBOQ)


