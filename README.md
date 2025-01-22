# Nubk Case - Documentação do Projeto

## Introdução

Esta solução foi desenvolvida em .NET Core C#, utilizando uma abordagem baseada em DDD (Domain-Driven Design). Essa escolha facilita a organização e a clareza dos casos de uso e domínios da aplicação, seguindo também princípios de **Clean Code** para garantir legibilidade e manutenibilidade.

A estrutura do projeto está organizada em:

- **Application**: Contém as regras de negócio para cada caso de uso e domínio. Inclui também objetos auxiliares, como DTOs, Responses, e classes de execução.
- **Domain**: Contém os modelos de domínio, interfaces e agregações. Em projetos maiores, outras camadas, como API e Infrastructure, poderiam ser adicionadas.

---

## Estrutura do Projeto

```plaintext
Nubk_Case/
│
├── Application/
│   ├── ConsoleWrapper/
│   │   ├── ConsoleWrapper.cs
│   │   └── IConsoleWrapper.cs
│   ├── UseCases/
│   │   └── Operation/
│   │       ├── OperationApplication.cs
│   │       ├── OperationDto.cs
│   │       ├── OperationResponse.cs
│   │       └── OperationWeighted.cs
├── Domain/
│   └── Program.cs
│
└── Nubk_CaseTest/
    ├── IntegrationTests/
    │   └── ProgramTests.cs
    ├── UnitTests/
    │   └── OperationTests/
    │       └── OperationApplicationTest.cs
```

---

## Como Preparar o Ambiente de Execução

### 1. Instalar o .NET SDK

Caso o **.NET SDK** não esteja instalado, siga os passos abaixo para instalá-lo:

1. Acesse o site oficial do .NET:  
   [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

2. Baixe e instale a versão compatível do .NET SDK para o seu sistema operacional.

3. Após a instalação, abra o terminal e verifique se o .NET SDK foi instalado corretamente executando o comando:

   ```bash
   dotnet --version
   ```

   Caso uma versão seja exibida, o .NET SDK está configurado corretamente.

---

### 2. Instalar o Docker

Caso o **Docker** não esteja instalado, siga os passos abaixo para instalá-lo:

1. Acesse o site oficial do Docker:  
   [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop)

2. Baixe e instale o **Docker Desktop** para o seu sistema operacional.

3. Após a instalação, abra o **Docker Desktop** e certifique-se de que ele está em execução.

4. Para confirmar que o Docker foi instalado corretamente, execute o comando no terminal:

   ```bash
   docker --version
   ```

   Caso uma versão seja exibida, o Docker está configurado corretamente.

---

## Como Executar a Aplicação

### 1. Executar Localmente

1. Abra o terminal (CMD, PowerShell ou equivalente).
2. Navegue até a pasta onde o arquivo `Nubk_Case.csproj` está localizado.
3. Execute o comando:

   ```bash
   dotnet run
   ```

4. Insira o input no formato esperado e pressione **Enter**.
   - **Nota**: Certifique-se de que o texto do input está em uma única linha, pois ao copiar do PDF ele pode ficar formatado incorretamente.
5. Após o resultado ser exibido, a tela será limpa e um novo input poderá ser inserido.
6. Para encerrar a aplicação, pressione **Enter** sem inserir nenhum input.

---

### 2. Executar os Testes

1. Navegue até a pasta onde o arquivo `Nubk_CaseTest.csproj` está localizado.
2. Execute o comando:

   ```bash
   dotnet test
   ```

---

### 3. Executar em um Container Docker

1. Certifique-se de que o **Docker Desktop** está em execução.
2. Navegue até a pasta onde o arquivo `Dockerfile` está localizado.
3. Construa a imagem Docker com o comando:

   ```bash
   docker build -t nubk_case .
   ```

4. Inicie o container com o comando:

   ```bash
   docker run -it nubk_case
   ```

5. Insira o input no formato esperado e pressione **Enter**.
6. Para encerrar a aplicação, pressione **Enter** sem inserir nenhum input.

---

## Exemplos de Inputs Válidos

### Case 1
```json
[{"operation":"buy", "unit-cost":10.00, "quantity": 100}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}]
```

### Case 2
```json
[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":20.00, "quantity": 5000}, {"operation":"sell", "unit-cost":5.00, "quantity": 5000}]
```

### Case 3
```json
[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":5.00, "quantity": 5000}, {"operation":"sell", "unit-cost":20.00, "quantity": 3000}]
```

### Outros Casos
Consulte a lista completa de exemplos fornecida inicialmente.

---

## Estrutura de Desenvolvimento

O projeto utiliza DDD para separar responsabilidades, tornando o código mais claro e de fácil manutenção. As principais camadas são:

1. **Application**:
   - Regras de negócio e casos de uso.
   - Classes auxiliares como DTOs e Responses.
2. **Domain**:
   - Modelos de domínio e contratos.

Em projetos maiores, camadas adicionais como **Infrastructure** e **API** podem ser adicionadas.

---

## Tecnologias Utilizadas

- .NET Core C#
- Docker
- Testes Unitários com `dotnet test` and `xUnit`
