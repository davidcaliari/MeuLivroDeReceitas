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


---

## Como Executar a Aplicação

### 1. Executar Localmente

1. Abra o terminal (CMD, PowerShell ou equivalente).
2. Navegue até a pasta onde o arquivo `Nubk_Case.csproj` está localizado.
3. Execute o comando:

   ```bash
   dotnet run

-------------------------------------------------------

#Documentação do proejto
Para a solução proposta no case eu escolhi a stack .net core C# por ter mais familiaridade.
Para arquitetura da solução foi criado utilizando DDD por dar mais clareza em relação aos caso de uso e dominios da da aplicação, assim como clean code para apoiar na clareza organização e legibilidade do código fazendo a separação da aplicação 
nas seguintes pasta:
Application: Ira conter as regras de negócio de cada caso de uso e de cada dominio, nela também é criado tudo aquilo necessário para a chamada da mesma tal como Dto, Response e a propria application de execução.
Domain: Ira conter os modelos de dominio, interfaces, modelos de agregação.
Em projetos mais complexos ainda teriamos outras camadas como API, Infrastructure entre outras

#Documentação para execução do Projeto

Para executar a aplicação siga os passos abaixo:
1 - Executar o CMD na pasta onde se encontrato o projeto "Nubk_Case.csproj"
2 - Executar o comando: dotnet run
3 - Agora é só colar o input e pressionar enter, porém o texto do input deve estar em uma linha, ao copiar do PDF o mesmo fica errado, mais abaixo tem exemplos de input. 
4 - Ao pressionar enter apoós o resultado a tela será limpa e permitirá incluir um novo input
5 - Caso seja pressionado enter sem passar qualquer input a aplicação irá se encerrar

Para executar os testes unitario siga os passos abaixo?
1 - Executar o CMD na pasta onde se encontrato o projeto de testes "Nubk_CaseTest.csproj"
2 - Executar o comando: dotnet test

Para executar a aplicação dentro de um container siga os passos abaixo:
1 - Garantir que o Docker Desktop esta rodando
2 - Executar o CMD na pasta onde se encontrato o arquivo "Dockerfile"
3 - Executar o comando: docker build -t NubankCase .
4 - Executar o comando: docker run
5 - Agora é só colar o input e pressionar enter, porém o texto do input deve estar em uma linha, ao copiar do PDF o mesmo fica errado, mais abaixo tem exemplos de input.
6 - Ao pressionar enter apoós o resultado a tela será limpa e permitirá incluir um novo input
7 - Caso seja pressionado enter sem passar qualquer input a aplicação irá se encerrar

Segue exemplos de inputs corretos:
Case 1
[{"operation":"buy", "unit-cost":10.00, "quantity": 100}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}]

Case 2
[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":20.00, "quantity": 5000}, {"operation":"sell", "unit-cost":5.00, "quantity": 5000}]

Case 1 + Case 2
[{"operation":"buy", "unit-cost":10.00, "quantity": 100}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}] [{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":20.00, "quantity": 5000}, {"operation":"sell", "unit-cost":5.00, "quantity": 5000}]

Case 3
[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":5.00, "quantity": 5000}, {"operation":"sell", "unit-cost":20.00, "quantity": 3000}]

Case 4
[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"buy", "unit-cost":25.00, "quantity": 5000}, {"operation":"sell", "unit-cost":15.00, "quantity": 10000}]

Case 5
[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"buy", "unit-cost":25.00, "quantity": 5000}, {"operation":"sell", "unit-cost":15.00, "quantity": 10000}, {"operation":"sell", "unit-cost":25.00, "quantity": 5000}]

Case 6
[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":2.00, "quantity": 5000}, {"operation":"sell", "unit-cost":20.00, "quantity": 2000}, {"operation":"sell", "unit-cost":20.00, "quantity": 2000}, {"operation":"sell", "unit-cost":25.00, "quantity": 1000}]

Case 7
[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":2.00, "quantity": 5000}, {"operation":"sell", "unit-cost":20.00, "quantity": 2000}, {"operation":"sell", "unit-cost":20.00, "quantity": 2000}, {"operation":"sell", "unit-cost":25.00, "quantity": 1000}, {"operation":"buy", "unit-cost":20.00, "quantity": 10000}, {"operation":"sell", "unit-cost":15.00, "quantity": 5000}, {"operation":"sell", "unit-cost":30.00, "quantity": 4350}, {"operation":"sell", "unit-cost":30.00, "quantity": 650}]

Case 8
[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":50.00, "quantity": 10000}, {"operation":"buy", "unit-cost":20.00, "quantity": 10000}, {"operation":"sell", "unit-cost":50.00, "quantity": 10000}]


=======
## Build
```
docker-compose up -d --build
```


## Run
```
docker-compose run nubkteste01
```
