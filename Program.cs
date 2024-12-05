using System.Text.Json;
using ProjetoPessoal_ConversorDeMoedas.Models;

class Program
{
    static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();
        string apiKey = "456abc43878dbeac9c09fb33"; // sua chave de API

        bool continueConversion = true;

        void ExibirLogo()
        {
            Console.WriteLine(@"
░█████╗░░█████╗░███╗░░██╗██╗░░░██╗███████╗██████╗░░██████╗░█████╗░██████╗░  ██████╗░███████╗
██╔══██╗██╔══██╗████╗░██║██║░░░██║██╔════╝██╔══██╗██╔════╝██╔══██╗██╔══██╗  ██╔══██╗██╔════╝
██║░░╚═╝██║░░██║██╔██╗██║╚██╗░██╔╝█████╗░░██████╔╝╚█████╗░██║░░██║██████╔╝  ██║░░██║█████╗░░
██║░░██╗██║░░██║██║╚████║░╚████╔╝░██╔══╝░░██╔══██╗░╚═══██╗██║░░██║██╔══██╗  ██║░░██║██╔══╝░░
╚█████╔╝╚█████╔╝██║░╚███║░░╚██╔╝░░███████╗██║░░██║██████╔╝╚█████╔╝██║░░██║  ██████╔╝███████╗
░╚════╝░░╚════╝░╚═╝░░╚══╝░░░╚═╝░░░╚══════╝╚═╝░░╚═╝╚═════╝░░╚════╝░╚═╝░░╚═╝  ╚═════╝░╚══════╝

███╗░░░███╗░█████╗░███████╗██████╗░░█████╗░░██████╗
████╗░████║██╔══██╗██╔════╝██╔══██╗██╔══██╗██╔════╝
██╔████╔██║██║░░██║█████╗░░██║░░██║███████║╚█████╗░
██║╚██╔╝██║██║░░██║██╔══╝░░██║░░██║██╔══██║░╚═══██╗
██║░╚═╝░██║╚█████╔╝███████╗██████╔╝██║░░██║██████╔╝
╚═╝░░░░░╚═╝░╚════╝░╚══════╝╚═════╝░╚═╝░░╚═╝╚═════╝░");
        }
        

        while (continueConversion)
        {
            ExibirLogo();
            Console.WriteLine("\nOpções:");
            Console.WriteLine("Digite 1 para iniciar a conversão de moedas");
            Console.WriteLine("Digite 2 para ver a lista de códigos de moedas");
            Console.WriteLine("Digite -1 para sair");
            Console.Write("\nDigite a sua opção: ");
            string menuOption = Console.ReadLine();

            if (menuOption == "-1")
            {
                continueConversion = false;
                Console.WriteLine("\nSaindo do programa...");
                break;
            }

            if (menuOption == "2")
            {
                Console.Clear();
                string titulo2 = "Lista de Códigos de Moedas!";
                int quantidadeDeLetras2 = titulo2.Length;
                string asteriscos2 = string.Empty.PadLeft(quantidadeDeLetras2, '*');
                Console.WriteLine(asteriscos2);
                Console.WriteLine(titulo2);
                Console.WriteLine(asteriscos2 + "\n");
                Console.WriteLine("Para ver a lista completa de códigos de moedas, acesse:");
                Console.WriteLine("https://www.exchangerate-api.com/docs/supported-currencies");
                Console.WriteLine("\nDigite uma tecla para voltar ao menu principal");
                Console.ReadKey();
                Console.Clear();
                continue;
            }

            Console.Clear();

            string titulo1 = "Conversor de Moedas!";
            int quantidadeDeLetras1 = titulo1.Length;
            string asteriscos1 = string.Empty.PadLeft(quantidadeDeLetras1, '*');
            Console.WriteLine(asteriscos1);
            Console.WriteLine(titulo1);
            Console.WriteLine(asteriscos1 + "\n");

            Console.Write("Digite a moeda base (ex: BRL): ");
            string baseCurrency = Console.ReadLine().ToUpper();

            string url = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/{baseCurrency}";

            try
            {
                var response = await httpClient.GetStringAsync(url);

                var exchangeRates = JsonSerializer.Deserialize<ExchangeRatesResponse>(response);

                if (exchangeRates == null || exchangeRates.TaxaDeConversao == null)
                {
                    Console.WriteLine("Erro: Dados de câmbio não foram carregados corretamente.");
                    return;
                }

                Console.Write("Digite a moeda de destino (ex: USD): ");
                string targetCurrency = Console.ReadLine().ToUpper();

                Console.Write("Digite o valor a ser convertido: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    Console.WriteLine("Valor inválido.");
                    continue;
                }

                if (exchangeRates.TaxaDeConversao.TryGetValue(targetCurrency, out decimal rate))
                {
                    decimal convertedAmount = amount * rate;
                    Console.WriteLine($"\n{amount} {baseCurrency} equivale a {convertedAmount:F2} {targetCurrency} com a taxa {rate:F2}");
                    Console.WriteLine("\nDigite uma tecla para voltar ao menu principal");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Moeda de destino não encontrada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Tivemos um erro: {ex.Message}");
                Console.WriteLine("\nDigite uma tecla para voltar ao menu principal");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}