## :computer: Projeto API - Gatherly
API disponibilizada em RESTful .Net 6.0 para controle de eventos

## :building_construction: Arquitetura em camadas
- **Application**: CQRS, PipelineBehavior para validação de comandos, Command, Queries e Events com base no CQRS
- **Domain**: Entidades, Enums, Value Objects, Interfaces e Classes compartilhadas entre todo o ecossistema.
- **Persistence**: Camada de acesso a dados com banco de dados Sql e Nosql e afins.
- **Infrastructure**: Responsável por cuidar da parte de autenticação, serviços externos, background jobs (Quartz) e cache (Redis)
- **Domain.UnitTests**: Testes únitarios para classes de entidades.
- **Application.UnitTests**: Testes únitarios para commands and validators.

## :pushpin: Back-end
- Fluent Validation
- Entity Framework Core 6
- SQL Server
- Repository
- CQRS

## :zap: Running
- SSMS (SQL SERVER)
- Redis
