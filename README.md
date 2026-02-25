# 🎬 Moov Cine API

> **Back-end completo para gestão de redes de cinema, focado em integridade de dados, conteinerização e automação.**

Esta API RESTful é uma solução completa que gerencia o ciclo de vida de exibições de filmes, implementando autenticação segura, integração com serviços externos e regras de negócio complexas para garantir a consistência do sistema.

## 🛠️ Stack & Infraestrutura

* **Core:** .NET 8 SDK (C#)
* **ORM:** Entity Framework Core (Code First & Migrations)
* **Database:** MySQL 8.0 orquestrado via Docker
* **Auth:** JWT (JSON Web Tokens) com Claims-based Authorization
* **Mapping:** AutoMapper
* **Validation:** Data Annotations & Business Logic Validation
* **Serialização:** Otimizada para **CamelCase** via `NewtonsoftJson`, garantindo integração nativa com Front-ends modernos.
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

O sistema possui um serviço de ingestão de dados integrado à API do **The Movie Database (TMDB)**, eliminando a necessidade de cadastro manual de filmes, apesar de também ser possível fazê-lo.

* **Sincronização Inteligente:** Importa automaticamente filmes nas categorias *Now Playing* (Em cartaz) e *Upcoming* (Lançamentos).
* **Localização de Dados (pt-BR):** Traz conteúdo localizado, baixando títulos traduzidos, sinopses completas e URLs de pôsteres em alta resolução.
* **Métricas de Sucesso:** Captura a propriedade `Popularity` em tempo real para alimentar o motor de agendamento de sessões.
* **Prevenção de Duplicidade:** O sistema verifica a existência do filme antes da importação para evitar registros redundantes.

---

## 🛡️ Engenharia & Regras de Negócio

O diferencial do projeto está na solidez das regras que garantem a integridade do banco de dados.

### 1. Sistema Híbrido de Deleção (Smart Delete)
Implementação de uma lógica condicional para exclusão de recursos:
* **Soft Delete (Arquivamento):** Se um filme possui histórico de sessões, ele é apenas marcado como excluído, por meio do preenchimento da propriedade (`DataExclusao`), preservando a integridade referencial e dados para Analytics. É possível consultar o Id do administrador que efetuou a exclusão.
* **Hard Delete (Limpeza):** Se o filme foi cadastrado erroneamente e nunca teve sessões, permite a remoção física do registro no banco. Exige confirmação explícita via *Query Parameter* (`?force=true`).

### 2. Validação de Conflitos de Sessão
O agendamento de sessões passa por validações rigorosas:
* **Otimização em RAM:** Utiliza o `.Local` do EF Core para validar conflitos em memória, eliminando milhares de queries redundantes durante a geração em massa.
* **Bloqueio Temporal e Físico:** Impede criação de sessões no passado, bloqueia a exclusão de cinemas com sessões futuras ativas e impede a alteração de endereço de cinemas com ingressos vendidos.

### 3. Filtros de Consulta Dinâmicos
* **Global Query Filters:** O EF Core ignora automaticamente registros "Soft Deleted" para usuários comuns.
* **Admin Bypass:** Injeção de `.IgnoreQueryFilters()` condicionalmente quando a requisição provém de um administrador.

---

## 📂 Arquitetura de Dados

O projeto utiliza **DTOs (Data Transfer Objects)** para desacoplar a camada de domínio da camada de apresentação:

* **Create/Update DTOs:** Validam a entrada de dados (Required, StringLength).
* **Read DTOs:** Otimizam a saída, formatando datas e aninhando objetos relacionados para evitar *Over-fetching* ou *Under-fetching* no Front-end.

---

## 🍒 A Cereja do Bolo: Agendamento Inteligente

O projeto vai além de um CRUD tradicional ao apresentar o `GerarSessoesAutomaticamente`, um algoritmo proprietário de curadoria que simula o comportamento de grandes redes cinematográficas.

* **Curadoria Baseada em Popularidade:** Utiliza os dados do TMDB para dar prioridade de exibição aos grandes sucessos de bilheteria.
* **Lógica Matemática Avançada:** Para evitar que grandes lançamentos monopolizem todas as salas, o algoritmo aplica uma escala logarítmica:

$$\text{bônus} = \log_{10}(\max(1, \text{popularidade})) \times 20$$

* **Escalabilidade por Cinema:** A "base de exclusão" é calculada dinamicamente pelo `NumeroSalas`. Cinemas pequenos têm curadoria rígida; grandes complexos oferecem variedade.
* **Segurança Lógica (`Math.Clamp`):** As probabilidades são travadas entre **5% e 95%**, garantindo margem de erro e realismo (nenhum agendamento é matematicamente impossível ou 100% garantido).
* **Consistência Semanal:** Uma hierarquia de loops estruturada garante que um filme mantenha horários fixos durante a semana de exibição, facilitando a fidelização do público.
