using System.Text.Json.Serialization;

namespace WebAPI_Video.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DepartamentoEnum
    {
        RH,
        FINANCEIRO,
        Compras,
        Atendimento,
        Zeladoria
    }
}
