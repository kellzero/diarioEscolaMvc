# 📚 Diário Escolar MVC

Aplicação web desenvolvida em **ASP.NET Core MVC** para gerenciamento de alunos e notas escolares, com foco em organização, usabilidade e boas práticas de desenvolvimento.

O projeto segue o padrão **MVC (Model-View-Controller)** e implementa um relacionamento **1:N entre Alunos e Diário de Notas**.

---

## 🚀 Funcionalidades

### 👨‍🎓 Gerenciamento de Alunos
- ✅ Cadastro completo de alunos com:
  - Nome completo
  - Turma (dropdown com opções pré-definidas)
  - Data de nascimento (com cálculo automático de idade)
  - Ano de nascimento (armazenado separadamente no banco)

### 📝 Gerenciamento de Notas (Diário)
- ✅ Registro de notas por aluno e matéria
- ✅ Cálculo automático de média
- ✅ Determinação automática de situação (Aprovado/Recuperação)
- ✅ Visualização de notas com nome do aluno via JOIN

### 🔄 Integração Aluno-Diário
- ✅ **Formulário único** para cadastro de aluno + nota (ViewModel)
- ✅ Relacionamento 1:N (um aluno pode ter várias notas)
- ✅ Exibição do nome do aluno na listagem de notas
- ✅ Filtro de alunos em recuperação (média < 6)
- ✅ Exclusão de notas diretamente na visualização do diário

### 📊 Interface
- ✅ Design responsivo com Bootstrap
- ✅ Ícones com Font Awesome
- ✅ Feedback visual com cores (verde = aprovado, amarelo = recuperação)
- ✅ Preview em tempo real da idade e média no formulário

---

## 🛠️ Tecnologias Utilizadas

- **C# / .NET 8.0**
- **ASP.NET Core MVC**
- **ADO.NET** (acesso direto ao banco, sem Entity Framework)
- **SQL Server / LocalDB**
- **HTML5 / CSS3 / Bootstrap 5**
- **JavaScript / jQuery**
- **Font Awesome 6**

---
### ?? Passo a passo

# 1. Clone o repositório
git clone https://github.com/kellzero/diarioEscolaMvc.git

# 2. Acesse a pasta do projeto
cd diarioEscolaMvc

# 3. Configure a connection string no appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=escola_gestao_dev;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}

# 4. Execute o script SQL para criar as tabelas
# (disponível na documentação)

# 5. Execute o projeto
dotnet run
# ou abra o .sln no Visual Studio e pressione F5
## ?? Autor

Kelvin Camilo Ferreira
GitHub: https://github.com/kellzero
