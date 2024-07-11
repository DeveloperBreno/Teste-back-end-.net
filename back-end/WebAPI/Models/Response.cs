namespace WebAPI.Models
{
    public class Response<T>
    {
        public T Result { get; set; }  // Propriedade para armazenar o resultado da operação
        public string Mensagem { get; set; }  // Propriedade para armazenar uma mensagem

        // Construtor padrão
        public Response() { }

        // Construtor com parâmetros para facilitar a criação de instâncias
        public Response(T result, string mensagem)
        {
            Result = result;
            Mensagem = mensagem;
        }
    }

}
