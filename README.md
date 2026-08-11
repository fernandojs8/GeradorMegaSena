# Gerador Mega Sena

Gerador de jogos aleatorios para Mega Sena via console, com foco em simplicidade, rapidez e jogadas sem repeticao.

## O que este projeto faz

Este app permite:

- Informar quantos numeros cada jogo deve ter.
- Informar quantas jogadas deseja gerar.
- Gerar jogadas com numeros unicos dentro do intervalo de 1 a 60.
- Evitar jogadas duplicadas na mesma execucao.
- Exibir os numeros ordenados e formatados (ex.: `01 - 07 - 19 - 33 - 44 - 60`).

## Features

- Aplicacao console em C# (.NET 8).
- Interface textual com animacao leve de digitacao.
- Validacao de entrada (somente inteiros positivos).
- Validacao matematica para garantir que a quantidade solicitada de jogadas unicas e possivel.
- Gera jogos sem repeticao de numero dentro de cada jogada.

## Tecnologias

- C#
- .NET 8

## Estrutura do projeto

```text
GeradorMegaSena/
|-- Gerador.Mega.Sena.sln
|-- Gerador.Mega.Sena/
|   |-- Gerador.Mega.Sena.csproj
|   |-- Program.cs
|-- README.md
|-- LICENSE
```

## Como executar

### Pre-requisitos

- .NET SDK 8.0 ou superior instalado

### Rodando no terminal

Na raiz do repositorio:

```bash
dotnet run --project Gerador.Mega.Sena/Gerador.Mega.Sena.csproj
```

Opcionalmente:

```bash
cd Gerador.Mega.Sena
dotnet run
```

## Exemplo de uso

```text
Por favor me diga quantos numeros voce precisa agora?
6

Por favor me diga quantas jogadas voce precisa agora?
3

>> 03 - 08 - 14 - 26 - 39 - 55 <<
>> 01 - 10 - 18 - 33 - 45 - 60 <<
>> 05 - 12 - 21 - 34 - 46 - 58 <<
```

## Regras e validacoes

O programa protege contra cenarios invalidos:

- Nao aceita entrada vazia, texto ou numero menor/igual a zero.
- Nao permite pedir mais numeros por jogo do que o intervalo comporta (maximo 60).
- Verifica se a quantidade de jogadas unicas solicitada e matematicamente possivel.

Exemplo:

- Se pedir 61 numeros em um jogo, o programa encerra com mensagem de erro.
- Se pedir mais jogadas unicas do que as combinacoes disponiveis para aquele tamanho de jogo, tambem encerra com aviso.

## Como funciona por dentro

1. O intervalo base e de 1 a 60.
2. Para cada jogada, o programa embaralha parcialmente uma lista de 1 a 60.
3. Seleciona os `k` primeiros numeros (sem repeticao dentro da jogada).
4. Ordena os numeros para exibicao.
5. Usa um `HashSet` para garantir que uma jogada inteira nao se repita.

### Controle de combinacoes

Antes de gerar, o app estima se existe espaco combinatorio suficiente usando combinacoes:

$$
\binom{n}{k} = \frac{n!}{k!(n-k)!}
$$

Onde:

- $n = 60$ (tamanho do universo)
- $k = quantidade\ de\ numeros\ por\ jogada$

Isso evita loops longos quando o usuario pede mais jogadas unicas do que o possivel.

## Curiosidade divertida

Nenhum gerador aumenta sua chance real de ganhar alem das regras da loteria, mas ele pode:

- poupar tempo,
- evitar erro manual,
- e deixar o ritual de escolha dos numeros mais divertido.

Em resumo: sorte nao se programa... mas um bom gerador sim.

## Ideias para evolucao

- Exportar jogadas para TXT/CSV.
- Permitir escolher intervalo customizado.
- Historico de jogadas geradas.
- Modo rapido (sem animacao).
- Testes automatizados para regras e funcoes matematicas.

## Contribuicao

Contribuicoes sao bem-vindas.

Fluxo sugerido:

1. Faca um fork.
2. Crie uma branch para sua feature (`feature/minha-melhoria`).
3. Commit suas alteracoes.
4. Abra um Pull Request.

## Licenca

Este projeto esta sob a licenca descrita no arquivo `LICENSE`.

## Aviso

Este projeto tem finalidade educacional e de entretenimento.
