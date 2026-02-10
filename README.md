# 🎬 Moov Cine API

> **Back-end completo para gestão de redes de cinema, desenvolvido com .NET 8 e focado em integridade de dados e automação.**

Esta API RESTful é uma solução completa que gerencia o ciclo de vida de exibições de filmes, implementando autenticação segura, integração com serviços externos e regras de negócio complexas para garantir a consistência do sistema.

## 🛠️ Stack

* **Core:** .NET 8 SDK (C#)
* **ORM:** Entity Framework Core (Code First & Migrations)
* **Database:** MySQL / SQL Server
* **Auth:** JWT (JSON Web Tokens) com Claims-based Authorization
* **Mapping:** AutoMapper
* **Validation:** Data Annotations & Business Logic Validation
* **Architecture:** Layered Architecture (Controllers, Services, DTOs, Models)

---

## 🔐 Segurança & Controle de Acesso

O sistema implementa uma camada de segurança rigorosa utilizando **Bearer Authentication**.

* **Autenticação JWT:** Geração de tokens seguros com tempo de expiração configurável.
* **RBAC (Role-Based Access Control):**
    * `Admin`: Acesso irrestrito a endpoints de gestão, visualização de itens deletados (Soft Delete) e trigger de importações.
    * `Client`: Acesso "Read-Only" filtrado apenas para sessões futuras e filmes disponíveis.
* **Proteção de Rotas:** Uso de decorador `[Authorize(Roles = "admin")]` para blindar endpoints sensíveis contra acesso não autorizado.



---

## 🤖 Integração Automatizada com TMDB

O sistema possui um serviço de ingestão de dados integrado à API do **The Movie Database (TMDB)**, eliminando a necessidade de cadastro manual de filmes apesar de também ser possível fazê-lo.

* **Sincronização Inteligente:** Importa automaticamente filmes nas categorias *Now Playing* (Em cartaz nos cinemas) e *Upcoming* (Lançamento em breve).
* **Localização de Dados (pt-BR):** O serviço traz conteúdo localizado, baixando automaticamente:
    * Títulos traduzidos para o mercado brasileiro.
    * Sinopses completas em português.
    * URLs de Pôsteres em alta resolução.
* **Prevenção de Duplicidade:** O sistema verifica a existência do filme antes da importação para evitar registros redundantes.

---

## 🧠 Engenharia & Regras de Negócio

O diferencial do projeto está na robustez das regras que garantem a integridade do banco de dados.

### 1. Sistema Híbrido de Deleção (Smart Delete)
Implementação de uma lógica condicional para exclusão de recursos:
* **Soft Delete (Arquivamento):** Se um filme possui histórico de sessões (registros financeiros/históricos), ele é apenas marcado como excluído (`IsDeleted`), preservando a integridade referencial e dados para Analytics.
* **Hard Delete (Limpeza):** Se o filme foi cadastrado erroneamente e nunca teve sessões, o administrador pode realizar a remoção física do registro no banco de dados.
* **Confirmação em Duas Etapas:** Para evitar acidentes, o `Hard Delete` exige uma confirmação explícita via *Query Parameter* (`?force=true`), retornando um erro específico de aviso na primeira tentativa.



### 2. Validação de Conflitos de Sessão
O agendamento de sessões passa por validações rigorosas:
* **Bloqueio Temporal:** Impede criação de sessões no passado.
* **Integridade Referencial:** Impede exclusão de Cinemas que possuem sessões futuras ativas.
* **Imutabilidade de Endereço:** Bloqueia a alteração do endereço físico de um cinema caso existam ingressos/sessões futuras vendidas para aquele local.

### 3. Filtros de Consulta Dinâmicos
* **Global Query Filters:** O EF Core aplica automaticamente filtros para ignorar registros "Soft Deleted" para usuários comuns.
* **Admin Bypass:** O serviço injeta `.IgnoreQueryFilters()` condicionalmente quando a requisição provém de um administrador, permitindo auditoria de dados arquivados.

---

## 📂 Arquitetura de Dados

O projeto utiliza **DTOs (Data Transfer Objects)** para desacoplar a camada de domínio da camada de apresentação:

* **Create/Update DTOs:** Validam a entrada de dados (Required, StringLength).
* **Read DTOs:** Otimizam a saída, formatando datas e aninhando objetos relacionados para evitar *Over-fetching* ou *Under-fetching* no Front-end.


*Desenvolvido por Gabriel Caldeira*