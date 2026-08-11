# Gerador de Loterias CAIXA

Aplicacao desktop em .NET 8 (WinForms) para gerar jogadas unicas de varias loterias da CAIXA, com arquitetura em camadas e codigo orientado a Clean Code.

## Objetivo

Permitir que o usuario:

- Escolha a modalidade da loteria.
- Informe quantidade de numeros por jogada (respeitando as regras da modalidade).
- Informe quantidade de jogadas.
- Gere jogadas unicas, ordenadas e formatadas.

## Modalidades suportadas

- Mega-Sena: 6 a 20 numeros entre 1 e 60
- Lotofacil: 15 a 20 numeros entre 1 e 25
- Quina: 5 a 15 numeros entre 1 e 80
- Lotomania: 50 numeros entre 1 e 100
- Dupla Sena: 6 a 15 numeros entre 1 e 50
- Timemania: 10 numeros entre 1 e 80 (sem Time do Coracao)
- Dia de Sorte: 7 a 15 numeros entre 1 e 31 (sem Mes da Sorte)

## Arquitetura

O projeto foi reorganizado com separacao de responsabilidades inspirada em Clean Architecture e com fluxo MVC na camada de apresentacao.

### Camadas

- Domain
	- Entidades e regras puras de negocio.
	- Servicos de dominio (geracao de jogadas e matematica combinatoria).
- Application
	- Casos de uso e contratos.
	- Orquestracao de validacoes e execucao da regra de negocio.
- Infrastructure
	- Implementacoes concretas para dados/configuracoes (catalogo de jogos).
- Presentation
	- View em WinForms.
	- Controller que reage a eventos da UI e aciona o caso de uso.

### Padroes utilizados

- MVC (na apresentacao): MainForm + MainController.
- Use Case (Application): GeneratePlaysUseCase.
- Repository-like Catalog (Application/Infrastructure): ILotteryGameCatalog + LotteryGameCatalog.
- Composition Root (Program): montagem explicita das dependencias.

## Estrutura de pastas

```text
Gerador.Mega.Sena/
	Application/
		Abstractions/
			ILotteryGameCatalog.cs
		UseCases/
			GeneratePlaysUseCase.cs
	Domain/
		Entities/
			LotteryGame.cs
		Services/
			CombinationMath.cs
			UniquePlayGenerator.cs
	Infrastructure/
		Catalog/
			LotteryGameCatalog.cs
	Presentation/
		Controllers/
			MainController.cs
		Views/
			IMainView.cs
			MainForm.cs
	Program.cs
```

## Como executar

### Pre-requisitos

- .NET SDK 8.0 ou superior
- Windows (projeto WinForms)

### Rodar localmente

Na raiz do repositorio:

```bash
dotnet run --project Gerador.Mega.Sena/Gerador.Mega.Sena.csproj
```

### Build

```bash
dotnet build Gerador.Mega.Sena/Gerador.Mega.Sena.csproj
```

## Regras e validacoes

- Nao permite jogadas fora dos limites da modalidade.
- Nao permite quantidade de jogadas menor que 1.
- Verifica limite combinatorio antes de tentar gerar todas as jogadas.
- Retorna aviso quando nao consegue concluir 100% das jogadas dentro do limite de tentativas.

## Qualidade do codigo

- Classes pequenas e coesas.
- Nomes explicitos.
- Dependencias invertidas via interfaces na camada Application.
- Documentacao XML em classes principais.
- Regra de negocio isolada da UI.

## Proximos passos sugeridos

- Adicionar testes unitarios para Domain e Application.
- Adicionar exportacao para TXT/CSV.
- Adicionar Time do Coracao (Timemania) e Mes da Sorte (Dia de Sorte).
- Criar instalador/publicacao self-contained.

## Licenca

Este projeto esta sob a licenca descrita em LICENSE.

## Aviso

Projeto com finalidade educacional e de entretenimento.
