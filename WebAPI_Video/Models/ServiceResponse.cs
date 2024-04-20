namespace WebAPI_Video.Models
{
    public class ServiceResponse <T>
    {
        public T?   Dados { get; set; }
        public string Mensagem { get; set; }
        public bool Sucesso { get; set; } = true;


    }
}
