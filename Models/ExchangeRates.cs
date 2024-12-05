// Classe para deserializar a resposta JSON
using System.Text.Json.Serialization;

namespace ProjetoPessoal_ConversorDeMoedas.Models;

internal class ExchangeRatesResponse
{
    [JsonPropertyName("base_code")]
    public string CodBase { get; set; }

    [JsonPropertyName("conversion_rates")]
    public Dictionary<string, decimal> TaxaDeConversao { get; set; }
}
