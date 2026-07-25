using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Gerador.Mega.Sena
{
    class Program
    {
        private const string Indentacao = "     ";
        private const int AtrasoAnimacaoMs = 12;
        private const int AtrasoMenorMs = 800;
        private const int AtrasoPequenoMs = 1800;
        private const int MultiplicadorMaximoTentativas = 200;

        static void Main(string[] args)
        {
            var random = new Random();

            PulaLinha();
            PulaLinha();
            PulaLinha();
            PulaLinha();

            var caracteres = "     -= Gerador de números aleatórios para a ganhar na Mega Sena! =-".ToCharArray();

            EscreverLinha(caracteres);
            PulaLinha();
            PulaLinha();
            PequenaPausa();

            int menorNumero = 1;
            int maiorNumero = 60;

            int quantidadeDeNumeros = LerInteiroPositivo("Por favor me diga quantos números você precisa agora?");

            PulaLinha();

            int quantidadeDeJogadas = LerInteiroPositivo("Por favor me diga quantas jogadas você precisa agora?");

            int tamanhoIntervalo = (maiorNumero - menorNumero) + 1;

            if (quantidadeDeNumeros > tamanhoIntervalo)
            {
                PulaLinha();
                caracteres = "     Não é possível gerar números únicos nessa quantidade para o intervalo informado.".ToCharArray();
                EscreverLinha(caracteres);
                PulaLinha();
                Console.WriteLine("     Pressione qualquer tecla para sair...");
                Console.Read();
                return;
            }

            if (!PodeGerarQuantidadeSolicitada(tamanhoIntervalo, quantidadeDeNumeros, quantidadeDeJogadas, out long maximoDeJogadas))
            {
                PulaLinha();
                caracteres = $"     Não é possível gerar {quantidadeDeJogadas} jogadas únicas. Máximo possível: {maximoDeJogadas}.".ToCharArray();
                EscreverLinha(caracteres);
                PulaLinha();
                Console.WriteLine("     Pressione qualquer tecla para sair...");
                Console.Read();
                return;
            }

            PulaLinha();

            caracteres = $"     Entendido iremos agora gerar as suas {quantidadeDeJogadas} jogadas, contendo {quantidadeDeNumeros} números da sorte cada, no intervalo entre {menorNumero} e {maiorNumero}!".ToCharArray();
            EscreverLinha(caracteres);

            PulaLinha();

            PequenaPausa();

            PulaLinha();

            var jogadas = new List<string>(quantidadeDeJogadas);
            var indiceJogadas = new HashSet<string>();
            int tentativas = 0;
            int maximoTentativas = Math.Max(quantidadeDeJogadas * MultiplicadorMaximoTentativas, 1000);

            while (jogadas.Count < quantidadeDeJogadas && tentativas < maximoTentativas)
            {
                var numerosDaSorte = GerarJogoUnico(random, menorNumero, maiorNumero, quantidadeDeNumeros)
                    .OrderBy(x => x)
                    .Select(x => x < 10 ? $"0{x}" : x.ToString());

                string jogada = string.Join(" - ", numerosDaSorte);
                tentativas++;

                if (indiceJogadas.Add(jogada))
                    jogadas.Add(jogada);
            }

            if (jogadas.Count < quantidadeDeJogadas)
            {
                PulaLinha();
                caracteres = "     Não foi possível concluir todas as jogadas no tempo esperado. Tente reduzir a quantidade solicitada.".ToCharArray();
                EscreverLinha(caracteres);
                PulaLinha();
            }

            PulaLinha();

            foreach (var sorte in jogadas)
            {
                caracteres = $"     >> ".ToCharArray();
                Escrever(caracteres);

                caracteres = sorte.ToCharArray();

                EscreverLinha(caracteres);

                caracteres = $" <<".ToCharArray();
                Escrever(caracteres);
                PulaLinha();

                MenorPausa();
            }

            PulaLinha();
            PulaLinha();

            PequenaPausa();
            caracteres = $"     Aqui estão as suas {quantidadeDeJogadas} jogadas, contendo {quantidadeDeNumeros} números da sorte cada!".ToCharArray();
            EscreverLinha(caracteres);
            PulaLinha();
            PulaLinha();
            caracteres = "     Boa Sorte!".ToCharArray();
            EscreverLinha(caracteres);
            PulaLinha();
            PulaLinha();

            Console.WriteLine("     Pressione qualquer tecla para sair...");
            Console.Read();
        }

        private static void MenorPausa()
        {
            Thread.Sleep(AtrasoMenorMs);
        }

        private static void Escrever(char[] caracteres)
        {
            Console.Write(caracteres);
        }

        private static void Espaco()
        {
            Console.Write(Indentacao);
        }

        private static void PequenaPausa()
        {
            Thread.Sleep(AtrasoPequenoMs);
        }

        private static void PulaLinha()
        {
            Console.WriteLine();
        }

        private static void EscreverLinha(char[] caracteres)
        {
            foreach (var caracter in caracteres)
            {
                Thread.Sleep(AtrasoAnimacaoMs);
                Console.Write(caracter);
            }
        }

        private static int LerInteiroPositivo(string mensagem)
        {
            int valor;

            while (true)
            {
                EscreverLinha((Indentacao + mensagem).ToCharArray());

                PulaLinha();
                PulaLinha();
                Espaco();

                if (Int32.TryParse(Console.ReadLine(), out valor) && valor > 0)
                    return valor;

                PulaLinha();
                EscreverLinha((Indentacao + "Valor inválido. Digite um número inteiro maior que zero.").ToCharArray());
                PulaLinha();
            }
        }

        private static bool PodeGerarQuantidadeSolicitada(int n, int k, int quantidadeSolicitada, out long maximoPossivel)
        {
            maximoPossivel = CombinacaoLimitada(n, k, quantidadeSolicitada);
            return maximoPossivel >= quantidadeSolicitada;
        }

        private static long CombinacaoLimitada(int n, int k, int limite)
        {
            if (k < 0 || n < 0 || k > n)
                return 0;

            if (k == 0 || k == n)
                return 1;

            if (k > n - k)
                k = n - k;

            decimal resultado = 1m;
            decimal limiteDecimal = limite;

            for (int i = 1; i <= k; i++)
            {
                resultado *= (n - (k - i));
                resultado /= i;

                if (resultado >= limiteDecimal)
                    return limite;
            }

            return (long)resultado;
        }

        private static IEnumerable<int> GerarJogoUnico(Random random, int menorNumero, int maiorNumero, int quantidadeDeNumeros)
        {
            var universo = Enumerable.Range(menorNumero, (maiorNumero - menorNumero) + 1).ToList();

            for (int i = 0; i < quantidadeDeNumeros; i++)
            {
                int indiceSorteado = random.Next(i, universo.Count);
                int troca = universo[i];
                universo[i] = universo[indiceSorteado];
                universo[indiceSorteado] = troca;
            }

            return universo.Take(quantidadeDeNumeros);
        }
    }
}
