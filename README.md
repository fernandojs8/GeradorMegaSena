# Gerador de Loterias CAIXA

Aplicacao desktop em .NET 8 (WinForms) para gerar jogadas unicas de varias loterias da CAIXA, com arquitetura em camadas e codigo orientado a Clean Code.

## Objetivo

Permitir que o usuario:

- Escolha a modalidade da loteria.
- Escolha o idioma da interface (English, Portugues, Francais, Espanhol, Alemao).
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
	- Implementacoes concretas para dados/configuracoes (catalogo de jogos e localizacao).
- Presentation
	- View em WinForms.
	- Controller que reage a eventos da UI e aciona o caso de uso.

### Padroes utilizados

- MVC (na apresentacao): MainForm + MainController.
- Use Case (Application): GeneratePlaysUseCase.
- Repository-like Catalog (Application/Infrastructure): ILotteryGameCatalog + LotteryGameCatalog.
- Localization Service (Application/Infrastructure): ILocalizationService + InMemoryLocalizationService.
- Composition Root (Program): montagem explicita das dependencias.

## Estrutura de pastas

```text
Gerador.Mega.Sena/
	Application/
		Abstractions/
			ILotteryGameCatalog.cs
			ILocalizationService.cs
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
		Localization/
			InMemoryLocalizationService.cs
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

### Testes unitarios

```bash
dotnet test Gerador.Mega.Sena.sln
```

## Regras e validacoes

- Nao permite jogadas fora dos limites da modalidade.
- Nao permite quantidade de jogadas menor que 1.
- Nao permite modalidade vazia ou invalida.
- Nao permite quantidade de jogadas acima do limite de seguranca (100.000).
- Verifica limite combinatorio antes de tentar gerar todas as jogadas.
- Retorna aviso quando nao consegue concluir 100% das jogadas dentro do limite de tentativas.

## Testes e edge cases

A suite de testes cobre:

- Dominio matematico:
	- argumentos invalidos na combinacao limitada
	- retorno com short-circuit no limite configurado
	- validacao de valores conhecidos de combinacao
- Gerador de jogadas:
	- garantia de unicidade entre jogadas
	- validacao de quantidade de numeros por jogada
	- validacao de faixa numerica e ordenacao do resultado
- Caso de uso:
	- modalidade inexistente
	- estouro de limite de seguranca
	- pedido combinatoriamente impossivel
	- fluxo feliz com retorno de sucesso
- Idioma/localizacao:
	- idiomas disponiveis esperados
	- traducao aplicada ao trocar idioma
	- fallback para idioma padrao em codigo invalido

Projeto de testes: Gerador.Mega.Sena.Tests

## Consideracoes de seguranca

- Limite de seguranca para quantidade de jogadas evita uso abusivo de CPU/memoria.
- Validacao defensiva de entrada no caso de uso para evitar requests malformados.
- Regra combinatoria executada antes da geracao para reduzir risco de loops longos.
- Aplicacao nao utiliza rede, credenciais ou persistencia de dados sensiveis.

## Qualidade do codigo

- Classes pequenas e coesas.
- Nomes explicitos.
- Dependencias invertidas via interfaces na camada Application.
- Documentacao XML em classes principais.
- Regra de negocio isolada da UI.

## Exportacao

As jogadas geradas podem ser salvas diretamente da interface clicando em **Exportar**:

- **TXT**: formato legivel com cabecalho e numeracao de jogadas.
- **CSV**: formato tabulado para importar em planilhas (colunas: Jogo, NumerosJogada, Numeros).

## Publicacao self-contained

Para gerar um executavel unico sem dependencia do .NET instalado:

```bash
dotnet publish Gerador.Mega.Sena/Gerador.Mega.Sena.csproj \
  /p:PublishProfile=win-x64-self-contained
```

O arquivo sera gerado em `Gerador.Mega.Sena/publish/win-x64/`.

## Proximos passos sugeridos

Todos os passos anteriores foram implementados. Possiveis melhorias futuras:

- Adicionar suporte a mais loterias (ex.: Super Sete, +Milionaria).
- Persistir configuracoes do usuario entre sessoes.
- Adicionar tema escuro na interface.

## Licenca

Este projeto esta sob a licenca descrita em LICENSE.

## Aviso

Projeto com finalidade educacional e de entretenimento.
