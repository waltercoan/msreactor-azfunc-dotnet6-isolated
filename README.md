# Microsoft Reactor 27-01-2022
## Azure Function(.Net 6 Isolated) - Storage Account - Cosmos DB 
### Walter Silvestre Coan - www.waltercoan.com.br

O objetivo deste exemplo é demonstrar como fazer o uso das Azure Functions no modo Isolated com .Net 6.0, processando eventos de upload de arquivos em uma Conta de Armazenamento para criar miniaturas das imagens e extrair metadados de localização GPS e armazenar um banco de dados no Cosmos DB.

### O que são as Azure Functions Isolated
Fonte: https://docs.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?WT.mc_id=AZ-MVP-5003638

- As Azure Functions Isolated é uma forma de executar uma Azure Function como um processo separado do host process do .NET Function
- Permite maior liberdade para utilizar novas versões do SDK como o .NET 5.0/6.0
- Controle total sobre o ciclo de vida da Function
- Controle sobre o mecanismo de injeção de dependências
- Estrutura básica do projeto
    - arquivo host.json
    - arquivo local.settings.json
    - arquivo .csproj
    - arquivo Program.cs
- Pacotes
    - Microsoft.Azure.Functions.Worker
    - Microsoft.Azure.Functions.Worker.Sdk
- Bindings
    - Atributos
    - Parametros em métodos
    - Tipo de retorno dos métodos
- HTTP Triggers
    - Suporta todos os Triggers de uma Azure Function comum

### Criação do projeto
Fonte: https://github.com/Azure/azure-functions-dotnet-worker?WT.mc_id=AZ-MVP-5003638

### Pontos importantes
- Pacotes instalados
    - SixLabors.ImageSharp
    - MetadataExtractor
    - Microsoft.AspNetCore.Identity.EntityFrameworkCore 
    - Microsoft.Azure.Functions.Worker.Extensions.Storage
    - Microsoft.EntityFrameworkCore.Cosmos
- FuncDbContext
    - FuncTumbImgBlobTrigger
        - Modelo padrão de uma static class
        - BlobTrigger
            - Recebe o nome do container e o nome da configuração que contém a string de conexão para a conta de armazenamento
            - IMPORTANTE: no .NET 6 usar o byte[] para receber o conteúdo do blob, e não a classe Stream
        - BlobOutput grava automaticamente o binário em outro container de saída
    - FuncGeoDataImgBlobTrigger
        - Foi utilizada uma classe não static para suportar a injeção de dependência
            - Configurada como um serviço no arquivo Program.cs
        - Entity Framework Core - FuncDbContext
            - Foi utilizando o EF Core para persistir os dados em um banco no CosmosDB
            - É importante no método OnModelCreating, configurar o EF Core para compreender o schema dos documentos gravados
        - BlobTrigger
            - Mesmo evento de gravação do arquivo na conta de armazenamento
            - A propriedade definida como chave de partição precisa ter o valor atribuído
            - É possível recuperar os metadados do arquivo na conta de armazenamento
- Cosmos DB
    - Banco de Dados: dbazfuncdotnet6reactor
        - Container: PhotoItem
            - Partition Key: RowKey

Fontes 
- https://docs.microsoft.com/pt-br/ef/core/providers/cosmos/?tabs=dotnet-core-cli?WT.mc_id=AZ-MVP-5003638

- https://github.com/dotnet/EntityFramework.Docs/tree/main/samples/core/Cosmos/ModelBuilding?WT.mc_id=AZ-MVP-5003638