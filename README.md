🏋️ AppAcademia
Sistema web para gestão de academias, desenvolvido em ASP.NET Core MVC, com controle de usuários, treinos, exercícios e acompanhamento de desempenho.
Projeto focado em boas práticas de arquitetura, segurança, controle de acesso por perfil e experiência do usuário.

________________________________________
📌 Visão Geral
O AppAcademia permite que administradores, instrutores e alunos utilizem a plataforma de forma integrada:
•	Administradores gerenciam o sistema
•	Instrutores criam e acompanham treinos
•	Alunos executam treinos e acompanham seu progresso semanal
O sistema foi desenvolvido com foco em escalabilidade, organização de código e padrões utilizados em aplicações SaaS.

________________________________________
👥 Perfis de Usuário

🔐 Administrador
•	Dashboard com visão geral do sistema
•	Gestão de instrutores
•	Acompanhamento de alunos, treinos e exercícios

🏋️ Instrutor
•	Dashboard com métricas próprias
•	Gerenciamento de alunos
•	Criação, edição e controle de treinos
•	Bloqueio de edição/exclusão de treinos já iniciados

🧍 Aluno
•	Treino do dia
•	Marcação de exercícios concluídos
•	Histórico semanal com datas reais
•	Percentual de progresso semanal
•	Feedback visual de desempenho

________________________________________
⚙️ Funcionalidades Principais
•	✅ Autenticação segura (hash de senha)
•	✅ Controle de acesso por perfil (Admin / Instrutor / Aluno)
•	✅ CRUD completo de treinos e exercícios
•	✅ Soft delete de treinos
•	✅ Histórico semanal de exercícios
•	✅ Dashboard com indicadores
•	✅ Interface responsiva com Bootstrap
•	🚧 JWT (em implementação / roadmap)

________________________________________
🔐 Segurança
•	Hash de senha utilizando bcrypt
•	Validação de perfil por role
•	Proteção de rotas sensíveis
•	Preparado para autenticação via JWT

________________________________________
🧱 Arquitetura
•	ASP.NET Core MVC
•	Entity Framework Core
•	Padrão MVC
•	ViewModels para isolamento de regras
•	Camada de dados desacoplada
•	Boas práticas de null safety

________________________________________
🗄️ Tecnologias Utilizadas
•	ASP.NET Core MVC
•	Entity Framework Core
•	SQL Server
•	Bootstrap 5
•	C#
•	HTML / CSS
•	JavaScript

________________________________________
📊 Dashboards
•	📈 Dashboard do Administrador
•	📉 Dashboard do Instrutor
•	📅 Histórico e desempenho do aluno
•	🧮 Métricas de progresso semanal
________________________________________

🚀 Como Executar o Projeto
Pré-requisitos
•	.NET SDK 7 ou superior
•	SQL Server
•	Visual Studio ou VS Code
Passos
git clone https://github.com/seu-usuario/AppAcademia.git
cd AppAcademia
dotnet restore
dotnet ef database update
dotnet run
Acesse:
http://localhost:5186

________________________________________
🧪 Dados de Teste
Perfis disponíveis:
•	Administrador
•	Instrutor
•	Aluno
(Usuários podem ser criados via banco ou interface administrativa)

________________________________________
🛣️ Roadmap
•	🔑 Autenticação JWT
•	🔄 Refresh Token
•	📱 API para app mobile
•	📊 Gráficos de desempenho
•	📅 Planejamento mensal de treinos
•	📨 Notificações

________________________________________
👨‍💻 Desenvolvedor
Luciano Silva Ferreira
Desenvolvedor de Software
•	💼 ASP.NET Core | C#
•	🔐 Segurança e autenticação
•	📊 Sistemas de gestão e SaaS
🔗 GitHub: https://github.com/LucianoSF1992
🔗 LinkedIn: https://www.linkedin.com/in/lucianoferreira92/

________________________________________
📄 Licença
Este projeto é de uso educacional e demonstrativo.
Sinta-se à vontade para estudar, adaptar e evoluir.
________________________________________

⭐ Considerações Finais

Este projeto demonstra:
•	Organização de código
•	Visão de produto
•	Boas práticas reais de mercado
•	Capacidade de evoluir um sistema complexo

